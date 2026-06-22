using Microsoft.Extensions.Logging;
using proyecto3.Services;
using proyecto3.ViewModels;
using proyecto3.Views;

namespace proyecto3;

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
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Registrar Servicios
		builder.Services.AddSingleton<DatabaseService>();

		// Registrar ViewModels
		builder.Services.AddTransient<MainViewModel>();
		builder.Services.AddTransient<NuevaTransaccionViewModel>();

		// Registrar Vistas
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<NuevaTransaccionPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
