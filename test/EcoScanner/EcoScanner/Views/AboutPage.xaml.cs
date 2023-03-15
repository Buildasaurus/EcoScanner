using EcoScanner.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;
using static ZXing.Mobile.MobileBarcodeScanningOptions;


namespace EcoScanner.Views
{
    public partial class AboutPage : ContentPage
    {
        double height = 0;
        double width = 0;
        public AboutPage()
        {
            InitializeComponent();
			height = abc.Height;
            width = abc.Width;

			zxing.OnScanResult += (result) => Device.BeginInvokeOnMainThread(() => {
                lblResult.Text = result.Text;
            });
        }
        protected override void OnAppearing()
        {
			var options = new ZXing.Mobile.MobileBarcodeScanningOptions()
			{
				PossibleFormats = new List<ZXing.BarcodeFormat>() { ZXing.BarcodeFormat.QR_CODE },
				CameraResolutionSelector = new CameraResolutionSelectorDelegate((resolutions) => SelectLowestResolutionMatchingDisplayAspectRatio(resolutions, abc))
			};
			zxing.Options = options;

            base.OnAppearing();
            zxing.IsScanning = true;
            var PossibleFormats = new List<ZXing.BarcodeFormat>() { ZXing.BarcodeFormat.QR_CODE };
          
        }
        protected override void OnDisappearing()
        {
            zxing.IsScanning = false;
            base.OnDisappearing();
        }

        
        public static CameraResolution SelectLowestResolutionMatchingDisplayAspectRatio(List<CameraResolution> availableResolutions, Grid abc)
        {
            CameraResolution result = null;

            //a tolerance of 0.1 should not be visible to the user
            double aspectTolerance = 0.3;

			var displayOrientationHeight = DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Portrait ? DeviceDisplay.MainDisplayInfo.Height : DeviceDisplay.MainDisplayInfo.Width;
            var displayOrientationWidth = DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Portrait ? DeviceDisplay.MainDisplayInfo.Width : DeviceDisplay.MainDisplayInfo.Height;
            try
            {
                var others = abc.RowDefinitions[0].Height.Value + abc.RowDefinitions[2].Height.Value;
                displayOrientationHeight = abc.Height - others;
                displayOrientationWidth = abc.Width;
				Trace.WriteLine("using heights " + displayOrientationHeight + " and with " + displayOrientationWidth);

			}
			catch
            {
                Trace.WriteLine("Error: Couldn't get heights and widths.");
            }

			//calculatiing our targetRatio
			var targetRatio = displayOrientationHeight / displayOrientationWidth;
            var targetHeight = displayOrientationHeight;
            var minDiff = double.MaxValue;

			//camera API lists all available resolutions from highest to lowest, perfect for us
			//making use of this sorting, following code runs some comparisons to select the lowest resolution that matches the screen aspect ratio and lies within tolerance
			//selecting the lowest makes Qr detection actual faster most of the time
			foreach (var r in availableResolutions.Where(r => Math.Abs(((double)r.Width / r.Height) - targetRatio) < aspectTolerance))
            {
                //slowly going down the list to the lowest matching solution with the correct aspect ratio
                if (Math.Abs(r.Height - targetHeight) < minDiff)
                    minDiff = Math.Abs(r.Height - targetHeight);
                result = r;
            }

            try
            {
                Trace.WriteLine("result" + result.Height + " " + result.Width);
            }
            catch
            {
                Trace.WriteLine("There was no available camera resolution that fits the height " + displayOrientationHeight + " and with " + displayOrientationWidth);
            }
            foreach (var b in availableResolutions)
            {
				Trace.WriteLine(b.Height + " and width " + b.Width);
			}

			return result;
        }
    }
}