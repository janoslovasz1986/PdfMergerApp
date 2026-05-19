using System;
using System.Collections.Generic;
using System.Text;

namespace PdfMergerApp
{
    public partial class PagePreviewModal : ContentPage
    {
        private readonly PdfPageItem _item;

        private double _startScale = 1;
        private bool _pinchStarted = false;
        private double _startX = 0;
        private double _startY = 0;

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
        private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            if (e.Status == GestureStatus.Started)
            {
                _startScale = PreviewImage.Scale;
                _pinchStarted = true;
            }

            if (e.Status == GestureStatus.Running)
            {
                // ha Started még nem tüzelt, itt kapjuk el
                if (!_pinchStarted)
                {
                    _startScale = PreviewImage.Scale;
                    _pinchStarted = true;
                }

                PreviewImage.Scale = Math.Clamp(_startScale * e.Scale, 1, 5);
            }

            if (e.Status == GestureStatus.Completed)
            {
                _pinchStarted = false;

                if (PreviewImage.Scale <= 1)
                {
                    PreviewImage.TranslationX = 0;
                    PreviewImage.TranslationY = 0;
                }
            }
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Started)
            {
                _startX = PreviewImage.TranslationX;
                _startY = PreviewImage.TranslationY;
            }

            if (e.StatusType == GestureStatus.Running && PreviewImage.Scale > 1)
            {
                PreviewImage.TranslationX = _startX + e.TotalX;
                PreviewImage.TranslationY = _startY + e.TotalY;
            }

            if (e.StatusType == GestureStatus.Completed && PreviewImage.Scale <= 1)
            {
                PreviewImage.TranslationX = 0;
                PreviewImage.TranslationY = 0;
            }
        }
    }
}
