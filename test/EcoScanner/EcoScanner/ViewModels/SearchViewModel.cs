using EcoScanner.Models;
using EcoScanner.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing;

namespace EcoScanner.ViewModels
{
	public class SearchViewModel : BaseViewModel
	{
		public SearchViewModel() 
		{
			SearchInitilized = new Command(search);
			ItemTapped = new Command<Product>(OnItemSelected);
			Items = new ObservableCollection<Product>();
			Title = "Søg efter Produkter";

		}
		private Product _selectedItem;
		public ObservableCollection<Product> Items { get; }
		public Command LoadItemsCommand { get; }
		public Command AddItemCommand { get; }
		public Command<Product> ItemTapped { get; }
		public Command SearchInitilized { get; set; }
		public string Text { get; set; }
		public string Info { get; set; }
		public string NoResultLabel { get; set; }
		public bool LabelVisible { get; set; }
		public async void search()
		{
			IsBusy = true;

			await Databasehandler.LoadAllProducts();
			List<Product> products = Databasehandler.Search(Text.Trim());
			string builder = "";
			Items.Clear();
			if (products.Count != 0)
			{
				foreach (var item in products)
				{
					Items.Add(item);
				}
				NoResultLabel = string.Empty;
				LabelVisible = false;
			}
			else
			{
				LabelVisible = true;
				NoResultLabel = "Intet resultat";
				//do stuff TODO
			}
			
			OnPropertyChanged(null);
			IsBusy = false;
		}
		public Product SelectedItem
		{
			get => _selectedItem;
			set
			{
				SetProperty(ref _selectedItem, value);
				OnItemSelected(value);
			}
		}

		public void onAppearing()
		{
			Trace.WriteLine("before isbusy = " + IsBusy);
			IsBusy = true;
			Items.Clear();
			if (!MyPopup.onPopup)
			{
				Text = string.Empty;
			}
			OnPropertyChanged(nameof(Items));
			IsBusy=false;

		}
		async void OnItemSelected(Product item)
		{
			if (item == null)
				return;

			if (!MyPopup.onPopup)
			{
				MyPopup.onPopup = true;
				item.Count = 1;
				await PopupNavigation.Instance.PushAsync(new MyPopup(item));
			}
		}
	}
}
