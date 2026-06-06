using iText.Kernel.Pdf;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace PdfMergerApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            string lang = Preferences.Get("language", "hu");
            if (string.IsNullOrEmpty(lang))
            {
                // Első indulás - telefon nyelvét követjük
                string systemLang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                lang = systemLang switch
                {
                    "hu" => "hu",
                    "de" => "de",
                    "es" => "es",
                    _ => "en" // minden más -> angol
                };
            }

            GlobalVariables.currentLanguage = lang;
            Task.Run(async () => await LocalizationManager.LoadLanguageAsync(lang)).Wait();

            bool darkMode = Preferences.Get("darkMode", false);
            UserAppTheme = darkMode ? AppTheme.Dark : AppTheme.Light;

            // iText7 warmup háttérben
            Task.Run(() =>
            {
                try
                {
                    using var stream = new MemoryStream();
                    using var writer = new PdfWriter(stream);
                    using var pdf = new PdfDocument(writer);
                    pdf.AddNewPage();
                }
                catch { }
            });

        }
        

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new SplashPage());
        }
    }
}