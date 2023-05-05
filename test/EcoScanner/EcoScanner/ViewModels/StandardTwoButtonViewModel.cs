using EcoScanner.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
	public class StandardTwoButtonViewModel : BaseViewModel
	{
		public Command LeftPressed { get; set; }
		public Command RightPressed { get; set; }
		public string LeftImage { get; set; }
		public string RightImage { get; set; }
		public StandardTwoButtonViewModel(Action leftButton, Action rightButton, string leftImageSource, string rightImageSource)
		{
			LeftPressed = new Command(leftButton);
			RightPressed = new Command(rightButton);
			LeftImage = leftImageSource;
			RightImage = rightImageSource;
		}
	}
}