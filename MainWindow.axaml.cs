using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using KtoolsGui.Avalonia.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace KtoolsGui.Avalonia;
public class LogEntry
{
    public string Text { get; set; } = "";
    public IBrush Color { get; set; } = Brushes.Gainsboro;
}
public partial class MainWindow : Window
{
    private readonly SettingsService _settingsService = new();
    private readonly KtoolsRunner _runner = new();
    private CancellationTokenSource? _cts;
    private bool _isEnglish;
    private readonly ObservableCollection<LogEntry> _logEntries = new();
    private void CheckKtoolsFolder(string folder)
    {
        if (string.IsNullOrWhiteSpace(folder) || !Directory.Exists(folder))
        {
            return;
        }

        string ktechName = OperatingSystem.IsWindows() ? "ktech.exe" : "ktech";
        string kraneName = OperatingSystem.IsWindows() ? "krane.exe" : "krane";

        string ktechPath = Path.Combine(folder, ktechName);
        string kranePath = Path.Combine(folder, kraneName);

        bool hasKtech = File.Exists(ktechPath);
        bool hasKrane = File.Exists(kranePath);

        if (hasKtech && hasKrane)
        {
            Log(Localization.T("KtoolsFound"));
        }

        if (!hasKtech)
        {
            Log(Localization.T("KtechMissing"), true);
        }

        if (!hasKrane)
        {
            Log(Localization.T("KraneMissing"), true);
        }

        if (hasKtech)
        {
            EnsureExecutable(ktechPath);
        }

        if (hasKrane)
        {
            EnsureExecutable(kranePath);
        }
    }
    public MainWindow()
    {
        InitializeComponent();
        
        LogList.ItemsSource = _logEntries;
        
        ClearLogButton.Click += OnClearLog;

        _runner = new KtoolsRunner();

        Localization.IsEnglish = _isEnglish;

        ApplyLanguage();

        KtoolsPathBox.Text = SettingsStore.LoadKtoolsPath();
        InputFolderBox.Text = SettingsStore.LoadInputFolder();
        OutputFolderBox.Text = SettingsStore.LoadOutputFolder();

        ModeBox.SelectedIndex = Math.Clamp(SettingsStore.LoadMode(), 0, 2);
        SkipExistingBox.IsChecked = SettingsStore.LoadSkipExisting();

        Closing += (_, _) => SaveSettings();
    }
    private void OnClearLog(object? sender, RoutedEventArgs e)
    {
        _logEntries.Clear();
    }
    private void SaveSettings()
    {
        SettingsStore.Save(
            KtoolsPathBox.Text?.Trim() ?? "",
            InputFolderBox.Text?.Trim() ?? "",
            OutputFolderBox.Text?.Trim() ?? "",
            ModeBox.SelectedIndex,
            SkipExistingBox.IsChecked == true);
    }
    private void Log(string message, bool isError = false)
    {
        if (!Dispatcher.UIThread.CheckAccess())
        {
            Dispatcher.UIThread.Post(() => Log(message, isError));
            return;
        }

        var entry = new LogEntry
        {
            Text = $"{DateTime.Now:HH:mm:ss}  {message}",
            Color = isError ? Brushes.IndianRed : Brushes.Gainsboro
        };

        _logEntries.Add(entry);
        LogList.ScrollIntoView(entry);
    }

    private async void OnBrowseKtools(object? sender, RoutedEventArgs e)
    {
        var folder = await PickFolderAsync("Выберите папку ktools");

        if (folder != null)
        {
            KtoolsPathBox.Text = folder;

            SaveSettings();
            CheckKtoolsFolder(folder);
        }
    }

