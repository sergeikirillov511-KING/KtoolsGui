using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KtoolsGui.Avalonia.Services;

namespace KtoolsGui.Avalonia.Services;

public class ConversionResult
{
    public int Success { get; set; }
    public int Skipped { get; set; }
    public int Failed { get; set; }
}

public class KtoolsRunner
{
    private static readonly string[] ImageExtensions = { ".png" };
    private static readonly string[] TextureExtensions = { ".tex" };

    public async Task<ConversionResult> ConvertAsync(
        string ktoolsFolder,
        string inputFolder,
        string outputFolder,
        bool pngToTex,
        bool skipExisting,
        CancellationToken cancellationToken,
        IProgress<(int Current, int Total)>? progress = null,
        Action<string, bool>? log = null)
    {
        var result = new ConversionResult();

        string toolPath = GetToolPath(ktoolsFolder, "ktech");

        if (!File.Exists(toolPath))
        {
            throw new FileNotFoundException(toolPath);
        }

        var files = Directory
            .EnumerateFiles(inputFolder, "*", SearchOption.AllDirectories)
            .Where(file =>
            {
                var extension = Path.GetExtension(file).ToLowerInvariant();

                return pngToTex
                    ? ImageExtensions.Contains(extension)
                    : TextureExtensions.Contains(extension);
            })
            .ToList();

        if (files.Count == 0)
        {
            log?.Invoke(Localization.T("NoFiles"), false);
            return result;
        }

        log?.Invoke(Localization.T("FoundObjects", files.Count), false);
        log?.Invoke(Localization.T("Tool", toolPath), false);

        int total = files.Count;
        int current = 0;

        foreach (var inputFile in files)
        {
            current++;
            progress?.Report((current, total));

            cancellationToken.ThrowIfCancellationRequested();

            string relativePath = Path.GetRelativePath(inputFolder, inputFile);
            string outputFile = Path.ChangeExtension(
                Path.Combine(outputFolder, relativePath),
                pngToTex ? ".tex" : ".png");

            Directory.CreateDirectory(Path.GetDirectoryName(outputFile)!);

            if (skipExisting && File.Exists(outputFile))
            {
                result.Skipped++;
                log?.Invoke(Localization.T("Skipped", relativePath), false);
                continue;
            }

            log?.Invoke(
                pngToTex
                    ? Localization.T("PngToTex", relativePath)
                    : Localization.T("TexToPng", relativePath),
                false);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = toolPath,
                    Arguments = $"\"{inputFile}\" \"{outputFile}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                }
            };

            try
            {
                process.Start();

                await process.StandardOutput.ReadToEndAsync(cancellationToken);
                await process.StandardError.ReadToEndAsync(cancellationToken);
                await process.WaitForExitAsync(cancellationToken);

                bool created = File.Exists(outputFile);

                if (process.ExitCode == 0 && created)
                {
                    result.Success++;
                }
                else
                {
                    result.Failed++;

                    log?.Invoke(
                        pngToTex
                            ? Localization.T("NoTexCreated", relativePath)
                            : Localization.T("NoPngCreated", relativePath),
                        true);
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                result.Failed++;
                log?.Invoke(ex.Message, true);
            }
            finally
            {
                process.Dispose();
            }
        }

        return result;
    }
    private static string GetToolPath(string folder, string baseName)
    {
        string exeName = OperatingSystem.IsWindows()
            ? $"{baseName}.exe"
            : baseName;

        return Path.Combine(folder, exeName);
    }
    public async Task<ConversionResult> ExtractAnimationsAsync(
        string ktoolsFolder,
        string inputFolder,
        string outputFolder,
        CancellationToken cancellationToken,
        IProgress<(int Current, int Total)>? progress = null,
        Action<string, bool>? log = null)
    {
        var result = new ConversionResult();

        string kranePath = GetToolPath(ktoolsFolder, "krane");

        if (!File.Exists(kranePath))
        {
            throw new FileNotFoundException(kranePath);
        }

        var buildFiles = Directory
            .EnumerateFiles(inputFolder, "*.build.bin", SearchOption.AllDirectories)
            .ToList();

        if (buildFiles.Count == 0)
        {
            log?.Invoke(Localization.T("NoFiles"), false);
            return result;
        }

        log?.Invoke(Localization.T("FoundObjects", buildFiles.Count), false);

        int total = buildFiles.Count;
        int current = 0;

        foreach (var buildFile in buildFiles)
        {
            current++;
            progress?.Report((current, total));

            cancellationToken.ThrowIfCancellationRequested();

            string relativePath = Path.GetRelativePath(inputFolder, buildFile);
            string buildFolder = Path.GetDirectoryName(buildFile)!;

            log?.Invoke(Localization.T("Extracting", relativePath), false);

            string atlasFile = Directory
                .EnumerateFiles(buildFolder, "*.tex", SearchOption.TopDirectoryOnly)
                .FirstOrDefault();

            if (atlasFile is null)
            {
                result.Failed++;
                log?.Invoke(Localization.T("AtlasMissing", relativePath), true);
                continue;
            }

            string resultFolder = Path.Combine(
                outputFolder,
                Path.ChangeExtension(relativePath, null));

            Directory.CreateDirectory(resultFolder);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = kranePath,
                    Arguments = $"\"{atlasFile}\" \"{buildFile}\" \"{resultFolder}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    StandardOutputEncoding = Encoding.UTF8,
                    StandardErrorEncoding = Encoding.UTF8
                }
            };

            try
            {
                process.Start();

                await process.StandardOutput.ReadToEndAsync(cancellationToken);
                await process.StandardError.ReadToEndAsync(cancellationToken);
                await process.WaitForExitAsync(cancellationToken);

                bool hasScml = Directory
                    .EnumerateFiles(resultFolder, "*.scml", SearchOption.AllDirectories)
                    .Any();

                if (process.ExitCode == 0 && hasScml)
                {
                    result.Success++;
                    log?.Invoke(Localization.T("AnimationDone", resultFolder), false);
                }
                else
                {
                    result.Failed++;
                    log?.Invoke(Localization.T("KraneError", relativePath), true);
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                result.Failed++;
                log?.Invoke(ex.Message, true);
            }
            finally
            {
                process.Dispose();
            }
        }

        return result;
    }
}