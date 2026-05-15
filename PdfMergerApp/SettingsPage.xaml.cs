namespace PdfMergerApp
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            FileNameEntry.Text = GlobalVariables.outputFileName;
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
    }
}