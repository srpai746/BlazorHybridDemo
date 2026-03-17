# Blazor Hybrid Demo

A comprehensive .NET MAUI Blazor Hybrid application demonstrating various platform features and capabilities across Android, iOS, Windows, and macOS.

## Features

- **Counter Demo**: Interactive counter with component state management
- **Camera Demo**: Access device camera functionality
- **Location Demo**: Get device GPS location
- **JS Interop Demo**: JavaScript interoperability examples
- **Platform Info**: Display device and platform information
- **State Management**: Application state management demonstration
- **Weather Page**: Sample data display with minimal layout and back navigation
- **Multiple Layouts**: MainLayout (with navigation) and MinimalLayout (clean UI)

## System Requirements

### For Windows Development Machine
- **Operating System**: Windows 10 version 1809 or higher, or Windows 11
- **.NET SDK**: .NET 9.0 or later
- **Visual Studio 2022** (version 17.8 or later) or **Visual Studio 2026**
- **Required Workloads to Install**:
  - ✅ `.NET Multi-platform App UI development` (for Android, iOS, macOS, Windows development)
  - ✅ `Mobile development with .NET` (includes Android SDK)
- **For iOS Development from Windows**: Requires a paired Mac with Xcode installed

### For macOS Development Machine
- **Operating System**: macOS 12 (Monterey) or later
- **.NET SDK**: .NET 9.0 or later
- **Xcode**: 14.0 or later (required for iOS and macOS development)
- **IDE Options**:
  - **JetBrains Rider** (Recommended)
  - **Visual Studio Code** with C# Dev Kit extension
- **Required Workloads to Install**:
  - ✅ `.NET MAUI` workload (for Android, iOS, macOS development)

## Installation

### Windows Setup

