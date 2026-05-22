using Microsoft.Extensions.DependencyInjection;

namespace PdfMergerApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            string lang = Preferences.Get("language", "hu");
            LocalizationManager.SetLanguage(lang);
            GlobalVariables.currentLanguage = lang;

            bool darkMode = Preferences.Get("darkMode", false);
            UserAppTheme = darkMode ? AppTheme.Dark : AppTheme.Light;

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new SplashPage());
        }
    }
}