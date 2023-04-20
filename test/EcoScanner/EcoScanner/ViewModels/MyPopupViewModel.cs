using EcoScanner.Models;
using MyNamespace;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
		public string ProductName { get; set; }
		public string ProductUnit { get; set; }
		public string ScalePath { get; set; }
		public string weightOf1kg { get; set; }
		public string Description { get; set; }
		public string TotalWeight { get; set; }
		public string Number { get; set; }
		Product product;
		public Command MinusClicked { get; }
		public Command PlusClicked { get; }
		public Command CloseClicked { get; }
		float weight;
		string unit;

		public MyPopupViewModel(Product product) 
		{
			Number = "1";
			this.product = product;
			ProductName = product.Name;

			//commands for buttons
			AddToList = new Command(addToList);
			PlusClicked = new Command(plusClicked);
			MinusClicked = new Command(minusClicked);
			CloseClicked = new Command(closeClicked);

			//image correction for scale
			double[] intervals = { 1.0, 2.5, 4.0, 7.5, 10.0, 1000.0 };
			int mappedNum = Array.IndexOf(intervals, intervals.First(x => x > product.CO2));
			string path = "SkalaKlasse" + mappedNum + ".png";
			ScalePath = path;

			//update other numbers on popup
			updateNumbers();
		}
		public MyPopupViewModel(Product product, float weight, string unit)
		{
			Number = "1";
			this.product = product;
			ProductName = product.Name;
			ProductUnit = weight + " " + unit;
			this.weight = weight;
			this.unit = unit;



			//commands for buttons
			AddToList = new Command(addToList);
			PlusClicked = new Command(plusClicked);
			MinusClicked = new Command(minusClicked);
			CloseClicked = new Command(closeClicked);

			//image correction for scale
			double[] intervals = { 1.0, 2.5, 4.0, 7.5, 10.0, 1000.0 };
			int mappedNum = Array.IndexOf(intervals, intervals.First(x => x > product.CO2));
			string path = "SkalaKlasse" + mappedNum + ".png";
			ScalePath = path;

			//update other numbers on popup
			updateNumbers();
		}
		public Command AddToList { get; set; }

		void addToList()
		{
			product.Count = int.Parse(Number);
			Liste.saveProduct(product);
			closeClicked();
		}
		void minusClicked()
		{
			int num = int.Parse(Number);
			if (num > 0)
			{
				Number = "" + (num - 1);
			}
			updateNumbers();
		}
		private void plusClicked()
		{
			Number = "" + (int.Parse(Number) + 1);
			updateNumbers();
		}
		async void closeClicked()
		{
			await PopupNavigation.Instance.PopAsync();
			MyPopup.onPopup = false;
		}
		private void updateNumbers()
		{
			weightOf1kg = "" + product.CO2.ToString("0.0") + " kg CO2e";
			Description = "Udledningen af " + Number + " vare bliver:";
			TotalWeight = (weight*int.Parse(Number) * product.CO2).ToString("0.0") + " kg CO2e";
			OnPropertyChanged(null);
		}
	}
}
