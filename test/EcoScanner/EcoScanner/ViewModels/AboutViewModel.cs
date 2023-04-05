using EcoScanner.Models;
using MyNamespace;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing;
using ZXing.Mobile;

namespace EcoScanner.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
			ScanCommand = new Command(OnScanResultCommand);
			MyPopup.onPopup = false;
		}
		public void Main()
        {
		}

		private async void OnScanResultCommand()
		{
			if (!MyPopup.onPopup)
			{
				MyPopup.onPopup = true;
				bool parsed = int.TryParse(Result.Text, out int number);
				if (parsed)	
				{
					Product product = Databasehandler.GetProduct(number);
					await PopupNavigation.Instance.PushAsync(new MyPopup(product));
				}
				else
				{
					MyPopup.onPopup = false;
					Trace.WriteLine("not a number");
				}
				//result.Text
			}
		}
		public Result Result { get; set; }
		public Command ScanCommand { get; set; }
	}


}