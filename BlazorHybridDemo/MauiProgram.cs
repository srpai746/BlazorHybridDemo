using Microsoft.Extensions.Logging;
using BlazorHybridDemo.Services;

namespace BlazorHybridDemo;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

		// Register application services
		builder.Services.AddSingleton<AppStateService>();

		// Register MAUI Essentials services
		builder.Services.AddSingleton<IMediaPicker>(MediaPicker.Default);
		builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
