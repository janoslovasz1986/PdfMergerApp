namespace PdfMergerApp
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            // Betöltés
            FileNameEntry.Text = Preferences.Get("outputFileName", "output");
            VibrateSwitch.IsToggled = Preferences.Get("vibrateOnDone", false);
            AutoOpenSwitch.IsToggled = Preferences.Get("autoOpenPdf", false);
            LargePreviewSwitch.IsToggled = Preferences.Get("largePreview", true);
            DarkModeSwitch.IsToggled = Preferences.Get("darkMode", false);

            // Nyelv picker beállítása
            string lang = Preferences.Get("language", "hu");
            LanguagePicker.SelectedIndex = lang == "en" ? 1 : 0;

            // GlobalVariables szinkronizálása
            GlobalVariables.outputFileName = FileNameEntry.Text;
            GlobalVariables.vibrateOnDone = VibrateSwitch.IsToggled;
            GlobalVariables.autoOpenPdf = AutoOpenSwitch.IsToggled;
            GlobalVariables.previewHeight = LargePreviewSwitch.IsToggled ? 400 : 160;
            GlobalVariables.previewSpan = LargePreviewSwitch.IsToggled ? 1 : 2;
            Application.Current.UserAppTheme = DarkModeSwitch.IsToggled ? AppTheme.Dark : AppTheme.Light;

            FileNameEntry.Text = GlobalVariables.outputFileName;
            VibrateSwitch.IsToggled = GlobalVariables.vibrateOnDone;
            AutoOpenSwitch.IsToggled = GlobalVariables.autoOpenPdf;
            LargePreviewSwitch.IsToggled = GlobalVariables.previewHeight == 300;
            DarkModeSwitch.IsToggled = Application.Current.UserAppTheme == AppTheme.Dark;

            PasswordSwitch.IsToggled = Preferences.Get("passwordProtect", false);
            GlobalVariables.passwordProtect = PasswordSwitch.IsToggled;

            ApplyLocalization();
        }

        private void VibrateSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (!(GlobalVariables.vibrateOnDone))
                Vibration.Default.Vibrate(TimeSpan.FromSeconds(0.5));
            GlobalVariables.vibrateOnDone = e.Value;
            Preferences.Set("vibrateOnDone", e.Value);


        }

        private void LargePreviewSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            GlobalVariables.previewHeight = e.Value ? 400 : 160;
            GlobalVariables.previewSpan = e.Value ? 1 : 2;
            GlobalVariables.changePreview = e.Value;
            Preferences.Set("largePreview", e.Value);
        }

        private void AutoOpenSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            GlobalVariables.autoOpenPdf = e.Value;
            Preferences.Set("autoOpenPdf", e.Value);
        }

        private void SaveSettings_Clicked(object sender, EventArgs e)
        {
            string name = FileNameEntry.Text?.Trim();

            if (string.IsNullOrEmpty(name))
            {
                ConfirmLabel.TextColor = Colors.Red;
                ConfirmLabel.Text = LocalizationManager.Get("ErrorEmptyFileName");
                return;
            }

            GlobalVariables.outputFileName = name;
            Preferences.Set("outputFileName", name);
            ConfirmLabel.TextColor = Colors.Green;
            ConfirmLabel.Text = $"{LocalizationManager.Get("Save")}: {name}.pdf";
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
            Preferences.Set("darkMode", e.Value);
        }

        private void AutoQuitSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            GlobalVariables.autoQuit = e.Value;
            Preferences.Set("autoQuit", e.Value);
        }

        private void PasswordSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            GlobalVariables.passwordProtect = e.Value;
            Preferences.Set("passwordProtect", e.Value);
        }

        private void ApplyLocalization()
        {
            TitleLabel.Text = LocalizationManager.Get("Settings");
            OutputFileNameLabel.Text = LocalizationManager.Get("OutputFileName");
            SaveButton.Text = LocalizationManager.Get("Save");
            VibrationLabel.Text = LocalizationManager.Get("Vibration");
            VibrationDescLabel.Text = LocalizationManager.Get("VibrationDesc");
            AutoOpenLabel.Text = LocalizationManager.Get("AutoOpen");
            AutoOpenDescLabel.Text = LocalizationManager.Get("AutoOpenDesc");
            LargePreviewLabel.Text = LocalizationManager.Get("LargePreview");
            LargePreviewDescLabel.Text = LocalizationManager.Get("LargePreviewDesc");
            DarkModeLabel.Text = LocalizationManager.Get("DarkMode");
            DarkModeDescLabel.Text = LocalizationManager.Get("DarkModeDesc");
            AutoQuitLabel.Text = LocalizationManager.Get("AutoQuit");
            AutoQuitDescLabel.Text = LocalizationManager.Get("AutoQuitDesc");
            PasswordLabel.Text = LocalizationManager.Get("PasswordProtect");
            PasswordDescLabel.Text = LocalizationManager.Get("PasswordProtectDesc");
            LanguageLabel.Text = LocalizationManager.Get("Language");
            ConfirmLabel.Text = "";
        }

        private void LanguagePicker_Changed(object sender, EventArgs e)
        {
            string lang = LanguagePicker.SelectedIndex == 1 ? "en" : "hu";
            LocalizationManager.SetLanguage(lang);
            GlobalVariables.currentLanguage = lang;
            Preferences.Set("language", lang);
            ApplyLocalization();
        }
    }
}