using Avalonia.Controls;
using Avalonia.Interactivity;
using KtoolsGui.Avalonia.Services;

namespace KtoolsGui.Avalonia;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();

        ApplyLanguage();

        CloseButton.Click += OnCloseClick;
    }

    private void OnCloseClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ApplyLanguage()
    {
        bool isEnglish = Localization.IsEnglish;

        if (isEnglish)
        {
            Title = "About";

            TitleText.Text = "ktoolsGUI";

            DescriptionText.Text =
                "A simple graphical interface for converting Don't Starve textures and extracting animations.";

            ToolsText.Text =
                "Uses ktech.exe and krane.exe from the ktools package.";

            CloseButton.Content = "Close";
        }
        else
        {
            Title = "О программе";

            TitleText.Text = "ktoolsGUI";

            DescriptionText.Text =
                "Простой графический интерфейс для конвертации текстур Don't Starve и извлечения анимаций.";

            ToolsText.Text =
                "Использует ktech.exe и krane.exe из пакета ktools.";

            CloseButton.Content = "Закрыть";
        }
    }
}