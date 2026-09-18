using System.Diagnostics;
using System.IO;
using System.Text;

namespace KtoolsGui
{
    public static class KtoolsRunner
    {
        public static string RunKtechBatch(string inputFolder, string outputFolder)
        {
            var log = new StringBuilder();
            log.AppendLine($"🔄 Конвертация текстур (.tex → .png)");
            log.AppendLine($"📂 Вход: {inputFolder}");
            log.AppendLine($"📤 Выход: {outputFolder}");
            log.AppendLine();

            if (!Directory.Exists(inputFolder))
            {
                log.AppendLine($"❌ Папка не найдена: {inputFolder}");
                return log.ToString();
            }

            var texFiles = Directory.GetFiles(inputFolder, "*.tex");
            if (texFiles.Length == 0)
            {
                log.AppendLine("❌ .tex файлы не найдены!");
                return log.ToString();
            }

            log.AppendLine($"📁 Найдено файлов: {texFiles.Length}");
            log.AppendLine();

            var startInfo = new ProcessStartInfo
            {
                FileName = "ktech.exe",
                Arguments = $"\"{inputFolder}\" \"{outputFolder}\" batch-reverse",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    log.AppendLine("🚀 Запуск ktech batch...");
                    log.AppendLine(output);
                    if (!string.IsNullOrEmpty(error))
                        log.AppendLine($"⚠️ {error}");
                    log.AppendLine($"🎉 Завершено! Файлы в: {outputFolder}");
                }
            }

            return log.ToString();
        }

        public static string RunKtechBatchReverse(string inputFolder, string outputFolder)
        {
            var log = new StringBuilder();
            log.AppendLine($"🔄 Обратная конвертация текстур (.png → .tex)");
            log.AppendLine($"📂 Вход: {inputFolder}");
            log.AppendLine($"📤 Выход: {outputFolder}");
            log.AppendLine();

            if (!Directory.Exists(inputFolder))
            {
                log.AppendLine($"❌ Папка не найдена: {inputFolder}");
                return log.ToString();
            }

            var pngFiles = Directory.GetFiles(inputFolder, "*.png");
            if (pngFiles.Length == 0)
            {
                log.AppendLine("❌ .png файлы не найдены!");
                return log.ToString();
            }

            log.AppendLine($"📁 Найдено файлов: {pngFiles.Length}");
            log.AppendLine();

            // Конвертируем каждый PNG файл
            foreach (var pngFile in pngFiles)
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = "ktech.exe",
                    Arguments = $"\"{pngFile}\" \"{outputFolder}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (var process = Process.Start(startInfo))
                {
                    if (process != null)
                    {
                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();
                        process.WaitForExit();

                        log.AppendLine($"📄 {Path.GetFileName(pngFile)}:");
                        if (!string.IsNullOrEmpty(output))
                            log.AppendLine(output);
                        if (!string.IsNullOrEmpty(error))
                            log.AppendLine($"⚠️ {error}");
                    }
                }
            }

            log.AppendLine($"🎉 Завершено! Файлы в: {outputFolder}");

            return log.ToString();
        }

        public static string RunKraneBatch(string binFolder, string outputFolder)
        {
            var log = new StringBuilder();
            log.AppendLine($"🔄 Извлечение анимаций (.bin → SCML + PNG)");
            log.AppendLine($"📂 Папка .bin: {binFolder}");
            log.AppendLine($"📤 Выход: {outputFolder}");
            log.AppendLine();

            if (!Directory.Exists(binFolder))
            {
                log.AppendLine($"❌ Папка не найдена: {binFolder}");
                return log.ToString();
            }

            var animBin = Path.Combine(binFolder, "anim.bin");
            var buildBin = Path.Combine(binFolder, "build.bin");

            if (!File.Exists(animBin))
            {
                log.AppendLine("❌ anim.bin не найден!");
                return log.ToString();
            }

            if (!File.Exists(buildBin))
            {
                log.AppendLine("❌ build.bin не найден!");
                return log.ToString();
            }

            log.AppendLine($"✅ anim.bin: найден");
            log.AppendLine($"✅ build.bin: найден");
            log.AppendLine();

            var startInfo = new ProcessStartInfo
            {
                FileName = "krane.exe",
                Arguments = $"\"{binFolder}\" \"{outputFolder}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    log.AppendLine("🚀 Запуск krane...");
                    log.AppendLine(output);
                    if (!string.IsNullOrEmpty(error))
                        log.AppendLine($"⚠️ {error}");
                    log.AppendLine($"🎉 Завершено! SCML и PNG в: {outputFolder}");
                }
            }

            return log.ToString();
        }

        public static string RunKraneBatchReverse(string inputFolder, string outputFolder)
        {
            var log = new StringBuilder();
            log.AppendLine($"🔄 Сборка анимаций (SCML + PNG → .bin)");
            log.AppendLine($"📂 Вход: {inputFolder}");
            log.AppendLine($"📤 Выход: {outputFolder}");
            log.AppendLine();

            if (!Directory.Exists(inputFolder))
            {
                log.AppendLine($"❌ Папка не найдена: {inputFolder}");
                return log.ToString();
            }

            // Проверяем наличие SCML файла
            var scmlFiles = Directory.GetFiles(inputFolder, "*.scml");
            if (scmlFiles.Length == 0)
            {
                log.AppendLine("❌ .scml файлы не найдены!");
                log.AppendLine("   Для сборки нужен Spriter проект (.scml)");
                return log.ToString();
            }

            log.AppendLine($"📁 Найдено SCML: {scmlFiles.Length}");
            log.AppendLine();

            var startInfo = new ProcessStartInfo
            {
                FileName = "krane.exe",
                Arguments = $"\"{inputFolder}\" \"{outputFolder}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                if (process != null)
                {
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    log.AppendLine("🚀 Запуск krane...");
                    log.AppendLine(output);
                    if (!string.IsNullOrEmpty(error))
                        log.AppendLine($"⚠️ {error}");
                    log.AppendLine($"🎉 Завершено! .bin файлы в: {outputFolder}");
                }
            }

            return log.ToString();
        }
    }
}