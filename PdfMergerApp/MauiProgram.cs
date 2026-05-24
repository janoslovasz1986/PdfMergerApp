using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Serilog;
using Serilog.Core;
using Microsoft.Maui.Devices;

namespace PdfMergerApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                 .ConfigureFonts(fonts =>
                 {
                     fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                     fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                 });
            //builder.Services.AddLocalization();

            IServiceCollection services = builder.Services;
            builder.Services.AddSingleton<SettingsPage>();

            return builder.Build();
        }
    }
}
