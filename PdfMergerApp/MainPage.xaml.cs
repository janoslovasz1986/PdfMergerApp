#if ANDROID
using Android.OS;
#endif
using CommunityToolkit.Maui.Storage;
using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.ApplicationModel;
using Org.BouncyCastle.Asn1.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PdfMergerApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        //bool IsBusy = false;
        private CancellationTokenSource _animCts;
        private readonly ILogger<MainPage> _logger;

        public MainPage(ILogger<MainPage> logger)
        {
            _logger = logger;
            InitializeComponent();
            //MyCollectionView.ItemsSource = GlobalVariables.inputPdf;
            PagesCollectionView.ItemsSource = GlobalVariables.pages;
        }


        public async void clearSelection(object sender, EventArgs e)
        {
            await DeleteTempFiles();
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
            if (GlobalVariables.inputPdf.Count == 0)
            {
                await DisplayAlert("Hiba", "Nincs PDF kiválasztva!", "OK");
                return;
            }

            BtnAddPdf.IsEnabled = false;
            BtnClear.IsEnabled = false;
            BtnCreatePdf.IsEnabled = false;
            StartLoadingAnimation();

            GlobalVariables.outputFileNameCreatedOnDeviceInnerStorage = "output_temp.pdf";
            string outputPdfPath = Path.Combine(FileSystem.AppDataDirectory,
                GlobalVariables.outputFileNameCreatedOnDeviceInnerStorage);

            bool mergeSuccess = false;

            // snapshot a merge előtt, hogy a clear ne befolyásolja
            var pageSnapshot = GlobalVariables.pages.ToList();

            try
            {
                await Task.Run(() =>
                {
                    using (PdfWriter writer = new PdfWriter(outputPdfPath))
                    using (PdfDocument destPdf = new PdfDocument(writer))
                    {
                        var openPdfs = new Dictionary<string, PdfDocument>();
                        try
                        {
                            foreach (var pageItem in pageSnapshot)
                            {
                                string filePath = Path.Combine(FileSystem.AppDataDirectory, pageItem.FileName);
                                bool isImage = pageItem.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                               pageItem.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                               pageItem.FileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase);

                                if (isImage)
                                {
                                    var imageData = ImageDataFactory.Create(filePath);
                                    float imgWidth = imageData.GetWidth();
                                    float imgHeight = imageData.GetHeight();

                                    PdfPage page;
                                    iText.Kernel.Pdf.Canvas.PdfCanvas pdfCanvas;

                                    switch (pageItem.Rotation)
                                    {
                                        case 90:
                                            page = destPdf.AddNewPage(new iText.Kernel.Geom.PageSize(imgHeight, imgWidth));
                                            pdfCanvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page);
                                            pdfCanvas.ConcatMatrix(0, -1, 1, 0, 0, imgWidth);
                                            break;
                                        case 180:
                                            page = destPdf.AddNewPage(new iText.Kernel.Geom.PageSize(imgWidth, imgHeight));
                                            pdfCanvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page);
                                            pdfCanvas.ConcatMatrix(-1, 0, 0, -1, imgWidth, imgHeight);
                                            break;
                                        case 270:
                                            page = destPdf.AddNewPage(new iText.Kernel.Geom.PageSize(imgHeight, imgWidth));
                                            pdfCanvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page);
                                            pdfCanvas.ConcatMatrix(0, 1, -1, 0, imgHeight, 0);
                                            break;
                                        default: // 0 fok
                                            page = destPdf.AddNewPage(new iText.Kernel.Geom.PageSize(imgWidth, imgHeight));
                                            pdfCanvas = new iText.Kernel.Pdf.Canvas.PdfCanvas(page);
                                            break;
                                    }

                                    pdfCanvas.AddImageAt(imageData, 0, 0, false);
                                    GlobalVariables.sumOfPages++;
                                }
                                else
                                {
                                    // PDF oldal másolása – meglévő logika
                                    if (!openPdfs.ContainsKey(pageItem.FileName))
                                    {
                                        var reader = new PdfReader(filePath).SetUnethicalReading(true);
                                        openPdfs[pageItem.FileName] = new PdfDocument(reader);
                                    }

                                    var sourcePdf = openPdfs[pageItem.FileName];
                                    sourcePdf.CopyPagesTo(pageItem.PageNumber, pageItem.PageNumber, destPdf);

                                    if (pageItem.Rotation != 0)
                                    {
                                        var copiedPage = destPdf.GetPage(destPdf.GetNumberOfPages());
                                        int currentRotation = copiedPage.GetRotation();
                                        copiedPage.SetRotation((currentRotation + pageItem.Rotation) % 360);
                                    }

                                    GlobalVariables.sumOfPages++;
                                }
                            }
                        }
                        finally
                        {
                            foreach (var pdf in openPdfs.Values)
                                pdf.Close();
                        }
                    }
                });

                mergeSuccess = true;
                await DisplayAlert("OK", $"PDF létrejött. Összesen {GlobalVariables.sumOfPages} oldal.", "OK");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error merging PDFs");
                await DisplayAlert("Hiba", ex.Message, "OK");
            }
            finally
            {
                StopLoadingAnimation();
                BtnAddPdf.IsEnabled = true;
                BtnClear.IsEnabled = true;
                BtnCreatePdf.IsEnabled = true;

                if (GlobalVariables.vibrateOnDone)
                    Vibration.Default.Vibrate(TimeSpan.FromSeconds(0.5));
            }

            if (mergeSuccess)
            {
                try
                {
                    await MoveFileFromAppDirectoryToDownloadAsync(FileSystem.AppDataDirectory,
                        GlobalVariables.outputFileNameCreatedOnDeviceInnerStorage);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error moving PDF to downloads");
                    await DisplayAlert("Hiba", ex.Message, "OK");
                }

                try
                {
                    if (GlobalVariables.autoOpenPdf)
                        await OpenCreatedPdf();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error opening PDF");
                    await DisplayAlert("Hiba", ex.Message, "OK");
                }
            }

            
            await DeleteTempFiles();
            await ClearGlobalVariables();
            await AutoQuit();
        }

        private async Task AutoQuit()
        {
            if (GlobalVariables.autoQuit)
            {
                await Task.Delay(5000);
                Application.Current.Quit();
            }
        }

        static int get_pageCcount(string file)
        {
            using (StreamReader sr = new StreamReader(File.OpenRead(file)))
            {
                return new Regex(@"/Type\s*/Page[^s]").Matches(sr.ReadToEnd()).Count;
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
            GlobalVariables.androidDestinationPath = destinationPath;
            //Source Path
            //string sourceFilePath = Path.Combine(FileSystem.AppDataDirectory, "test.txt");
            string sourceFilePath = Path.Combine(sourcePath, fileName);
            string timeStamp = "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            string finalName = GlobalVariables.outputFileName + timeStamp + ".pdf";
            GlobalVariables.androidDestinationFinalName = finalName;
            using (FileStream sourceStream = new FileStream(sourceFilePath, FileMode.OpenOrCreate))

            //using (FileStream destinationStream = new FileStream(Path.Combine(destinationPath, "output" + timeStamp + ".pdf"), FileMode.Create))
            using (FileStream destinationStream = new FileStream(Path.Combine(destinationPath, finalName), FileMode.Create))
            {
                sourceStream.CopyTo(destinationStream);
            }
# endif
        }

        public static async Task DeleteTempFiles()
        {
#if ANDROID
            foreach (var fileName in GlobalVariables.inputPdf)
            {
                string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
                if (File.Exists(path)) File.Delete(path);
            }

            string outputTemp = Path.Combine(FileSystem.AppDataDirectory,
                GlobalVariables.outputFileNameCreatedOnDeviceInnerStorage);
            if (File.Exists(outputTemp)) File.Delete(outputTemp);
#endif
        }

        public async Task CopyFileFromDownloadToAppDirectory()
        {
            var options = new PickOptions
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.Android, new[] { "application/pdf", "image/png", "image/jpeg" } }
        })
            };

            var result = await FilePicker.Default.PickAsync(options);
            if (result == null) return;

            bool isPdf = result.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);
            bool isImage = result.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                           result.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                           result.FileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase);

            if (!isPdf && !isImage)
            {
                await DisplayAlert("Hiba", "Csak PDF, PNG vagy JPEG fájlt lehet kiválasztani!", "OK");
                return;
            }

            var destPath = Path.Combine(FileSystem.AppDataDirectory, result.FileName);
            using var sourceStream = await result.OpenReadAsync();
            using var destStream = File.Create(destPath);
            await sourceStream.CopyToAsync(destStream);
            destStream.Close();

            GlobalVariables.inputPdf.Add(result.FileName.ToString());

