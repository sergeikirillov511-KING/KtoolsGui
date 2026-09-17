#nullable disable

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace KtoolsGui
{
    public partial class MainWindow : Window
    {
        private bool _isRussian = true;

        public MainWindow()
        {
            InitializeComponent();

            // Устанавливаем фон
            var bitmap = new System.Windows.Media.Imaging.BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri("Assets/10th_Anniversary_Celebration.png", UriKind.Relative);
            bitmap.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();

            MainGrid.Background = new System.Windows.Media.ImageBrush
            {
                ImageSource = bitmap,
                Stretch = System.Windows.Media.Stretch.UniformToFill,
                Opacity = 0.15
            };
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateLanguage();
        }

        // === Обработчики меню ===
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var about = new AboutWindow(_isRussian);
            about.Owner = this;
            about.ShowDialog();
        }

        private void LangRu_Click(object sender, RoutedEventArgs e)
        {
            _isRussian = true;
            UpdateLanguage();
        }

        private void LangEn_Click(object sender, RoutedEventArgs e)
        {
            _isRussian = false;
            UpdateLanguage();
        }

        // === Обновление языка ===
        // === Обновление языка ===
        // === Обновление языка ===
        private void UpdateLanguage()
        {
            TabControl tabControl = null;

            // Меню
            if (this.FindName("MainMenu") is Menu menu && menu.Items.Count >= 2)
            {
                var fileItem = menu.Items[0] as MenuItem;
                var helpItem = menu.Items[1] as MenuItem;

                if (fileItem != null)
                {
                    fileItem.Header = _isRussian ? "Файл" : "File";
                    if (fileItem.Items.Count > 0)
                        (fileItem.Items[0] as MenuItem).Header = _isRussian ? "Выход" : "Exit";
                }

                if (helpItem != null)
                {
                    helpItem.Header = _isRussian ? "Помощь" : "Help";
                    if (helpItem.Items.Count > 0)
                        (helpItem.Items[0] as MenuItem).Header = _isRussian ? "О программе" : "About";
                    if (helpItem.Items.Count > 1)
                        (helpItem.Items[1] as MenuItem).Header = _isRussian ? "Язык / Language" : "Language / Язык";
                }
            }

            // Вкладки
            if (this.FindName("MainTabControl") is TabControl tc && tc.Items.Count >= 2)
            {
                tabControl = tc;
                (tabControl.Items[0] as TabItem).Header = _isRussian ? "📁 Текстуры" : "📁 Textures";
                (tabControl.Items[1] as TabItem).Header = _isRussian ? "🎬 Анимации" : "🎬 Animations";
            }

            // Тексты в первой вкладке (Текстуры)
            if (tabControl?.Items[0] is TabItem texturesTab && texturesTab.Content is Grid texturesGrid)
            {
                foreach (var row in GetChildrenOfType<Border>(texturesGrid))
                {
                    foreach (var panel in GetChildrenOfType<StackPanel>(row))
                    {
                        foreach (var tb in GetChildrenOfType<TextBlock>(panel))
                        {
                            if (tb.Text.Contains(".tex"))
                                tb.Text = _isRussian ? "📂 Входная папка (.tex):" : "📂 Input folder (.tex):";
                            else if (tb.Text.Contains(".png"))
                                tb.Text = _isRussian ? "📤 Выходная папка (.png):" : "📤 Output folder (.png):";
                        }
                    }
                }

                foreach (var btn in GetChildrenOfType<Button>(texturesGrid))
                {
                    if (btn.Content.ToString() == "Выбрать" || btn.Content.ToString() == "Select")
                        btn.Content = _isRussian ? "Выбрать" : "Select";
                    else if (btn.Content.ToString().Contains("Конвертировать текстуры") || btn.Content.ToString().Contains("Convert textures"))
                        btn.Content = _isRussian ? "🚀 Конвертировать текстуры" : "🚀 Convert textures";
                }
            }

            // Тексты во второй вкладке (Анимации)
            if (tabControl?.Items[1] is TabItem animTab && animTab.Content is Grid animGrid)
            {
                foreach (var row in GetChildrenOfType<Border>(animGrid))
                {
                    foreach (var panel in GetChildrenOfType<StackPanel>(row))
                    {
                        foreach (var tb in GetChildrenOfType<TextBlock>(panel))
                        {
                            if (tb.Text.Contains("Исходная") || tb.Text.Contains("Source"))
                                tb.Text = _isRussian ? "📂 Исходная папка:" : "📂 Source folder:";
                            else if (tb.Text.Contains("Выходная") || tb.Text.Contains("Output folder"))
                                tb.Text = _isRussian ? "📤 Выходная папка:" : "📤 Output folder:";
                        }
                    }
                }

                foreach (var btn in GetChildrenOfType<Button>(animGrid))
                {
                    if (btn.Content.ToString() == "Выбрать" || btn.Content.ToString() == "Select")
                        btn.Content = _isRussian ? "Выбрать" : "Select";
                    else if (btn.Content.ToString().Contains("Конвертировать анимации") || btn.Content.ToString().Contains("Convert animations"))
                        btn.Content = _isRussian ? "🚀 Конвертировать анимации" : "🚀 Convert animations";
                }
            }

            // About окно (если открыто)
            if (Application.Current.Windows.OfType<AboutWindow>().FirstOrDefault() is AboutWindow about)
                about.UpdateLanguage(_isRussian);
        }

        // Вспомогательный метод для поиска элементов
        private IEnumerable<T> GetChildrenOfType<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) yield break;

            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                    yield return typedChild;

                foreach (var grandChild in GetChildrenOfType<T>(child))
                    yield return grandChild;
            }
        }

        // === Текстуры ===
        private void TexInputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFolderDialog { Title = _isRussian ? "Выберите папку с .tex" : "Select .tex folder" };
            if (dlg.ShowDialog() == true)
                TexInputFolderBox.Text = dlg.FolderName;
        }

        private void TexOutputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFolderDialog { Title = _isRussian ? "Выберите папку для .png" : "Select .png folder" };
            if (dlg.ShowDialog() == true)
                TexOutputFolderBox.Text = dlg.FolderName;
        }

        private void ConvertTexturesButton_Click(object sender, RoutedEventArgs e)
        {
            string input = TexInputFolderBox.Text.Trim();
            string output = TexOutputFolderBox.Text.Trim();

            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(output))
            {
                MessageBox.Show(
                    _isRussian ? "Выберите входную и выходную папки." : "Select input and output folders.",
                    _isRussian ? "Ошибка" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                string log = KtoolsRunner.RunKtechBatch(input, output);
                TexLogBox.Text = log;
                MessageBox.Show(
                    _isRussian ? "Конвертация текстур завершена." : "Texture conversion completed.",
                    _isRussian ? "Готово" : "Done",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(
                    $"{(_isRussian ? "Ошибка" : "Error")}: {ex.Message}",
                    _isRussian ? "Ошибка" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // === Анимации ===
        private void AnimBuildFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFolderDialog { Title = _isRussian ? "Выберите папку с build.bin" : "Select build.bin folder" };
            if (dlg.ShowDialog() == true)
                AnimBuildFolderBox.Text = dlg.FolderName;
        }

        private void AnimOutputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFolderDialog { Title = _isRussian ? "Выберите выходную папку" : "Select output folder" };
            if (dlg.ShowDialog() == true)
                AnimOutputFolderBox.Text = dlg.FolderName;
        }

        private void ConvertAnimationsButton_Click(object sender, RoutedEventArgs e)
        {
            string source = AnimBuildFolderBox.Text?.Trim() ?? "";
            string output = AnimOutputFolderBox.Text?.Trim() ?? "";

            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(output))
            {
                MessageBox.Show(
                    _isRussian ? "Выберите исходную и выходную папки." : "Select source and output folders.",
                    _isRussian ? "Ошибка" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            try
            {
                string log = KtoolsRunner.RunKraneBatch(source, output);
                AnimLogBox.Text = log;
                MessageBox.Show(
                    _isRussian ? "Конвертация анимаций завершена." : "Animation conversion completed.",
                    _isRussian ? "Готово" : "Done",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(
                    $"{(_isRussian ? "Ошибка" : "Error")}: {ex.Message}",
                    _isRussian ? "Ошибка" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}