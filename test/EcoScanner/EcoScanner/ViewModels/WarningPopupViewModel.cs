using EcoScanner.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
	public class WarningPopupViewModel : BaseViewModel, IWarningPopup
	{
		public string Message { get; set; }
		public ContentView ButtonContent { get; set; }
		public WarningPopupViewModel(string message, ContentView content)
		{
			Message = message;
			ButtonContent = content;
		}

	}
}
