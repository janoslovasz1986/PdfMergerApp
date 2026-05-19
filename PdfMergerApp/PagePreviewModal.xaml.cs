using System;
using System.Collections.Generic;
using System.Text;

namespace PdfMergerApp
{
    public partial class PagePreviewModal : ContentPage
    {
        private readonly PdfPageItem _item;
        private double _currentScale = 1.0;

        public PagePreviewModal(PdfPageItem item)
        {
            InitializeComponent();
            _item = item;
            PreviewImage.Source = item.Preview;
            PreviewImage.Rotation = item.Rotation;
        }

        private void ZoomIn_Clicked(object sender, EventArgs e)
        {
            _currentScale = Math.Min(_currentScale + 0.25, 4.0);
            PreviewImage.Scale = _currentScale;
        }

        private void ZoomOut_Clicked(object sender, EventArgs e)
        {
            _currentScale = Math.Max(_currentScale - 0.25, 0.5);
            PreviewImage.Scale = _currentScale;
        }

        private void RotateLeft_Clicked(object sender, EventArgs e)
        {
            _item.Rotation = (_item.Rotation + 270) % 360;
            PreviewImage.Rotation = _item.Rotation;
        }

        private void RotateRight_Clicked(object sender, EventArgs e)
        {
            _item.Rotation = (_item.Rotation + 90) % 360;
            PreviewImage.Rotation = _item.Rotation;
        }

        private async void Close_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
