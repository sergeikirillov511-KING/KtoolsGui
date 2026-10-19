using System.Collections.Generic;

namespace KtoolsGui.Avalonia.Services;

public static class Localization
{
    public static bool IsEnglish { get; set; }

    private static readonly Dictionary<string, string> Russian = new()
    {
        ["AppStarted"] = "Программа запущена.",
        ["FillPaths"] = "Заполните путь к ktools, исходную папку и папку результата.",
        ["InputMissing"] = "Исходная папка не существует: {0}",
        ["Processing"] = "Обработка...",
        ["Done"] = "Готово",
        ["Cancelled"] = "Отменено",
        ["CancelMessage"] = "Операция отменена.",
        ["Error"] = "Ошибка",
        ["Summary"] = "Готово. Успешно: {0}, пропущено: {1}, ошибок: {2}.",
        ["OutputMissing"] = "Папка результата ещё не существует.",
        ["OpenFailed"] = "Не удалось открыть папку: {0}",

        ["NoFiles"] = "Подходящие файлы не найдены.",
        ["FoundObjects"] = "Найдено объектов: {0}",
        ["Tool"] = "Инструмент: {0}",
        ["PngToTex"] = "PNG → TEX: {0}",
        ["TexToPng"] = "TEX → PNG: {0}",
        ["Skipped"] = "Пропущен: {0}",
        ["NoTexCreated"] = "Не создан TEX: {0}",
        ["NoPngCreated"] = "Не создан PNG: {0}",

        ["Extracting"] = "Извлечение анимации: {0}",
        ["AtlasCount"] = "Атласов: {0}",
        ["BuildMissing"] = "Не найден build.bin рядом с {0}",
        ["AtlasMissing"] = "Не найдены .tex-атласы рядом с {0}",
        ["KraneError"] = "krane завершился с ошибкой: {0}",
        ["ScmlMissing"] = "SCML-проект не создан: {0}",
        ["AnimationDone"] = "Готово: {0}",
        ["ScmlFound"] = "Найдено SCML: {0}",

        ["KtoolsFound"] = "ktools найден: ktech.exe и krane.exe присутствуют.",
        ["KtechMissing"] = "ktech.exe не найден в выбранной папке ktools.",
        ["KraneMissing"] = "krane.exe не найден в выбранной папке ktools.",

        ["OpenResultAsk"] = "Конвертация завершена. Открыть папку результата?"
    };

    private static readonly Dictionary<string, string> English = new()
    {
        ["AppStarted"] = "Application started.",
        ["FillPaths"] = "Fill in the ktools path, input folder and output folder.",
        ["InputMissing"] = "Input folder does not exist: {0}",
        ["Processing"] = "Processing...",
        ["Done"] = "Ready",
        ["Cancelled"] = "Cancelled",
        ["CancelMessage"] = "Operation cancelled.",
        ["Error"] = "Error",
        ["Summary"] = "Done. Success: {0}, skipped: {1}, failed: {2}.",
        ["OutputMissing"] = "Output folder does not exist yet.",
        ["OpenFailed"] = "Could not open folder: {0}",

        ["NoFiles"] = "No suitable files found.",
        ["FoundObjects"] = "Found objects: {0}",
        ["Tool"] = "Tool: {0}",
        ["PngToTex"] = "PNG → TEX: {0}",
        ["TexToPng"] = "TEX → PNG: {0}",
        ["Skipped"] = "Skipped: {0}",
        ["NoTexCreated"] = "TEX was not created: {0}",
        ["NoPngCreated"] = "PNG was not created: {0}",

        ["Extracting"] = "Extracting animation: {0}",
        ["AtlasCount"] = "Atlas files: {0}",
        ["BuildMissing"] = "build.bin was not found near {0}",
        ["AtlasMissing"] = "No .tex atlas files were found near {0}",
        ["KraneError"] = "krane finished with an error: {0}",
        ["ScmlMissing"] = "SCML project was not created: {0}",
        ["AnimationDone"] = "Done: {0}",
        ["ScmlFound"] = "SCML files found: {0}",

        ["KtoolsFound"] = "ktools found: ktech.exe and krane.exe are present.",
        ["KtechMissing"] = "ktech.exe was not found in the selected ktools folder.",
        ["KraneMissing"] = "krane.exe was not found in the selected ktools folder.",

        ["OpenResultAsk"] = "Conversion finished. Open the output folder?"
    };

    public static string T(string key, params object[] args)
    {
        var dictionary = IsEnglish ? English : Russian;

        string format = dictionary.TryGetValue(key, out var value)
            ? value
            : key;

        return args.Length == 0 ? format : string.Format(format, args);
    }
}