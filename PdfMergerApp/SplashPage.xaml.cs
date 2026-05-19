using System;
using System.Collections.Generic;
using System.Text;

namespace PdfMergerApp
{
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            StartPulseAnimation();
            await Task.Delay(2500);
            Application.Current.MainPage = new AppShell();
        }

        private void StartPulseAnimation()
        {
            var pulse = new Animation
            {
                { 0.0, 0.5, new Animation(v => SplashIcon.Scale = v, 1.0, 1.2, Easing.SinInOut) },
                { 0.5, 1.0, new Animation(v => SplashIcon.Scale = v, 1.2, 1.0, Easing.SinInOut) }
            };

            pulse.Commit(SplashIcon, "SplashPulse", length: 1000, repeat: () => true);
        }
    }
}
