using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace KtoolsGui
{
    public partial class MainWindow : Window
    {
        private bool _isRussian = false;

        public MainWindow()
        {
            InitializeComponent();
            _isRussian = false;  // Английский по умолчанию
            UpdateLanguage();    // Обновляем интерфейс
        }

        // === Выход ===
        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // === О программе ===
        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                _isRussian
                    ? "Ktools GUI\n\nГрафический интерфейс для ktools (ktech, krane)."
                    : "Ktools GUI\n\nGraphical interface for ktools (ktech, krane).",
                _isRussian ? "О программе" : "About",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        // === Язык ===
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

        private void UpdateLanguage()
        {
            if (_isRussian)
            {
                Title = "Ktools GUI";
                ((MenuItem)MainMenu.Items[0]).Header = "Файл";
                ((MenuItem)((MenuItem)MainMenu.Items[0]).Items[0]).Header = "Выход";
                ((MenuItem)MainMenu.Items[1]).Header = "Помощь";
                ((MenuItem)((MenuItem)MainMenu.Items[1]).Items[0]).Header = "О программе";
                ((MenuItem)((MenuItem)MainMenu.Items[1]).Items[1]).Header = "Язык / Language";
                ((TabItem)MainTabControl.Items[0]).Header = "📁 Текстуры";
                ((TabItem)MainTabControl.Items[1]).Header = "🎬 Анимации";

                // Текстуры
                TexDirectTitle.Text = "🔄 Прямая конвертация (.tex → .png)";
                TexInputTextLabel.Text = "📂 Входная папка (.tex):";
                TexInputFolderButton.Content = "Выбрать";
                TexOutputTextLabel.Text = "📤 Выходная папка (.png):";
                TexOutputFolderButton.Content = "Выбрать";
                ConvertTexturesButton.Content = "🚀 Конвертировать .tex → .png";
                TexBackTitle.Text = "🔄 Обратная конвертация (.png → .tex)";
                TexBackInputTextLabel.Text = "📂 Входная папка (.png):";
                TexBackInputFolderButton.Content = "Выбрать";
                TexBackOutputTextLabel.Text = "📤 Выходная папка (.tex):";
                TexBackOutputFolderButton.Content = "Выбрать";
                ConvertTexturesBackButton.Content = "🔄 Собрать .png → .tex";

                // Анимации
                AnimExtractTitle.Text = "🔄 Извлечение анимаций";
                AnimInputTextLabel.Text = "📂 Исходная папка:";
                AnimBuildFolderButton.Content = "Выбрать";
                AnimOutputTextLabel.Text = "📤 Выходная папка:";
                AnimOutputFolderButton.Content = "Выбрать";
                ConvertAnimationsButton.Content = "🚀 Извлечь (anim.bin и build.bin + .tex)";
                AnimAssemblyTitle.Text = "⚠️ Сборка анимаций";
                AnimSpriterText1.Text = "❗ Для сборки анимаций обратно в .bin";
                AnimSpriterText2.Text = "   требуется Spriter (http://www.brashmonkey.com/spriter.htm)";
                DownloadSpriterButton.Content = "📥 Скачать Spriter";
            }
            else
            {
                Title = "Ktools GUI";
                ((MenuItem)MainMenu.Items[0]).Header = "File";
                ((MenuItem)((MenuItem)MainMenu.Items[0]).Items[0]).Header = "Exit";
                ((MenuItem)MainMenu.Items[1]).Header = "Help";
                ((MenuItem)((MenuItem)MainMenu.Items[1]).Items[0]).Header = "About";
                ((MenuItem)((MenuItem)MainMenu.Items[1]).Items[1]).Header = "Language";
                ((TabItem)MainTabControl.Items[0]).Header = "📁 Textures";
                ((TabItem)MainTabControl.Items[1]).Header = "🎬 Animations";

                // Textures
                TexDirectTitle.Text = "🔄 Direct Conversion (.tex → .png)";
                TexInputTextLabel.Text = "📂 Input Folder (.tex):";
                TexInputFolderButton.Content = "Browse";
                TexOutputTextLabel.Text = "📤 Output Folder (.png):";
                TexOutputFolderButton.Content = "Browse";
                ConvertTexturesButton.Content = "🚀 Convert .tex → .png";
                TexBackTitle.Text = "🔄 Reverse Conversion (.png → .tex)";
                TexBackInputTextLabel.Text = "📂 Input Folder (.png):";
                TexBackInputFolderButton.Content = "Browse";
                TexBackOutputTextLabel.Text = "📤 Output Folder (.tex):";
                TexBackOutputFolderButton.Content = "Browse";
                ConvertTexturesBackButton.Content = "🔄 Convert .png → .tex";

                // Animations
                AnimExtractTitle.Text = "🔄 Extract Animations";
                AnimInputTextLabel.Text = "📂 Source Folder:";
                AnimBuildFolderButton.Content = "Browse";
                AnimOutputTextLabel.Text = "📤 Output Folder:";
                AnimOutputFolderButton.Content = "Browse";
                ConvertAnimationsButton.Content = "🚀 Extract (anim.bin and build.bin + .tex)";
                AnimAssemblyTitle.Text = "⚠️ Animation Assembly";
                AnimSpriterText1.Text = "❗ Animation assembly requires Spriter software";
                AnimSpriterText2.Text = "   (http://www.brashmonkey.com/spriter.htm)";
                DownloadSpriterButton.Content = "📥 Download Spriter";
            }
        }

        // === ТЕКСТУРЫ ===

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
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{(_isRussian ? "Ошибка" : "Error")}: {ex.Message}",
                    _isRussian ? "Ошибка" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // === Обратная конвертация текстур ===

        private void TexBackInputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFolderDialog { Title = _isRussian ? "Выберите папку с .png" : "Select .png folder" };
            if (dlg.ShowDialog() == true)
                TexBackInputFolderBox.Text = dlg.FolderName;
        }

        private void TexBackOutputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFolderDialog { Title = _isRussian ? "Выберите папку для .tex" : "Select .tex folder" };
            if (dlg.ShowDialog() == true)
                TexBackOutputFolderBox.Text = dlg.FolderName;
        }

        private void ConvertTexturesBackButton_Click(object sender, RoutedEventArgs e)
        {
            string input = TexBackInputFolderBox.Text.Trim();
            string output = TexBackOutputFolderBox.Text.Trim();

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
                string log = KtoolsRunner.RunKtechBatchReverse(input, output);
                TexLogBox.Text = log;
                MessageBox.Show(
                    _isRussian ? "Обратная конвертация завершена." : "Reverse conversion completed.",
                    _isRussian ? "Готово" : "Done",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{(_isRussian ? "Ошибка" : "Error")}: {ex.Message}",
                    _isRussian ? "Ошибка" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // === АНИМАЦИИ ===

        private void AnimBuildFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFolderDialog { Title = _isRussian ? "Выберите папку с anim.bin и build.bin" : "Select anim.bin and build.bin folder" };
            if (dlg.ShowDialog() == true)
                AnimBuildFolderBox.Text = dlg.FolderName;
        }

        private void AnimOutputFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFolderDialog { Title = _isRussian ? "Выберите папку для PNG" : "Select PNG folder" };
            if (dlg.ShowDialog() == true)
                AnimOutputFolderBox.Text = dlg.FolderName;
        }

        private void ConvertAnimationsButton_Click(object sender, RoutedEventArgs e)
        {
            string binFolder = AnimBuildFolderBox.Text.Trim();
            string output = AnimOutputFolderBox.Text.Trim();

            if (string.IsNullOrEmpty(binFolder) || string.IsNullOrEmpty(output))
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
                string log = KtoolsRunner.RunKraneBatch(binFolder, output);
                AnimLogBox.Text = log;
                MessageBox.Show(
                    _isRussian ? "Конвертация анимаций завершена." : "Animation conversion completed.",
                    _isRussian ? "Готово" : "Done",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"{(_isRussian ? "Ошибка" : "Error")}: {ex.Message}",
                    _isRussian ? "Ошибка" : "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        // === Скачать Spriter ===
        private void DownloadSpriterButton_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.brashmonkey.com/spriter.htm");
        }
    }
}