#if ANDROID
    if (isPdf)
        await LoadPdfPagesAsync(destPath);
    else
        await LoadImagePageAsync(destPath, result.FileName);
#endif
        }


#if ANDROID
private async Task LoadImagePageAsync(string filePath, string fileName)
{
    var pageItem = new PdfPageItem
    {
        FileName = Path.GetFileName(filePath),
        PageNumber = 1,
        Preview = ImageSource.FromFile(filePath)
    };

    MainThread.BeginInvokeOnMainThread(() =>
    {
        GlobalVariables.pages.Add(pageItem);
    });
}
#endif

        public async Task OpenCreatedPdf()
        {
            var filePath = Path.Combine(GlobalVariables.androidDestinationPath, GlobalVariables.androidDestinationFinalName);

            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });


        }

        private void StartLoadingAnimation()
        {
            LoadingOverlay.IsVisible = true;

            var pulse = new Animation
                {
                    { 0.0, 0.5, new Animation(v => LoadingIcon.Scale = v, 1.0, 1.2, Easing.SinInOut) },
                    { 0.5, 1.0, new Animation(v => LoadingIcon.Scale = v, 1.2, 1.0, Easing.SinInOut) }
                };

            pulse.Commit(LoadingIcon, "PulseAnim", length: 1000, repeat: () => true);
        }

        private void StopLoadingAnimation()
        {
            LoadingIcon.AbortAnimation("PulseAnim");
            LoadingIcon.Scale = 1.0;
            LoadingOverlay.IsVisible = false;
        }

        private void MovePageUp_Clicked(object sender, TappedEventArgs e)
        {
            var item = e.Parameter as PdfPageItem;
            var index = GlobalVariables.pages.IndexOf(item);
            if (index > 0)
                GlobalVariables.pages.Move(index, index - 1);
        }

        private void MovePageDown_Clicked(object sender, TappedEventArgs e)
        {
            var item = e.Parameter as PdfPageItem;
            var index = GlobalVariables.pages.IndexOf(item);
            if (index < GlobalVariables.pages.Count - 1)
                GlobalVariables.pages.Move(index, index + 1);
        }

        private void DeletePage_Clicked(object sender, TappedEventArgs e)
        {
            var item = e.Parameter as PdfPageItem;
            GlobalVariables.pages.Remove(item);
        }

        private async void PreviewImage_Tapped(object sender, TappedEventArgs e)
        {
            var item = e.Parameter as PdfPageItem;
            await Navigation.PushModalAsync(new PagePreviewModal(item));
        }

        private void QuitApp_Clicked(object sender, EventArgs e)
        {
         
            Application.Current.Quit();
        }

