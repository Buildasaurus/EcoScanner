using EcoScanner.Models;
using EcoScanner.Views;
using MyNamespace;
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


		}
		private Product _selectedItem;
		public ObservableCollection<Product> Items { get; }
		public Command LoadItemsCommand { get; }
		public Command AddItemCommand { get; }
		public Command<Product> ItemTapped { get; }
		public Command SearchInitilized { get; set; }
		public string Text { get; set; }
		public string Info { get; set; }
		async void search()
		{
			IsBusy = true;

			await Databasehandler.LoadAllProducts();
			List<Product> products = Databasehandler.Search(Text);
			string builder = "";
			Items.Clear();
			foreach (var item in products)
			{
				Items.Add(item);
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

		async void OnItemSelected(Product item)
		{
			if (item == null)
				return;

			if (!MyPopup.onPopup)
			{
				MyPopup.onPopup = true;
				await PopupNavigation.Instance.PushAsync(new MyPopup(item));
			}
		}
	}
}
