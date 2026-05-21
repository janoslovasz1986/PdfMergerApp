using Microsoft.Extensions.DependencyInjection;

namespace PdfMergerApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            bool darkMode = Preferences.Get("darkMode", false);
            UserAppTheme = darkMode ? AppTheme.Dark : AppTheme.Light;

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new SplashPage());
        }
    }
}