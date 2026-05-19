using System;
using System.Collections.Generic;
using System.Text;

namespace PdfMergerApp
{
    public partial class PagePreviewModal : ContentPage
    {
        private readonly PdfPageItem _item;

        public PagePreviewModal(PdfPageItem item)
        {
            InitializeComponent();
            _item = item;
            PreviewImage.Source = item.Preview;
            PreviewImage.Rotation = item.Rotation;
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
