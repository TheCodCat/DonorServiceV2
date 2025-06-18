using DonorApplication.Singlton;
using DonorApplication.ViewModel;
using Microsoft.Extensions.Logging;
using UraniumUI;

namespace DonorApplication
{
	public static class MauiProgram
	{
		public static MauiApp CreateMauiApp()
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.UseUraniumUI()
				.UseUraniumUIMaterial()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
					fonts.AddFontAwesomeIconFonts();
				});
			Eliseev.MauiXamlBase64ImageToolkit.Controls.Init();
			builder.Services.AddSingleton<AuthorizationViewModel>();
			builder.Services.AddSingleton<EditProfileViewModel>();
			builder.Services.AddSingleton<UserData>();
#if DEBUG
			builder.Logging.AddDebug();
#endif

			return builder.Build();
		}
	}
}
