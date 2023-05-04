using EcoScanner.Models;
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
using EcoScanner.Views;
using EcoScanner.Services;

namespace EcoScanner.ViewModels
{
    public class MyPopupViewModel : BaseViewModel
	{
		public string ProductName { get; set; }
		public string ProductUnit { get 
			{
				if (product.Weight < 1 && product.Unit == "kg")
				{
					return (product.Weight*1000).ToString("0.0") + " g";
				}
				else
				{
					return product.Weight + " " + product.Unit;
				}
			}
		}
		public string ScalePath { get; set; }
		public string weightOf1kg { get 
			{
				string a = "" + product.CO2.ToString("0.0") + " kg CO2e";
				return a;
			}
		}
		public string Description { get; set; }
		public string TotalWeight { get; set; }
		public string Color { get; set; }
		public string Number { get; set; }
		Product product;
		public Command MinusClicked { get; }
		public Command PlusClicked { get; }
		public Command CloseClicked { get; }
		public float Weight { get; set; }
		public string Unit { get; set; }
		string[] colors = { "#237536", "#5bc45b", "#fce475", "#ffac3d", "#ff7a35", "#d53d3a" };
		bool onListe;

		public string ConfirmPath { get; set; }
		public MyPopupViewModel(Product product, bool onListe) 
		{
			this.onListe = onListe;
			Number = "1";
			this.product = product;
			ProductName = product.Name;
			Number = "" + product.Count;
			this.Weight = product.Weight;
			this.Unit = product.Unit;

			//commands for buttons
			AddToList = new Command(addToList);
			PlusClicked = new Command(plusClicked);
			MinusClicked = new Command(minusClicked);
			CloseClicked = new Command(closeClicked);

			//image correction for scale
			double[] intervals = { 1.0, 2.5, 4.0, 7.5, 10.0, 1000.0 };
			int mappedNum = Array.IndexOf(intervals, intervals.First(x => x > product.CO2));
			string path = "SkalaKlasse" + mappedNum + ".png";
			if(!onListe)
			{
				ConfirmPath = "TilfoejTilListeKnap.png";
			}
			else
			{
				ConfirmPath = "GemKnap.png";
			}
			ScalePath = path;
			Color = colors[mappedNum];

			//update other numbers on popup
			updateNumbers();
		}
		public Command AddToList { get; set; }

		void addToList()
		{
			
			product.Count = int.Parse(Number);
			if (!onListe)
			{
				Liste.saveProduct(product);
			}
			else
			{
				Liste.overrideProduct(product);
				ListeViewModel.invokeRefreshList();
			}
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
			float totalWeight = Weight * int.Parse(Number);
			if (totalWeight < 1 && Unit == "kg") 
			{
				Description = "Udledningen af " + (totalWeight*1000).ToString("0.0") + " g bliver:";
			}
			else
			{
				Description = "Udledningen af " + totalWeight + " " + Unit + " bliver:";
			}
			TotalWeight = (totalWeight * product.CO2).ToString("0.0") + " kg CO2e";

			OnPropertyChanged(null);
		}
	}
}
