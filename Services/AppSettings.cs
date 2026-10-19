namespace KtoolsGui.Avalonia.Services;

public class AppSettings
{
    public string? KtoolsPath { get; set; }
    public string? InputFolder { get; set; }
    public string? OutputFolder { get; set; }
    public int Mode { get; set; }
    public bool SkipExisting { get; set; }
}