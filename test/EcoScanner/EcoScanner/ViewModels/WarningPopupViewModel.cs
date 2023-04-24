using EcoScanner.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
	public class WarningPopupViewModel : BaseViewModel
	{
		public string Message { get; set; }
		public ContentView ButtonContent { get; set; }
		public WarningPopupViewModel(string message, int buttons)
		{
			Message = message;
			if(buttons == 1) 
			{
				ButtonContent = new SingleButtonView();
			}
			else if(buttons == 2)
			{
				ButtonContent = new TwoButtonWarningView();
			}
		}

	}
}
