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
                await Task.Delay(1500); // app indulás után
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    // Briefly switch to Settings tab and back
                    var mainTab = CurrentItem;
                    CurrentItem = Items[0].Items[1]; // Settings tab
                    CurrentItem = mainTab; // vissza
                });
            });
        }
    }
}