#if ANDROID
        private async Task LoadPdfPagesAsync(string filePath)
        {
            await Task.Run(() =>
            {
                var file = new Java.IO.File(filePath);
                var fd = Android.OS.ParcelFileDescriptor.Open(file,
                    Android.OS.ParcelFileMode.ReadOnly);

                using var renderer = new Android.Graphics.Pdf.PdfRenderer(fd);

                for (int i = 0; i < renderer.PageCount; i++)
                {
                    using (var page = renderer.OpenPage(i))
                    {
                        int width = 600; 
                        int height = (int)(width * page.Height / (float)page.Width);

                        var bitmap = Android.Graphics.Bitmap.CreateBitmap(
                            width, height, Android.Graphics.Bitmap.Config.Argb8888);
                        bitmap.EraseColor(Android.Graphics.Color.White);
                        page.Render(bitmap, null, null,
                            Android.Graphics.Pdf.PdfRenderMode.ForDisplay);

                        page.Close();

                        using var stream = new MemoryStream();
                        bitmap.Compress(Android.Graphics.Bitmap.CompressFormat.Png, 80, stream);
                        stream.Position = 0;
                        var bytes = stream.ToArray();

                        var pageItem = new PdfPageItem
                        {
                            FileName = Path.GetFileName(filePath),
                            PageNumber = i + 1,
                            Preview = ImageSource.FromStream(() => new MemoryStream(bytes))
                        };

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            GlobalVariables.pages.Add(pageItem);
                        });
                    } 
                }
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (PagesCollectionView.ItemsLayout is GridItemsLayout gridLayout)
                gridLayout.Span = GlobalVariables.previewSpan;

            if (GlobalVariables.previousPreviewHeight != GlobalVariables.previewHeight)
            {
                GlobalVariables.previousPreviewHeight = GlobalVariables.previewHeight;
                _ = DeleteTempFiles();
                _ = ClearGlobalVariables();
            }
        }
#endif

        public async Task ClearGlobalVariables()
        {
            await Task.Run(() =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    GlobalVariables.inputPdf.Clear();
                    GlobalVariables.pages.Clear();
                    GlobalVariables.outputPdf = "";
                    GlobalVariables.sumOfPages = 0;
                    GlobalVariables.outputFileNameCreatedOnDeviceInnerStorage = "";
                    GlobalVariables.androidDestinationPath = "";
                    GlobalVariables.androidDestinationFinalName = "";
                });
            });
        }
    }

    public class PdfPageItem : INotifyPropertyChanged
    {
        public string FileName { get; set; }
        public int PageNumber { get; set; }
        public ImageSource Preview { get; set; }
        public string DisplayName => $"{PageNumber}. oldal";
        public int PreviewHeight => GlobalVariables.previewHeight;

        private int _rotation = 0;
        public int Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public static class GlobalVariables
    {
        public static ObservableCollection<string> inputPdf = new ObservableCollection<string>();
        public static String outputPdf = "";
        public static int sumOfPages = 0;
        public static string outputFileName = "output";
        public static string outputFileNameCreatedOnDeviceInnerStorage = "";
        public static string androidDestinationPath = "";
        public static string androidDestinationFinalName = "";
        public static bool vibrateOnDone = false;
        public static ObservableCollection<PdfPageItem> pages = new ObservableCollection<PdfPageItem>();
        public static bool autoOpenPdf = true;
        public static int previewHeight = 160;
        public static int previewSpan = 2;
        public static bool autoQuit = false;
        public static bool changePreview = false;
        public static int previousPreviewHeight = 400;
    }
}
