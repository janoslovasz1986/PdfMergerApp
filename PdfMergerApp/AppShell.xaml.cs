namespace PdfMergerApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            ApplyLocalization();
            PreloadSettingsPage();
        }

        private void PreloadSettingsPage()
        {
            Task.Run(async () =>
            {
                await Task.Delay(1500);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var tabBar = Items[0] as TabBar;
                    if (tabBar == null) return;

                    var mainTab = tabBar.Items[0];
                    var settingsTab = tabBar.Items[1];

                    CurrentItem = settingsTab;
                    CurrentItem = mainTab;
                });
            });
        }

        public void ApplyLocalization()
        {
            var tabBar = Items[0] as TabBar;
            if (tabBar == null) return;
            tabBar.Items[0].Title = LocalizationManager.Get("MainTab");
            tabBar.Items[1].Title = LocalizationManager.Get("SettingsTab");
        }
    }
}
