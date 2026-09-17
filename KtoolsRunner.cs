using System;
using System.Diagnostics;
using System.IO;

public static class KtoolsRunner
{
    private static readonly string KtechPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ktech.exe");
    private static readonly string KranePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "krane.exe");

    public static string RunKtech(string inputTex, string outputPng)
    {
        return RunKtoolsTool(KtechPath, $"\"{inputTex}\" \"{outputPng}\"");
    }

    public static string RunKrane(string sourceDir, string outputDir)
    {
        // krane ожидает папку, где лежат build.bin и anim.bin
        var args = $"\"{sourceDir}\" \"{outputDir}\"";
        return RunKtoolsTool(KranePath, args);
    }

    public static string RunKtechBatch(string inputDir, string outputDir)
    {
        Directory.CreateDirectory(outputDir);
        var texFiles = Directory.GetFiles(inputDir, "*.tex");

        var log = new System.Text.StringBuilder();

        foreach (var tex in texFiles)
        {
            string fileName = Path.GetFileNameWithoutExtension(tex);
            string outPath = Path.Combine(outputDir, fileName + ".png");

            try
            {
                string result = RunKtech(tex, outPath);
                log.AppendLine($"[{Path.GetFileName(tex)}] {result}");
            }
            catch (Exception ex)
            {
                log.AppendLine($"[{Path.GetFileName(tex)}] ERROR: {ex.Message}");
            }
        }

        return log.ToString();
    }

    public static string RunKraneBatch(string sourceDir, string outputDir)
    {
        Directory.CreateDirectory(outputDir);

        // Ищем все build.bin в папке
        var buildFiles = Directory.GetFiles(sourceDir, "*build.bin", SearchOption.AllDirectories);
        var log = new System.Text.StringBuilder();

        foreach (var build in buildFiles)
        {
            string buildDir = Path.GetDirectoryName(build);
            string animPath = Path.Combine(buildDir, Path.GetFileNameWithoutExtension(build).Replace("build", "anim") + ".bin");

            if (!File.Exists(animPath))
            {
                log.AppendLine($"[{Path.GetFileName(build)}] Нет соответствующего anim-файла: {animPath}");
                continue;
            }

            string outProjectDir = Path.Combine(outputDir, Path.GetFileName(buildDir));

            try
            {
                // Передаём папку, где лежат оба файла
                string result = RunKrane(buildDir, outProjectDir);
                log.AppendLine($"[{Path.GetFileName(build)}] {result}");
            }
            catch (Exception ex)
            {
                log.AppendLine($"[{Path.GetFileName(build)}] ERROR: {ex.Message}");
            }
        }

        return log.ToString();
    }

    private static string RunKtoolsTool(string exePath, string arguments)
    {
        if (!File.Exists(exePath))
            throw new FileNotFoundException($"Не найден файл инструмента: {exePath}");

        var startInfo = new ProcessStartInfo
        {
            FileName = exePath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (var proc = Process.Start(startInfo))
        {
            string output = proc.StandardOutput.ReadToEnd();
            string error = proc.StandardError.ReadToEnd();
            proc.WaitForExit();

            return string.IsNullOrEmpty(error) ? output : error;
        }
    }
}