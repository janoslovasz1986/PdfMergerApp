namespace PdfMergerApp
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            FileNameEntry.Text = GlobalVariables.outputFileName;
            VibrateSwitch.IsToggled = GlobalVariables.vibrateOnDone;
        }

        private void VibrateSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            GlobalVariables.vibrateOnDone = e.Value;
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
    }
}