# Blazor Hybrid MAUI Application Architecture

## Table of Contents
1. [Application Boot-Up Flow](#application-boot-up-flow)
2. [Hybrid Architecture](#hybrid-architecture)
3. [Dependency Injection](#dependency-injection)
4. [Component Lifecycle](#component-lifecycle)
5. [JavaScript Interop](#javascript-interop)
6. [Navigation Flow](#navigation-flow)
7. [State Management](#state-management)

---

## Application Boot-Up Flow

### Complete Startup Sequence

```
┌─────────────────────────────────────────────────────────────────┐
│                    iOS OPERATING SYSTEM                         │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       │ User taps app icon
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│  STEP 1: iOS Native Entry Point                                │
│  File: Platforms/iOS/Program.cs:13                              │
│                                                                 │
│  static void Main(string[] args)                                │
│  {                                                              │
│      UIApplication.Main(args, null, typeof(AppDelegate));      │
│  }                                                              │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       │ iOS runtime creates UIApplication
                       │ and instantiates AppDelegate
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│  STEP 2: AppDelegate Initialization                             │
│  File: Platforms/iOS/AppDelegate.cs:8                           │
│                                                                 │
│  protected override MauiApp CreateMauiApp()                     │
│  {                                                              │
│      return MauiProgram.CreateMauiApp();                        │
│  }                                                              │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│  STEP 3: MAUI App Configuration                                 │
│  File: MauiProgram.cs:8-29                                      │
│                                                                 │
│  public static MauiApp CreateMauiApp()                          │
│  {                                                              │
│      var builder = MauiApp.CreateBuilder();                     │
│      builder.UseMauiApp<App>()                                  │
│      builder.Services.AddMauiBlazorWebView();  ← CRITICAL       │
│                                                                 │
│      // Register services (DI Container setup)                  │
│      builder.Services.AddSingleton<AppStateService>();          │
│      builder.Services.AddSingleton<IMediaPicker>(...);          │
│      builder.Services.AddSingleton<IGeolocation>(...);          │
│                                                                 │
│      return builder.Build();  ← Creates MauiApp instance        │
│  }                                                              │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│  STEP 4: Application Class Instantiation                       │
│  File: App.xaml.cs:5-8                                          │
│                                                                 │
│  public App()                                                   │
│  {                                                              │
│      InitializeComponent();  ← Loads App.xaml                   │
│  }                                                              │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       │ MAUI framework calls CreateWindow
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│  STEP 5: Main Window Creation                                  │
│  File: App.xaml.cs:10-13                                        │
│                                                                 │
│  protected override Window CreateWindow(...)                    │
│  {                                                              │
│      return new Window(new MainPage())                          │
│      {                                                          │
│          Title = "BlazorHybridDemo"                             │
│      };                                                         │
│  }                                                              │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│  STEP 6: MainPage Construction                                 │
│  File: MainPage.xaml:8-12                                       │
│                                                                 │
│  <BlazorWebView x:Name="blazorWebView"                          │
│                 HostPage="wwwroot/index.html">                  │
│      <BlazorWebView.RootComponents>                             │
│          <RootComponent Selector="#app"                         │
│                         ComponentType="Routes" />               │
│      </BlazorWebView.RootComponents>                            │
│  </BlazorWebView>                                               │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│  STEP 7: BlazorWebView Initialization                          │
│  (Internal MAUI Component)                                      │
│                                                                 │
│  • Creates native WebView (WKWebView on iOS)                    │
│  • Sets up JS ↔ C# bridge                                      │
│  • Configures message handlers                                  │
│  • Loads wwwroot/index.html                                     │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│  STEP 8: HTML Document Loading                                 │
│  File: wwwroot/index.html:1-30                                  │
│                                                                 │
│  <!DOCTYPE html>                                                │
│  <html>                                                         │
│  <head>                                                         │
│      <link rel="stylesheet" href="css/bootstrap/...css" />     │
│      <link rel="stylesheet" href="css/app.css" />              │
│  </head>                                                        │
│  <body>                                                         │
│      <div id="app">Loading...</div>  ← Placeholder             │
│                                                                 │
│      <script src="_framework/blazor.webview.js"></script>      │
│      <script src="js/jsInteropDemo.js"></script>               │
│  </body>                                                        │
│  </html>                                                        │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│  STEP 9: JavaScript Runtime Loading                            │
│                                                                 │
│  1. blazor.webview.js loads and executes                       │
│     • Creates global DotNet object                             │
│     • Sets up JS → C# communication bridge                     │
│     • Initializes Blazor runtime                               │
│                                                                 │
│  2. jsInteropDemo.js loads and executes                        │
│     • Defines window.blazorDemo functions                      │
│     • Defines window.registerCSharpCallbacks                   │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│  STEP 10: Blazor Component Initialization                      │
│                                                                 │
│  Blazor runtime starts and:                                    │
│  1. Looks for <div id="app"> in HTML (Selector="#app")         │
│  2. Instantiates Routes component (ComponentType)              │
│  3. Replaces "Loading..." with Routes component                │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│  STEP 11: Router Component Initialization                      │
│  File: Components/Routes.razor:1-6                              │
│                                                                 │
│  <Router AppAssembly="@typeof(MauiProgram).Assembly">          │
│      • Scans assembly for @page directives                     │
│      • Builds route table:                                     │
│        "/" → Home.razor                                         │
│        "/counter" → Counter.razor                               │
│        "/weather" → Weather.razor                               │
│        "/camera" → CameraDemo.razor                             │
│        "/location" → LocationDemo.razor                         │
│        "/jsinterop" → JSInteropDemo.razor                       │
│        "/platforminfo" → PlatformInfo.razor                     │
│        "/state" → StateDemo.razor                               │
│      • Matches current URL ("/")                                │
│      • Wraps in DefaultLayout (MainLayout)                     │
│  </Router>                                                      │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│  STEP 12: Layout Rendering                                     │
│  File: Components/Layout/MainLayout.razor                       │
│                                                                 │
│  Renders:                                                       │
│  • Navigation menu (NavMenu component)                         │
│  • Main content area                                            │
│  • @Body (where route component renders)                       │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│  STEP 13: Home Page Rendering                                  │
│  File: Components/Pages/Home.razor                              │
│                                                                 │
│  • OnInitialized() lifecycle method runs                       │
│  • Component state is initialized                               │
│  • Renders to DOM inside <div id="app">                        │
│  • CSS from scoped styles applied                               │
└──────────────────────┬──────────────────────────────────────────┘
                       │
                       ↓
┌─────────────────────────────────────────────────────────────────┐
│  🎉 APPLICATION FULLY LOADED AND INTERACTIVE                    │
│                                                                 │
│  User sees:                                                     │
│  • Navigation menu on the left                                  │
│  • Home page content on the right                               │
│  • Can click links to navigate                                  │
│  • All services registered in DI are ready                      │
│  • JS ↔ C# interop is active                                   │
└─────────────────────────────────────────────────────────────────┘
```

### Timeline Summary

| Time | Event |
|------|-------|
| T+0ms | iOS: App icon tapped |
| T+50ms | Program.Main() → UIApplication starts |
| T+100ms | AppDelegate creates MauiApp |
| T+150ms | DI container configured |
| T+200ms | App window created |
| T+250ms | BlazorWebView initialized |
| T+300ms | index.html loaded |
| T+400ms | JavaScript files loaded |
| T+500ms | Blazor runtime started |
| T+600ms | Routes component initialized |
| T+700ms | Home page rendered |
| **T+800ms** | **✅ App ready for user interaction** |

---

## Hybrid Architecture

```
┌──────────────────────────────────────────┐
│         Native Container (MAUI)          │
│  ┌────────────────────────────────────┐  │
│  │     Native WebView (WKWebView)     │  │
│  │  ┌──────────────────────────────┐  │  │
│  │  │   HTML/CSS/JavaScript        │  │  │
│  │  │   (Blazor Components)        │  │  │
│  │  └──────────────────────────────┘  │  │
│  │            ↕ Bridge ↕               │  │
│  │  ┌──────────────────────────────┐  │  │
│  │  │   C# Code & .NET Runtime     │  │  │
│  │  └──────────────────────────────┘  │  │
│  └────────────────────────────────────┘  │
│            ↕ Native APIs ↕                │
│  ┌────────────────────────────────────┐  │
│  │  iOS/Android Platform Services    │  │
│  │  (Camera, GPS, Sensors, etc.)     │  │
│  └────────────────────────────────────┘  │
└──────────────────────────────────────────┘
```

### Key Components

1. **Native MAUI Container**: The native app shell
2. **WebView**: Platform-specific web browser (WKWebView on iOS, WebView on Android)
3. **Blazor Components**: Your UI written in Razor/C#
4. **JS ↔ C# Bridge**: Communication layer between JavaScript and .NET
5. **Platform Services**: Access to native device features

---

## Dependency Injection

### Service Registration

Services are registered in `MauiProgram.cs`:

```csharp
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();

    // Register application services
    builder.Services.AddSingleton<AppStateService>();

    // Register MAUI Essentials services
    builder.Services.AddSingleton<IMediaPicker>(MediaPicker.Default);
    builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);

    return builder.Build();
}
```

### Service Injection in Components

Use `@inject` directive in Razor components:

```razor
@page "/camera"
@inject IMediaPicker MediaPicker

<button @onclick="TakePhoto">Take Photo</button>

@code {
    private async Task TakePhoto()
    {
        var photo = await MediaPicker.CapturePhotoAsync();
        // Process photo...
    }
}
```

### Service Lifetimes

- **Singleton**: Single instance throughout app lifetime
  - Best for: State management, platform services
  - Example: `AppStateService`, `IMediaPicker`, `IGeolocation`

- **Scoped**: New instance per component tree (not commonly used in MAUI)

- **Transient**: New instance every time it's requested

---

## Component Lifecycle

### Lifecycle Methods (in order)

```csharp
public class MyComponent : ComponentBase
{
    // 1. Constructor
    public MyComponent() { }

    // 2. Parameters are set
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);
    }

    // 3. Component initialization (runs once)
    protected override void OnInitialized()
    {
        // Synchronous initialization
    }

    protected override async Task OnInitializedAsync()
    {
        // Async initialization (e.g., API calls)
    }

    // 4. After parameters are set
    protected override void OnParametersSet()
    {
        // Runs every time parameters change
    }

    protected override async Task OnParametersSetAsync()
    {
        // Async version
    }

    // 5. After component renders
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            // Runs only on first render
            // Good for JS interop setup
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Setup JS interop
            await JSRuntime.InvokeVoidAsync("initializeComponent");
        }
    }

    // 6. Component disposal
    public void Dispose()
    {
        // Clean up resources
        dotNetReference?.Dispose();
    }
}
```

### When to Use Each Method

| Method | Use Case |
|--------|----------|
| `OnInitialized` | Set initial state, non-async operations |
| `OnInitializedAsync` | Load data from APIs, async initialization |
| `OnParametersSet` | React to parameter changes |
| `OnAfterRender(firstRender)` | JS interop setup (on first render only) |
| `Dispose` | Clean up resources, dispose references |

---

## JavaScript Interop

### Architecture

```
JavaScript Side              Blazor Runtime              C# Side
─────────────────           ────────────────           ──────────

DotNet.invokeMethodAsync ──► Serializes params ──►    Finds method
     ↓                       JSON format              with [JSInvokable]
     ↓                            ↓                          ↓
Waits for Promise           Invokes C# method           Executes
     ↓                            ↓                          ↓
Returns result     ◄────── Serializes result  ◄────── Returns value
```

### The `DotNet` Object

**Source**: Provided by `_framework/blazor.webview.js`

**Location**: NuGet package `Microsoft.AspNetCore.Components.WebView.Maui`

**Purpose**: Global JavaScript object for calling C# from JavaScript

### Static Method Calls (C# to JS)

**C# Code:**
```csharp
[JSInvokable]
public static Task<string> GetCurrentTime()
{
    return Task.FromResult(DateTime.Now.ToString("HH:mm:ss"));
}
```

**JavaScript Code:**
```javascript
const result = await DotNet.invokeMethodAsync(
    'BlazorHybridDemo',    // Assembly name
    'GetCurrentTime'        // Method name
);
```

### Instance Method Calls (C# to JS)

**C# Code:**
```csharp
private DotNetObjectReference<JSInteropDemo>? dotNetReference;

protected override void OnAfterRender(bool firstRender)
{
    if (firstRender)
    {
        dotNetReference = DotNetObjectReference.Create(this);
    }
}

[JSInvokable]
public Task<string> ProcessData(string input)
{
    // Can access component state
    StateHasChanged();
    return Task.FromResult($"Processed: {input.ToUpper()}");
}

public void Dispose()
{
    dotNetReference?.Dispose(); // Important!
}
```

**JavaScript Code:**
```javascript
// dotNetRef passed from C#
const result = await dotNetRef.invokeMethodAsync(
    'ProcessData',
    'Hello from JavaScript'
);
```

### Comparison Table

| Feature | Static Method | Instance Method |
|---------|--------------|-----------------|
| **Declaration** | `public static` | `public` (non-static) |
| **JavaScript Call** | `DotNet.invokeMethodAsync('Assembly', 'Method')` | `dotNetRef.invokeMethodAsync('Method')` |
| **First Parameter** | Assembly name | Method name |
| **Reference Needed** | ❌ No | ✅ Yes (`DotNetObjectReference`) |
| **Access Component State** | ❌ No | ✅ Yes |
| **Can Update UI** | ❌ No | ✅ Yes (via `StateHasChanged()`) |
| **Memory** | No extra allocation | Must dispose reference |
| **Use Case** | Utilities, global functions | Component interactions, state updates |

### JavaScript to C# (Calling JS from C#)

**JavaScript Code (wwwroot/js/jsInteropDemo.js):**
```javascript
window.blazorDemo = {
    showMessage: function(message) {
        alert('JavaScript says: ' + message);
        return 'Message shown: ' + message;
    }
};
```

**C# Code:**
```csharp
@inject IJSRuntime JSRuntime

private async Task CallJavaScript()
{
    // Call JS function and get result
    var result = await JSRuntime.InvokeAsync<string>(
        "blazorDemo.showMessage",
        "Hello from C#"
    );

    // For void functions
    await JSRuntime.InvokeVoidAsync(
        "console.log",
        "Logging from C#"
    );
}
```

---

## Navigation Flow

### Client-Side Navigation (No Page Reload)

```
User clicks link in NavMenu
        ↓
Router intercepts navigation
        ↓
Matches URL to route
        ↓
Instantiates target component
        ↓
Component lifecycle runs
        ↓
Component renders in @Body
        ↓
UI updates (no page reload!)
```

### Route Table

Routes are defined using `@page` directive:

```razor
@page "/camera"
@page "/camera/{id:int}"  <!-- With parameter -->
```

The Router scans the assembly and builds this table:

| Route | Component |
|-------|-----------|
| `/` | Home.razor |
| `/counter` | Counter.razor |
| `/weather` | Weather.razor |
| `/camera` | CameraDemo.razor |
| `/location` | LocationDemo.razor |
| `/jsinterop` | JSInteropDemo.razor |
| `/platforminfo` | PlatformInfo.razor |
| `/state` | StateDemo.razor |

### Programmatic Navigation

```csharp
@inject NavigationManager Navigation

private void NavigateToCamera()
{
    Navigation.NavigateTo("/camera");
}

private void NavigateWithParameter()
{
    Navigation.NavigateTo($"/camera/{photoId}");
}
```

---

## State Management

### Singleton Service Pattern

**Define State Service:**
```csharp
public class AppStateService
{
    private string? _userName;

    public string? UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
}
```

**Register in MauiProgram.cs:**
```csharp
builder.Services.AddSingleton<AppStateService>();
```

**Use in Components:**
```razor
@inject AppStateService AppState
@implements IDisposable

<p>Current User: @AppState.UserName</p>

@code {
    protected override void OnInitialized()
    {
        AppState.OnChange += StateHasChanged;
    }

    public void Dispose()
    {
        AppState.OnChange -= StateHasChanged;
    }
}
```

### State Persistence

State in singleton services persists:
- ✅ Across navigation
- ✅ Across component lifecycle
- ❌ NOT across app restarts (unless saved to SecureStorage/Preferences)

**Save to Device:**
```csharp
// Save
await SecureStorage.SetAsync("username", userName);

// Load
var userName = await SecureStorage.GetAsync("username");

// Simple preferences
Preferences.Set("theme", "dark");
var theme = Preferences.Get("theme", "light");
```

---

## Best Practices

### ✅ Do's

1. **Use Dependency Injection** for services
2. **Dispose** `DotNetObjectReference` to prevent memory leaks
3. **Use `OnAfterRender(firstRender)`** for JS interop setup
4. **Separate JS** into external files (not inline)
5. **Request permissions** before using device features
6. **Use singleton services** for shared state
7. **Clean up event handlers** in `Dispose()`

### ❌ Don'ts

1. **Don't use `eval()`** for JS interop (security risk)
2. **Don't forget** to add `[JSInvokable]` attribute
3. **Don't call `StateHasChanged()`** from static methods
4. **Don't forget** platform-specific permissions in Info.plist/AndroidManifest.xml
5. **Don't use async void** (use `async Task` instead)
6. **Don't store sensitive data** in plain Preferences (use SecureStorage)

---

## File Structure

```
BlazorHybridDemo/
├── Platforms/
│   ├── iOS/
│   │   ├── Program.cs              # Entry point
│   │   ├── AppDelegate.cs          # iOS app delegate
│   │   └── Info.plist              # iOS permissions & config
│   └── Android/
│       └── AndroidManifest.xml     # Android permissions
├── Components/
│   ├── Layout/
│   │   ├── MainLayout.razor        # App layout
│   │   └── NavMenu.razor           # Navigation
│   └── Pages/
│       ├── Home.razor              # Home page
│       ├── CameraDemo.razor        # Camera functionality
│       ├── LocationDemo.razor      # GPS functionality
│       ├── JSInteropDemo.razor     # JS interop examples
│       ├── PlatformInfo.razor      # Device info
│       └── StateDemo.razor         # State management
├── Services/
│   └── AppStateService.cs          # Shared state
├── wwwroot/
│   ├── index.html                  # HTML host page
│   ├── css/
│   │   └── app.css                 # Global styles
│   └── js/
│       └── jsInteropDemo.js        # JavaScript functions
├── App.xaml.cs                     # Application class
├── MainPage.xaml                   # BlazorWebView host
└── MauiProgram.cs                  # DI & configuration
```

---

## Additional Resources

- [Official MAUI Blazor Documentation](https://learn.microsoft.com/en-us/aspnet/core/blazor/hybrid/)
- [Blazor Component Lifecycle](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/lifecycle)
- [JavaScript Interop](https://learn.microsoft.com/en-us/aspnet/core/blazor/javascript-interoperability/)
- [MAUI Essentials](https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/)

---

**Document Version**: 1.0
**Last Updated**: March 11, 2026
**Author**: Generated for BlazorHybridDemo Project