using EcoScanner.Models;
using EcoScanner.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
	public class AcceptDeclinePopupViewModel : BaseViewModel
	{
		public Command BackPressed { get; set; }
		public Command ClearListPressed { get; set; }
		Dish dish { get; set; }
		public AcceptDeclinePopupViewModel(Dish dish)
		{
			BackPressed = new Command(backPressed);
			ClearListPressed = new Command(clearListPressed);
			this.dish = dish;
		}
		async void backPressed()
		{
			//close popup
			Trace.WriteLine("acceptdeclinneviewmodel back");
			WarningPopupView.onPopup = false;
			await PopupNavigation.Instance.PopAsync();

		}
		async void clearListPressed()
		{
			Trace.WriteLine("acceptdeclineViewmodel");
			WarningPopupView.onPopup = false;
			await PopupNavigation.Instance.PopAsync();
			Browser.OpenAsync(dish.URL).Wait();
		}
	}
}