1. **Install Visual Studio 2022 or 2026**
   - Download from [Visual Studio](https://visualstudio.microsoft.com/)
   - During installation, select these workloads:
     - ✅ `.NET Multi-platform App UI development`
     - ✅ `Mobile development with .NET`

   **What you can develop on Windows:**
   - ✅ Android apps (with emulator or physical device)
   - ✅ Windows apps (native)
   - ✅ iOS apps (requires Mac agent for building and testing)
   - ✅ macOS apps (requires Mac agent for building and testing)

2. **Verify .NET MAUI Workload**
   ```bash
   dotnet workload list
   ```
   If MAUI is not installed:
   ```bash
   dotnet workload install maui
   ```

3. **Clone the Repository**
   ```bash
   git clone git@github.com:srpai746/BlazorHybridDemo.git
   cd BlazorHybridSession
   ```

4. **Restore Dependencies**
   ```bash
   dotnet restore BlazorHybridDemo.slnx
   ```

5. **Configure Mac Agent (Optional - for iOS/macOS development)**
   - Install Xcode on your Mac
   - In Visual Studio, go to `Tools > iOS > Pair to Mac`
   - Follow the pairing wizard to connect to your Mac

### macOS Setup

1. **Install Xcode**
   - Install from Mac App Store
   - Open Xcode and accept the license agreement
   - Install Command Line Tools:
   ```bash
   xcode-select --install
   ```

2. **Install .NET 9.0 SDK**
   ```bash
   # Using Homebrew (recommended):
   brew install --cask dotnet-sdk

   # Or download from: https://dotnet.microsoft.com/download/dotnet/9.0
   ```

3. **Install .NET MAUI Workload**
   ```bash
   dotnet workload install maui
   ```

4. **Install IDE**
   - **Option A: JetBrains Rider** (Recommended for full-featured IDE)
     - Download from [JetBrains Rider](https://www.jetbrains.com/rider/)

   - **Option B: Visual Studio Code**
     - Download from [Visual Studio Code](https://code.visualstudio.com/)
     - Install the `C# Dev Kit` extension

   **What you can develop on macOS:**
   - ✅ Android apps (with emulator or physical device)
   - ✅ iOS apps (with simulator or physical device)
   - ✅ macOS apps (native via Mac Catalyst)
   - ❌ Windows apps (not supported on macOS)

5. **Clone the Repository**
   ```bash
   git clone git@github.com:srpai746/BlazorHybridDemo.git
   cd BlazorHybridSession
   ```

6. **Restore Dependencies**
   ```bash
   dotnet restore BlazorHybridDemo.slnx
   ```

## How to Run

### On Windows Development Machine

#### Using Visual Studio 2022/2026

1. Open `BlazorHybridDemo.slnx` in Visual Studio
2. Select your target platform from the debug toolbar dropdown:
   - **Windows Machine** - Run as native Windows app
   - **Android Emulator** - Run on Android emulator (or connected device)
   - **iOS Simulator** - Run on iOS simulator (requires paired Mac)
3. Press `F5` or click "Start Debugging"

#### Using Command Line

**Windows App:**
```bash
dotnet build BlazorHybridDemo/BlazorHybridDemo.csproj -f net9.0-windows10.0.19041.0
dotnet run --project BlazorHybridDemo/BlazorHybridDemo.csproj -f net9.0-windows10.0.19041.0
```

**Android App:**
```bash
dotnet build BlazorHybridDemo/BlazorHybridDemo.csproj -f net9.0-android
dotnet run --project BlazorHybridDemo/BlazorHybridDemo.csproj -f net9.0-android
```

### On macOS Development Machine

#### Using JetBrains Rider

1. Open `BlazorHybridDemo.slnx` in Rider
2. Select your target platform from run configurations:
   - **Mac Catalyst** - Run as native macOS app
   - **iOS Simulator** - Run on iOS simulator
   - **Android Emulator** - Run on Android emulator
3. Click the "Run" button or press `⌃ + R`

#### Using Visual Studio Code

1. Open the `BlazorHybridSession` folder in VS Code
2. Install the C# Dev Kit extension (if not installed)
3. Press `F5` or use Command Palette (`⌘ + Shift + P`) > "Debug: Select and Start Debugging"
4. Choose your target platform

#### Using Command Line

**macOS App (Mac Catalyst):**
```bash
dotnet build BlazorHybridDemo/BlazorHybridDemo.csproj -f net9.0-maccatalyst
dotnet run --project BlazorHybridDemo/BlazorHybridDemo.csproj -f net9.0-maccatalyst
```

**iOS App:**
```bash
dotnet build BlazorHybridDemo/BlazorHybridDemo.csproj -f net9.0-ios
dotnet run --project BlazorHybridDemo/BlazorHybridDemo.csproj -f net9.0-ios
```

**Android App:**
```bash
dotnet build BlazorHybridDemo/BlazorHybridDemo.csproj -f net9.0-android
dotnet run --project BlazorHybridDemo/BlazorHybridDemo.csproj -f net9.0-android
```

## Running Tests

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Run specific test project
dotnet test BlazorHybridDemo.Tests/BlazorHybridDemo.Tests.csproj
```

## Project Structure

```
BlazorHybridSession/
├── BlazorHybridDemo/              # Main application project
│   ├── Components/
│   │   ├── Controls/              # Reusable UI controls
│   │   ├── Layout/                # Layout components
│   │   │   ├── MainLayout.razor   # Main layout with navigation
│   │   │   ├── MinimalLayout.razor # Minimal layout without nav
│   │   │   └── NavMenu.razor      # Navigation menu
│   │   └── Pages/                 # Application pages
│   │       ├── Counter.razor
│   │       ├── CameraDemo.razor
│   │       ├── LocationDemo.razor
│   │       ├── JSInteropDemo.razor
│   │       ├── PlatformInfo.razor
│   │       ├── StateDemo.razor
│   │       └── Weather.razor
│   ├── Platforms/                 # Platform-specific code
│   │   ├── Android/               # Android-specific implementations
│   │   ├── iOS/                   # iOS-specific implementations
│   │   ├── MacCatalyst/           # macOS-specific implementations
│   │   └── Windows/               # Windows-specific implementations
│   ├── Resources/                 # App resources (images, fonts, etc.)
│   ├── Services/                  # Application services
│   └── wwwroot/                   # Static web assets (CSS, JS)
├── BlazorHybridDemo.Tests/        # Unit tests using bUnit
└── README.md
```

## Platform Support Matrix

| Platform | Development OS | Minimum Version | Status |
|----------|---------------|-----------------|--------|
| Android | Windows / macOS | API 21 (Android 5.0) | ✅ Fully Supported |
| iOS | macOS (or Windows with Mac agent) | iOS 11.0 | ✅ Fully Supported |
| macOS | macOS | macOS 10.15 (Catalyst) | ✅ Fully Supported |
| Windows | Windows | Windows 10 (1809+) | ✅ Fully Supported |

## Troubleshooting

### Windows Issues

**Issue**: Android emulator not starting
- Verify Android SDK is installed (check Visual Studio Installer)
- Enable hardware acceleration (Intel HAXM or Windows Hypervisor Platform)
- Create a new Android Virtual Device (AVD) in Android Device Manager

**Issue**: Cannot connect to Mac for iOS development
- Ensure Mac and Windows are on same network
- Enable Remote Login on Mac (`System Preferences > Sharing > Remote Login`)
- Install Xcode on the Mac and accept license agreement
- Use `Tools > iOS > Pair to Mac` in Visual Studio

**Issue**: "The workload 'maui' is not installed"
```bash
dotnet workload install maui
```

### macOS Issues

**Issue**: "xcode-select: error: tool 'xcodebuild' requires Xcode"
```bash
sudo xcode-select --switch /Applications/Xcode.app/Contents/Developer
sudo xcodebuild -license accept
```

**Issue**: iOS simulator not launching
```bash
# Open simulator manually
open -a Simulator

# List available simulators
xcrun simctl list devices

# Reset simulator if needed
xcrun simctl erase all
```

**Issue**: Code signing / provisioning profile errors
- Open Xcode > Preferences > Accounts
- Sign in with your Apple ID
- Xcode will automatically create necessary development certificates
- For deployment, you'll need an Apple Developer account

**Issue**: Android emulator not working
- Install Android SDK via Android Studio or command line tools
- Set `ANDROID_HOME` environment variable:
```bash
export ANDROID_HOME=$HOME/Library/Android/sdk
export PATH=$PATH:$ANDROID_HOME/emulator:$ANDROID_HOME/tools:$ANDROID_HOME/platform-tools
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is a demo application for educational purposes.

## Additional Resources

- [.NET MAUI Documentation](https://learn.microsoft.com/dotnet/maui/)
- [Blazor Hybrid Documentation](https://learn.microsoft.com/aspnet/core/blazor/hybrid/)
- [bUnit Testing Documentation](https://bunit.dev/)
- [JetBrains Rider for .NET MAUI](https://www.jetbrains.com/help/rider/MAUI.html)
- [Visual Studio Mac Agent](https://learn.microsoft.com/xamarin/ios/get-started/installation/windows/connecting-to-mac/)