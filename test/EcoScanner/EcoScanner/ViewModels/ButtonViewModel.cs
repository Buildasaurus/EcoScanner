using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
	public class ButtonViewModel : BaseViewModel
	{
		public Command BackPressed { get; set; }
		public Command ClearListPressed { get; set; }
		public ButtonViewModel() 
		{
			BackPressed = new Command(backPressed);
			ClearListPressed = new Command(clearListPressed);
		}
		async void backPressed()
		{
			//close popup
			Trace.WriteLine("close popup");
			await PopupNavigation.Instance.PopAsync();

		}
		async void clearListPressed()
		{
			Trace.WriteLine("clear pressd");
			ListeViewModel.invokeClearList();
			await PopupNavigation.Instance.PopAsync();

		}
	}
}
