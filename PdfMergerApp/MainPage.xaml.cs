using Android.OS;
using CommunityToolkit.Maui.Storage;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PdfMergerApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly ILogger<MainPage> _logger;

        public MainPage(ILogger<MainPage> logger)
        {
            _logger = logger;
            InitializeComponent();

            MyCollectionView.ItemsSource = GlobalVariables.inputPdf;
        }


        private async void clearSelection(object sender, EventArgs e)
        {
            await DeleteTempFiles();
            await DisplayAlert("OK", "Kiválasztás törölve", "OK");
            await ClearGlobalVariables();

        }

        private async void selectPdf(object sender, EventArgs e)
        {
            try
            {
                await CopyFileFromDownloadToAppDirectory();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error copying PDF to app directory");
                await DisplayAlert("Hiba", ex.Message, "OK");
                return;
            }
        }



        private async void createPdf(object sender, EventArgs e)
        {
            //await ClearGlobalVariables();

            string outputPdfPath = "";
            outputPdfPath = Path.Combine(FileSystem.AppDataDirectory, "3.pdf");

            string inputPdfPath1 = "";
            string inputPdfPath2 = "";


            try
            {
                await DisplayAlert("", "Pdf Összefüzése...", "OK");
                using (PdfWriter writer = new PdfWriter(outputPdfPath))
                using (PdfDocument destPdf = new PdfDocument(writer))
                {
                    PdfMerger merger = new PdfMerger(destPdf);

                    for (int i = 0; i < GlobalVariables.inputPdf.Count; i++)
                    {
                        string inputPdfPath = Path.Combine(FileSystem.AppDataDirectory, GlobalVariables.inputPdf[i]);
                        using (PdfReader reader = new PdfReader(inputPdfPath))
                        using (PdfDocument pdf = new PdfDocument(reader))
                        {
                            reader.SetUnethicalReading(true);
                            merger.Merge(pdf, 1, pdf.GetNumberOfPages());
                            GlobalVariables.sumOfPages++;
                        }
                    }
                }

                await DisplayAlert("", $"PDF létrejött. Összesen {GlobalVariables.sumOfPages} oldal.", "OK");
                await DisplayAlert("", "Másolás kezdödik....", "OK");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error merging PDFs");
                await DisplayAlert("Hiba", ex.Message, "OK");
            }

            //copy the file to downloads
            try
            {
                //outputPdfPath = 
                await MoveFileFromAppDirectoryToDownloadAsync(FileSystem.AppDataDirectory, "3.pdf");
                await DisplayAlert("", "Másolás kész", "OK");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error moving PDF to downloads");
                await DisplayAlert("Hiba", ex.Message, "OK");
            }

            try
            {
                await DeleteTempFiles();
                await DisplayAlert("", "Temp fájlok törölve", "OK");
                await OpenCreatedPdf();
                await ClearGlobalVariables();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting temp files");
                await DisplayAlert("Hiba", ex.Message, "OK");
            }

        }
        static int get_pageCcount(string file)
        {
            using (StreamReader sr = new StreamReader(File.OpenRead(file)))
            {
                return new Regex(@"/Type\s*/Page[^s]").Matches(sr.ReadToEnd()).Count;
            }
        }
        private async void Vibrate_Clicked(object sender, EventArgs e)
        {
            var platform = DeviceInfo.Platform;

            if (platform.ToString() == "Android")
            {
                int secondsToVibrate = Random.Shared.Next(1, 7);
                TimeSpan vibrationLength = TimeSpan.FromSeconds(secondsToVibrate);

                Vibration.Default.Vibrate(vibrationLength);

            }

        }
        private void VibrateStopButton_Clicked(object sender, EventArgs e) =>
                    Vibration.Default.Cancel();


        private async void SaveToTxt_Clicked(object sender, EventArgs e)
        {
            var platform = DeviceInfo.Platform;

            if (platform.ToString() == "Android")
            {
                string text = "test message to file";
                string outputPdfPath = Path.Combine(FileSystem.AppDataDirectory, "test.txt");

                using (StreamWriter sw = new StreamWriter(outputPdfPath, true))
                {
                    sw.WriteLine(text);
                }

                await MoveFileFromAppDirectoryToDownloadAsync(outputPdfPath, "test.txt");

            }

        }

        public static async Task MoveFileFromAppDirectoryToDownloadAsync(string sourcePath, string fileName)
        {

# if ANDROID
            var destinationPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
            //Source Path
            //string sourceFilePath = Path.Combine(FileSystem.AppDataDirectory, "test.txt");
            string sourceFilePath = Path.Combine(sourcePath, fileName);
            string timeStamp = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            GlobalVariables.outputPdf = Path.Combine(destinationPath, "output" + timeStamp + ".pdf");
            using (FileStream sourceStream = new FileStream(sourceFilePath, FileMode.OpenOrCreate))
            //using (FileStream destinationStream = new FileStream(Path.Combine(destinationPath, "test_copied.txt"), FileMode.Create))
            //using (FileStream destinationStream = new FileStream(Path.Combine(destinationPath, fileName), FileMode.Create)
            using (FileStream destinationStream = new FileStream(Path.Combine(destinationPath, "output" + timeStamp + ".pdf"), FileMode.Create))
            {
                sourceStream.CopyTo(destinationStream);
            }
# endif
        }

        public static async Task DeleteTempFiles()
        {

# if ANDROID
            string sourceFilePath1 = Path.Combine(FileSystem.AppDataDirectory, "1.pdf");
            string sourceFilePath2 = Path.Combine(FileSystem.AppDataDirectory, "2.pdf");
            string sourceFilePath3 = Path.Combine(FileSystem.AppDataDirectory, "3.pdf");
            //string sourceFilePath = Path.Combine(sourcePath, fileName);

            if (File.Exists(sourceFilePath1))
            {
                File.Delete(sourceFilePath1);
            }

            if (File.Exists(sourceFilePath2))
            {
                File.Delete(sourceFilePath2);
            }

            if (File.Exists(sourceFilePath3))
            {
                File.Delete(sourceFilePath3);
            }

# endif
        }

        public async Task CopyFileFromDownloadToAppDirectory()
        {
            var result = await FilePicker.Default.PickAsync();
            if (result == null) return;
            var destPath = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
            GlobalVariables.inputPdf.Add(result.FileName.ToString());
            using var sourceStream = await result.OpenReadAsync();
            using var destStream = File.Create(destPath);
            await sourceStream.CopyToAsync(destStream);
        }

        public async Task OpenCreatedPdf()
        {

            //var filePath = Path.Combine(FileSystem.AppDataDirectory, "teszt.pdf");
            var filePath = GlobalVariables.outputPdf;

            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });

        }

        public async Task ClearGlobalVariables()
        {
            await Task.Run(() =>
            {

                GlobalVariables.inputPdf.Clear();
                GlobalVariables.outputPdf = "";

            });
        }
    }

    public static class GlobalVariables
    {
        public static ObservableCollection<string> inputPdf = new ObservableCollection<string>();
        public static String outputPdf = "";
        public static int sumOfPages = 0;
    }
}
