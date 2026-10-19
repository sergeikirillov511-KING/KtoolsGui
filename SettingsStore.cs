using System;
using System.IO;
using System.Text.Json;

namespace KtoolsGui.Avalonia.Services;

public static class SettingsStore
{
    private static readonly string SettingsFolder =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ktoolsGUI");

    private static readonly string SettingsFile =
        Path.Combine(SettingsFolder, "settings.json");

    public static string LoadKtoolsPath()
    {
        return Load().KtoolsPath ?? "";
    }

    public static string LoadInputFolder()
    {
        return Load().InputFolder ?? "";
    }

    public static string LoadOutputFolder()
    {
        return Load().OutputFolder ?? "";
    }

    public static int LoadMode()
    {
        return Load().Mode;
    }

    public static bool LoadSkipExisting()
    {
        return Load().SkipExisting;
    }

    public static void Save(
        string ktoolsPath,
        string inputFolder,
        string outputFolder,
        int mode,
        bool skipExisting)
    {
        Directory.CreateDirectory(SettingsFolder);

        var settings = new AppSettings
        {
            KtoolsPath = ktoolsPath,
            InputFolder = inputFolder,
            OutputFolder = outputFolder,
            Mode = mode,
            SkipExisting = skipExisting
        };

        string json = JsonSerializer.Serialize(
            settings,
            new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(SettingsFile, json);
    }

    private static AppSettings Load()
    {
        try
        {
            if (!File.Exists(SettingsFile))
            {
                return new AppSettings();
            }

            string json = File.ReadAllText(SettingsFile);

            return JsonSerializer.Deserialize<AppSettings>(json)
                   ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }
}