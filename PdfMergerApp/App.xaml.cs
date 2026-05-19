using Microsoft.Extensions.DependencyInjection;

namespace PdfMergerApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new SplashPage();
            //Windows[0].Page = new SplashPage();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}