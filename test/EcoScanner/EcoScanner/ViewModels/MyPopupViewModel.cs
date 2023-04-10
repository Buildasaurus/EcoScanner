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
	public class MyPopupViewModel : BaseViewModel
	{
		public string Text { get; set; }

		public MyPopupViewModel() 
		{
			Text = "hej";
			AddToList = new Command(addToList);
		}
		public Command AddToList { get; set; }

		void addToList()
		{
			Liste.saveProduct(new Product(2, "popupproduct", (float)6.28));
		}
	}
}
