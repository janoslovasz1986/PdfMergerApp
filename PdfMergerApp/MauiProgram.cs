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


            var platform = DeviceInfo.Platform;
#if DEBUG
            //builder.Logging.AddDebug();
#endif
            IServiceCollection services = builder.Services;


            String path = @"C:\Logs";

            if (platform.ToString() == "WinUI")

            {
                path = "yes";

            }


            string logDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "logs");
            string logDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            Log.Logger = new LoggerConfiguration()
               .WriteTo.Debug()
               .WriteTo.File(logDir, rollingInterval: RollingInterval.Day)
               .CreateLogger();


            // Register Serilog with the MAUI logging pipeline
            builder.Logging.AddSerilog(Log.Logger, dispose: true);



            return builder.Build();
        }
    }
}
