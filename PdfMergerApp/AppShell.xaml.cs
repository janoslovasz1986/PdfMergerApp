namespace PdfMergerApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
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
    }
}
