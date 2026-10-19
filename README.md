# KtoolsGUI

Графический интерфейс для утилит **ktech** и **krane** из набора ktools — для работы с ресурсами игр Klei (Don't Starve и др.).

A graphical interface for the **ktech** and **krane** utilities from the ktools suite — for working with Klei game assets (Don't Starve and others).

KtoolsGUI построен на **Avalonia UI** — кроссплатформенном .NET-фреймворке для создания графических интерфейсов, что позволяет использовать одно и то же приложение на Windows, Linux и macOS.

KtoolsGUI is built with **Avalonia UI**, a cross-platform .NET framework for building graphical interfaces, allowing the same application to run on Windows, Linux, and macOS.

---

## Русский

### Сборки

| Платформа | Архив |
|---|---|
| Windows 64-bit | `KtoolsGUI-win-x64.zip` |
| Linux 64-bit | `KtoolsGUI-linux-x64.zip` |
| macOS Apple Silicon (M1/M2/M3/M4) | `KtoolsGUI-osx-arm64.zip` |


### Состав архива

- `KtoolsGui.Avalonia` — графическое приложение
- `ktech` — конвертер текстур KTEX
- `krane` — работа с анимациями и моделями
- `README.txt` — краткая справка

### Запуск

Распакуйте архив в любую папку и запустите `KtoolsGui.Avalonia`.

**macOS:** при первом запуске может понадобиться снять блокировку Gatekeeper:

```bash
xattr -cr /путь/к/KtoolsGui.Avalonia
```

---

## English

### Builds

| Platform | Archive |
|---|---|
| Windows 64-bit | `KtoolsGUI-win-x64.zip` |
| Linux 64-bit | `KtoolsGUI-linux-x64.zip` |
| macOS Apple Silicon (M1/M2/M3/M4) | `KtoolsGUI-osx-arm64.zip` |


### Archive contents

- `KtoolsGui.Avalonia` — the graphical application
- `ktech` — KTEX texture converter
- `krane` — animation and model tools
- `README.txt` — quick reference

### Running

Extract the archive to any folder and run `KtoolsGui.Avalonia`.

**macOS:** on first launch you may need to remove the Gatekeeper quarantine:

```bash
xattr -cr /path/to/KtoolsGui.Avalonia
```
