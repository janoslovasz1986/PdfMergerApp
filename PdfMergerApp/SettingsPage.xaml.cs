namespace PdfMergerApp
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            FileNameEntry.Text = GlobalVariables.outputFileName;
            VibrateSwitch.IsToggled = GlobalVariables.vibrateOnDone;
            AutoOpenSwitch.IsToggled = GlobalVariables.autoOpenPdf;
            LargePreviewSwitch.IsToggled = GlobalVariables.previewHeight == 300;
            DarkModeSwitch.IsToggled = Application.Current.UserAppTheme == AppTheme.Dark;
        }

        private void VibrateSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (!(GlobalVariables.vibrateOnDone))
                Vibration.Default.Vibrate(TimeSpan.FromSeconds(0.5));
            GlobalVariables.vibrateOnDone = e.Value;
         

        }

        private void LargePreviewSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            GlobalVariables.previewHeight = e.Value ? 400 : 160;
            GlobalVariables.previewSpan = e.Value ? 1 : 2;
        }

        private void AutoOpenSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            GlobalVariables.autoOpenPdf = e.Value;
        }

        private void SaveSettings_Clicked(object sender, EventArgs e)
        {
            string name = FileNameEntry.Text?.Trim();

            if (string.IsNullOrEmpty(name))
            {
                ConfirmLabel.TextColor = Colors.Red;
                ConfirmLabel.Text = "A fájlnév nem lehet üres!";
                return;
            }

            GlobalVariables.outputFileName = name;
            ConfirmLabel.TextColor = Colors.Green;
            ConfirmLabel.Text = $"Mentve: {name}.pdf";
        }

        private async void Vibrate_Clicked(object sender, EventArgs e)
        {
            var platform = DeviceInfo.Platform;

            if (platform.ToString() == "Android")
            {
                //int secondsToVibrate = Random.Shared.Next(1, 7);
                int secondsToVibrate = 1;
                TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);

                Vibration.Default.Vibrate(vibrationLength);

            }

        }

        private void DarkModeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            Application.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
        }

        private void AutoQuitSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            GlobalVariables.autoQuit = e.Value;
        }
    }
}