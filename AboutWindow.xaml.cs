#nullable disable

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;

namespace KtoolsGui
{
    public partial class AboutWindow : Window
    {
        private bool _isRussian = true;

        public AboutWindow(bool isRussian = true)
        {
            InitializeComponent();
            _isRussian = isRussian;
            this.Loaded += AboutWindow_Loaded;

            // Устанавливаем иконку
            try
            {
                this.Icon = new System.Windows.Media.Imaging.BitmapImage(
                    new Uri("Assets/KtoolGUI_Backround.ico", UriKind.Relative));
            }
            catch
            {
                // Если иконка не загрузится, будет стандартная
            }
        }

        private void AboutWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateLabels();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void UpdateLanguage(bool isRussian)
        {
            _isRussian = isRussian;
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            // Заголовок окна
            Title = _isRussian ? "О программе" : "About";

            // Ищем все TextBlock и Button в окне
            foreach (var tb in GetChildrenOfType<TextBlock>(this))
            {
                if (tb.Text == "Ktools")
                {
                    // Не меняем
                }
                else if (tb.Text.Contains("Версия") || tb.Text.Contains("Version"))
                {
                    tb.Text = _isRussian ? "Версия 4.5.1" : "Version 4.5.1";
                }
                else if (tb.Text.Contains("Программа для обработки") || tb.Text.Contains("Texture and animation"))
                {
                    tb.Text = _isRussian ? "Программа для обработки текстур и анимаций" : "Texture and animation converter";
                }
                else if (tb.Text.Contains("© 2026"))
                {
                    // Не меняем
                }
            }

            // Кнопка
            foreach (var btn in GetChildrenOfType<Button>(this))
            {
                if (btn.Content.ToString() == "Закрыть" || btn.Content.ToString() == "Close")
                {
                    btn.Content = _isRussian ? "Закрыть" : "Close";
                }
            }
        }

        private IEnumerable<T> GetChildrenOfType<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) yield break;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typedChild)
                    yield return typedChild;

                foreach (var grandChild in GetChildrenOfType<T>(child))
                    yield return grandChild;
            }
        }
    }
}