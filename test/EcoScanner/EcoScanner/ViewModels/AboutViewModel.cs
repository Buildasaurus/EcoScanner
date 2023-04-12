using EcoScanner.Models;
using EcoScanner.Views;
using MyNamespace;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
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
			OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart")); //DO NOT REMOVE - Causes invisible crash - just lovely

			ScanCommand = new Command(OnScanResultCommand);
			MyPopup.onPopup = false;
			ListeClicked = new Command(Liste_clicked);
			SearchCommand = new Command(searchClicked);
			SearchFocused = new Command(searchClicked);
		}
		public Command SearchCommand { get; set; }
		public bool IsScanning { get; set; }
		public Command ListeClicked { get; }
		public Command SearchFocused { get; set; }




		public ICommand OpenWebCommand { get; } //DO NOT REMOVE - Causes invisible crash - just lovely

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
		private async void searchClicked()
		{
			Trace.WriteLine(Shell.Current.CurrentState);
			await Shell.Current.GoToAsync("//SearchView");
		}
		private async void Liste_clicked()
		{
			IsScanning = false;

			//change View
			//BindingContext = new Liste();
			Trace.WriteLine(Shell.Current.CurrentState);
			await Shell.Current.GoToAsync("//ListePage");

		}
	}


}