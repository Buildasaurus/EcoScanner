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
		void backPressed()
		{
			//close popup
			Trace.WriteLine("close popup");
		}
		void clearListPressed()
		{
			Trace.WriteLine("clear pressd");
			ListeViewModel.invokeClearList();

		}
	}
}
