# KtoolsGui

Graphical interface for Ktools utilities (ktex2png and krane). Convert textures and animations from Don't Starve Together with a simple click instead of command line.

![Platform](https://img.shields.io/badge/platform-Windows-blue)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![License](https://img.shields.io/badge/license-MIT-green)

---

## 📖 Table of Contents

- [About](#about)
- [Features](#features)
- [Requirements](#requirements)
- [Installation](#installation)
- [Usage](#usage)
- [Screenshots](#screenshots)
- [Build from Source](#build-from-source)
- [License](#license)
- [Contacts](#contacts)

---

## ℹ️ About

**KtoolsGui** is a free, open-source graphical tool designed for **Don't Starve Together (DST)** modders, artists, and developers. It wraps the official **Ktools** command-line utilities (`ktex2png` and `krane`) into a modern, user-friendly Windows application.

### Why KtoolsGui?

The original Ktools require manual command-line usage, which can be intimidating for beginners. KtoolsGui solves this by providing:

- **Visual interface** — No need to remember commands
- **Bilingual support** — Russian and English
- **Fast workflow** — One-click conversion
- **Clean design** — Modern UI with background styling

Perfect for extracting game assets for modding, animation reference, or texture editing!

---

## ✨ Features

| Feature | Description |
|---------|-------------|
| 📁 **Texture Conversion** | Batch convert `.tex` files to `.png` format |
| 🎬 **Animation Extraction** | Extract animation frames from `.build.bin` and `.anim.bin` files |
| 🌐 **Bilingual Interface** | Toggle between Russian (Русский) and English with one button |
| 🎨 **Modern Design** | Clean, intuitive UI with custom background and smooth styling |
| 🚀 **One-Click Processing** | No manual commands — just select folders and click |
| 📂 **Smart Folder Picker** | Built-in folder browser for easy input/output selection |
| ⚡ **Fast Performance** | Multi-threaded processing for large batches |
| 📦 **Portable** | No installation required — just run the `.exe` |

---

## 🖥️ Requirements

### Minimum Requirements

| Component | Version |
|-----------|---------|
| **Operating System** | Windows 10 (64-bit) or Windows 11 |
| **.NET Runtime** | .NET 8.0 Desktop Runtime |
| **RAM** | 512 MB |
| **Disk Space** | 50 MB |

### Install .NET 8.0 Runtime

1. Go to **[Download .NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)**
2. Click **Download .NET Desktop Runtime 8.0**
3. Run the installer and follow the prompts
4. Restart your computer (if required)

---

## 📥 Installation

### Option 1: Download Pre-built Version (Recommended)

1. **Go to Releases**
   - Visit: https://github.com/sergeikirillov511-KING/KtoolsGui/releases

2. **Download the Latest Version**
   - Click on the latest release (e.g., `v1.0`)
   - Download the `.zip` file (e.g., `KtoolsGui-v1.0.zip`)

3. **Extract the Archive**
   - Right-click the `.zip` file → **Extract All**
   - Choose a folder (e.g., `C:\Games\KtoolsGui\`)

4. **Run the Application**
   - Double-click `KtoolsGui.exe`
   - The application will launch immediately

### Option 2: Build from Source

See the **[Build from Source](#build-from-source)** section below.

---

## 🚀 Usage

### 🌐 Switch Language

- Click the **🌐 Language** button in the top-right corner
- The interface will instantly switch between **Russian (Русский)** and **English**
- Default language: **English** (can be changed in code)

---

### 📁 Texture Conversion

**Purpose:** Convert `.tex` texture files (used by DST) to standard `.png` images.

#### Step-by-Step Guide

1. **Launch KtoolsGui**
   - Run `KtoolsGui.exe`

2. **Open the Textures Tab**
   - Click the **📁 Textures** tab at the top

3. **Select Input Folder**
   - Click **📂 Select Input Folder**
   - Browse to the folder containing your `.tex` files
   - Example: `C:\DST_Mod\textures\`

4. **Select Output Folder**
   - Click **📂 Select Output Folder**
   - Choose where to save the converted `.png` files
   - Example: `C:\DST_Mod\output\`

5. **Start Conversion**
   - Click **🚀 Convert Textures**
   - Wait for the progress bar to complete

6. **Check Results**
   - Open the output folder
   - You should see `.png` files with the same names as the original `.tex` files

#### Example

**Input folder contents:**
character.tex
weapon.tex
background.tex

**Output folder contents:**
character.png
weapon.png
background.png

### 🎬 Animation Conversion

**Purpose:** Extract individual animation frames from DST animation files (`.build.bin` and `.build.atlas.bin`).

#### Step-by-Step Guide

1. **Launch KtoolsGui**
   - Run `KtoolsGui.exe`

2. **Open the Animations Tab**
   - Click the **🎬 Animations** tab at the top

3. **Select Input Folder**
   - Click **📂 Select Input Folder**
   - Browse to the folder containing `build.bin` and `build.atlas.bin`
   - Example: `C:\DST_Mod\animations\character\`

4. **Select Output Folder**
   - Click **📂 Select Output Folder**
   - Choose where to save the extracted frames
   - Example: `C:\DST_Mod\output\character_frames\`

5. **Start Conversion**
   - Click **🚀 Convert Animations**
   - Wait for the process to complete

6. **Check Results**
   - Open the output folder
   - You should see numbered `.png` files (e.g., `anim_0001.png`, `anim_0002.png`)

#### Example

**Input folder contents:**

build.bin
build.atlas.bin

**Output folder contents:**
anim_0001.png
anim_0002.png
anim_0003.png
...
anim_0120.png


#### Tips

- Make sure both `build.bin` and `build.atlas.bin` are in the same folder
- The number of output frames depends on the animation length
- Output files are named sequentially for easy import into animation software

---

## 📸 Screenshots

### Main Window

![Main Window](screenshots/Screenshot_4.png)
![Main Window](screenshots/Screenshot_6.png)

### Animation Extraction

![Animations](screenshots/Screenshot_5.png)
![Animations](screenshots/Screenshot_7.png)

---

## 🛠️ Build from Source

### Prerequisites

| Tool | Version | Download |
|------|---------|----------|
| **Visual Studio 2022** | Community or higher | [Download](https://visualstudio.microsoft.com/) |
| **.NET 8.0 SDK** | Latest | [Download](https://dotnet.microsoft.com/download/dotnet/8.0) |
| **Git** | Latest | [Download](https://git-scm.com/) |

### Step 1: Clone the Repository

Open **Command Prompt** or **PowerShell** and run:

```bash
git clone [https://github.com/sergeikirillov511-KING/KtoolsGui.git](https://github.com/sergeikirillov511-KING/KtoolsGui.git)
cd KtoolsGui
```

### Step 2: Open in Visual Studio

1. Open **Visual Studio 2022**
2. Click **Open a project or solution**
3. Navigate to the `KtoolsGui` folder
4. Select `KtoolsGui.csproj` or `KtoolsGui.sln`
5. Click **Open**

### Step 3: Build the Project

1. In Visual Studio, go to **Build → Build Solution** (or press `Ctrl+Shift+B`)
2. Wait for the build to complete
3. Check the **Output** window for any errors

### Step 4: Run the Application

1. Press `F5` to run with debugging
2. Or go to `bin\Debug\net8.0-windows\` and run `KtoolsGui.exe`

### Step 5: Publish for Distribution (Optional)

To create a standalone version:

```bash
dotnet publish -c Release -o ./publish
```

The published files will be in the `./publish` folder.

---

## 📄 License

This project is licensed under the **MIT License**.

**What does this mean?**

- ✅ You can use this tool for free
- ✅ You can modify the source code
- ✅ You can distribute your own versions
- ❌ You cannot hold the author liable
- ❌ No warranty is provided

See the [LICENSE](LICENSE) file for the full text.

---

## 📬 Contacts

### Need Help?

- **Found a bug?** → Create an [Issue](https://github.com/sergeikirillov511-KING/KtoolsGui/issues)
- **Have a feature request?** → Create an [Issue](https://github.com/sergeikirillov511-KING/KtoolsGui/issues)
- **Want to contribute?** → Submit a [Pull Request](https://github.com/sergeikirillov511-KING/KtoolsGui/pulls)

### Before Creating an Issue

1. Check existing issues to avoid duplicates
2. Include your OS version and .NET version
3. Describe the problem clearly with steps to reproduce
4. Attach screenshots if applicable

---

## 🙏 Acknowledgments

- **Ktools** — Original command-line utilities by Klei Entertainment
- **Don't Starve Together** — Game by Klei Entertainment
- **DST Modding Community** — For inspiration and feedback

---

Made with ❤️ for the DST modding community