    private async void OnBrowseInput(object? sender, RoutedEventArgs e)
    {
        var folders = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Выберите исходную папку",
            AllowMultiple = false
        });

        if (folders.Count > 0)
        {
            InputFolderBox.Text = folders[0].Path.LocalPath;
            SaveSettings();
        }
    }

    private async void OnBrowseOutput(object? sender, RoutedEventArgs e)
    {
        var folder = await PickFolderAsync("Выберите папку результата");
        if (folder != null) OutputFolderBox.Text = folder;
        SaveSettings();
    }

    private void OnOpenOutput(object? sender, RoutedEventArgs e)
    {
        string path = OutputFolderBox.Text?.Trim() ?? "";

        if (!Directory.Exists(path))
        {
            Log(Localization.T("OutputMissing"));
            return;
        }

        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"\"{path}\"",
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Log(Localization.T("OpenFailed", ex.Message));
        }
    }
    private void OpenFolder(string folder)
    {
        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = folder,
                UseShellExecute = true
            });
        }
        catch (Exception ex)
        {
            Log(Localization.T("OpenFailed", ex.Message), true);
        }
    }
    private async void OnConvert(object? sender, RoutedEventArgs e)
    {
        var ktoolsFolder = KtoolsPathBox.Text?.Trim() ?? "";
        var inputFolder = InputFolderBox.Text?.Trim() ?? "";
        var outputFolder = OutputFolderBox.Text?.Trim() ?? "";
        var mode = ModeBox.SelectedIndex;
        var skipExisting = SkipExistingBox.IsChecked == true;

        SaveSettings();

        if (string.IsNullOrWhiteSpace(ktoolsFolder) ||
            string.IsNullOrWhiteSpace(inputFolder) ||
            string.IsNullOrWhiteSpace(outputFolder))
        {
            Log(Localization.T("FillPaths"));
            return;
        }

        if (!Directory.Exists(inputFolder))
        {
            Log(Localization.T("InputMissing", inputFolder));
            return;
        }

        if (!Directory.Exists(ktoolsFolder))
        {
            Log(Localization.T("InputMissing", ktoolsFolder));
            return;
        }

        _cts = new CancellationTokenSource();
        ConvertButton.IsEnabled = false;
        CancelButton.IsEnabled = true;

        var progress = new Progress<(int Current, int Total)>(value =>
        {
            StatusText.Text = $"{Localization.T("Processing")} {value.Current} / {value.Total}";
        });

        try
        {
            ConversionResult result;

            if (mode == 2)
            {
                result = await _runner.ExtractAnimationsAsync(
                    ktoolsFolder,
                    inputFolder,
                    outputFolder,
                    _cts.Token,
                    progress,
                    (message, isError) => Log(message, isError));
            }
            else
            {
                result = await _runner.ConvertAsync(
                    ktoolsFolder,
                    inputFolder,
                    outputFolder,
                    mode == 0,
                    skipExisting,
                    _cts.Token,
                    progress,
                    (message, isError) => Log(message, isError));
            }

            StatusText.Text = Localization.T("Done");
            Log(Localization.T("Summary", result.Success, result.Skipped, result.Failed));
            if (result.Success > 0 && Directory.Exists(outputFolder))
            {
                OpenFolder(outputFolder);
            }
        }
        catch (OperationCanceledException)
        {
            StatusText.Text = Localization.T("Cancelled");
            Log(Localization.T("CancelMessage"));
        }
        catch (Exception ex)
        {
            StatusText.Text = Localization.T("Error");
            Log(ex.Message, true);
        }
        finally
        {
            ConvertButton.IsEnabled = true;
            CancelButton.IsEnabled = false;
            _cts?.Dispose();
            _cts = null;
        }
    }

    private void OnCancel(object? sender, RoutedEventArgs e)
    {
        _cts?.Cancel();
    }

    private void OnLanguageChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (LanguageBox is null)
            return;

        _isEnglish = LanguageBox.SelectedIndex == 1;
        ApplyLanguage();
    }

    private void ApplyLanguage()
    {
        Localization.IsEnglish = _isEnglish;

        if (KtoolsLabel is null ||
            ModeLabel is null ||
            InputLabel is null ||
            OutputLabel is null ||
            KtoolsPathBox is null ||
            ModeBox is null ||
            InputFolderBox is null ||
            OutputFolderBox is null ||
            ConvertButton is null ||
            CancelButton is null ||
            AboutButton is null ||
            SkipExistingBox is null ||
            BrowseKtoolsButton is null ||
            BrowseInputButton is null ||
            BrowseOutputButton is null ||
            OpenOutputButton is null ||
            StatusText is null)
        {
            return;
        }

        if (_isEnglish)
        {
            Title = "ktoolsGUI";

            KtoolsLabel.Text = "ktools path";
            ModeLabel.Text = "Mode";
            InputLabel.Text = "Input folder";
            OutputLabel.Text = "Output folder";

            KtoolsPathBox.Watermark = "Folder containing ktech.exe and krane.exe";
            InputFolderBox.Watermark = "Folder with source files";
            OutputFolderBox.Watermark = "Folder for results";

            SetComboBoxItemText(ModeBox, 0, "PNG → TEX");
            SetComboBoxItemText(ModeBox, 1, "TEX → PNG");
            SetComboBoxItemText(ModeBox, 2, "Extract animations (krane)");

            ConvertButton.Content = "Start";
            CancelButton.Content = "Cancel";
            AboutButton.Content = "About";
            SkipExistingBox.Content = "Skip existing output files";

            BrowseKtoolsButton.Content = "Browse...";
            BrowseInputButton.Content = "Browse...";
            BrowseOutputButton.Content = "Browse...";
            OpenOutputButton.Content = "Open";
            StatusText.Text = "Ready";
            ClearLogButton.Content = "Clear log";
        }
        else
        {
            Title = "ktoolsGUI";

            KtoolsLabel.Text = "Путь к ktools";
            ModeLabel.Text = "Режим";
            InputLabel.Text = "Исходная папка";
            OutputLabel.Text = "Папка результата";

            KtoolsPathBox.Watermark = "Папка с ktech.exe и krane.exe";
            InputFolderBox.Watermark = "Папка с исходными файлами";
            OutputFolderBox.Watermark = "Папка для результатов";

            SetComboBoxItemText(ModeBox, 0, "PNG → TEX");
            SetComboBoxItemText(ModeBox, 1, "TEX → PNG");
            SetComboBoxItemText(ModeBox, 2, "Извлечь анимации (krane)");

            ConvertButton.Content = "Начать";
            CancelButton.Content = "Отмена";
            AboutButton.Content = "О программе";
            SkipExistingBox.Content = "Пропускать уже существующие файлы результата";

            BrowseKtoolsButton.Content = "Обзор...";
            BrowseInputButton.Content = "Обзор...";
            BrowseOutputButton.Content = "Обзор...";
            OpenOutputButton.Content = "Открыть";
            StatusText.Text = "Готово";
            ClearLogButton.Content = "Очистить журнал";
        }
    }
    private void EnsureExecutable(string filePath)
    {
        if (OperatingSystem.IsWindows() || !File.Exists(filePath))
        {
            return;
        }

        try
        {
            var process = Process.Start(new ProcessStartInfo
            {
                FileName = "/bin/chmod",
                Arguments = $"+x \"{filePath}\"",
                UseShellExecute = false,
                CreateNoWindow = true
            });

            process?.WaitForExit();
        }
        catch
        {
            // Если не получилось, пользователь сможет выполнить chmod вручную.
        }
    }
    private static void SetComboBoxItemText(ComboBox comboBox, int index, string text)
    {
        if (comboBox.Items is null || index >= comboBox.Items.Count)
            return;

        if (comboBox.Items[index] is ComboBoxItem item)
            item.Content = text;
    }

    private void OnAbout(object? sender, RoutedEventArgs e)
    {
        var aboutWindow = new AboutWindow();

        aboutWindow.ShowDialog(this);
    }

    private async Task<string?> PickFolderAsync(string title)
    {
        var result = await StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = title,
            AllowMultiple = false
        });

        return result.Count > 0 ? result[0].Path.LocalPath : null;
    }
}