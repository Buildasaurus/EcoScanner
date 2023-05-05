using EcoScanner.Models;
using EcoScanner.Services;
using EcoScanner.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.WindowsSpecific;

namespace EcoScanner.ViewModels
{
    public class ListeViewModel : BaseViewModel
	{
        private Product _selectedItem;
        public ObservableCollection<Product> Items { get; set; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Product> ItemTapped { get; }
		public static event EventHandler SumChanged; 
        public Command MinusClicked { get; set; }
        public Command ClearListClicked { get; set; }
        public Command PlusClicked { get; set; }
        public Command ReturnPressed { get; set; }
        public Command AddToHistory { get; set; }
        public int Number { get; set; } = 0;
		public static event EventHandler ClearList;
        public static event EventHandler ListeChanged;
		public string Total
		{
			get
            {
				return Liste.getSum().ToString("0.00"); 
            }
		}
		public ListeViewModel()
        {
            Title = "Liste";
            Items = new ObservableCollection<Product>();
            LoadItemsCommand = new Command(() => ExecuteLoadItemsCommand());

            ItemTapped = new Command<Product>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
            PlusClicked = new Command<Product>(plusClicked);
			MinusClicked = new Command<Product>(minusClicked);
            ClearListClicked = new Command(clearList);
            ReturnPressed = new Command(returnPressed);
            AddToHistory = new Command(historyPressed);
			ClearList += (sender, e) => clearTheList();
			SumChanged += (sender, e) => OnTotalChanged();
			ListeChanged += (sender, e) => refreshList();

		}
        public static void invoke()
        {
			SumChanged.Invoke(null, EventArgs.Empty);
		}
        public static void invokeClearList()
        {
            ClearList.Invoke(null, EventArgs.Empty);
        }
        void refreshList()
        {
            updateItem();
		}

		public static async void invokeRefreshList()
        {
            ListeChanged.Invoke(null, EventArgs.Empty);
		}

        void clearTheList()
        {
			updateItem();
			OnPropertyChanged(null);
			Items.Clear();
            Liste.clearFile();
		}
        async void returnPressed()
        {
			await Shell.Current.GoToAsync("//AboutPage");
		}
		public void OnTotalChanged()
		{
			OnPropertyChanged(nameof(Total));
		}
        void updateItem()
        {

			IsBusy = true; //this causes the refresh circle to appear - without it though, you can click fast, and it breaks the updating for some reason.

			IsBusy = false;
		}
        async void historyPressed()
        {
			WarningPopupViewModel viewModel = new WarningPopupViewModel("Vil du rydde listen og gemme din udledning i historikken?\nDette kan ikke gøres om",
                new TwoButtonWarningView(new StandardTwoButtonViewModel(() => ButtonCommands.ClosePopup(), () => ButtonCommands.GoToHistoryAsync(), "TilbageKnap.png", "TilfoejTilHistorikKnap.png")));
			await PopupNavigation.Instance.PushAsync(new WarningPopupView(viewModel));
			
		}

		void plusClicked(Product item)
        {
			if (item == null)
				return;

            item.Count = 1;
			Liste.saveProduct(item);
			updateItem();
		}
        void minusClicked(Product item)
        {
			if (item == null)
				return;

            item.Count = -1;
			Liste.saveProduct(item);
			updateItem();
		}
		async void clearList()
        {
			WarningPopupViewModel viewModel = new WarningPopupViewModel("Er du sikker på at du vil slette listen?\nDette kan ikke gøres om",
							new TwoButtonWarningView(new StandardTwoButtonViewModel(
                                () => ButtonCommands.ClosePopup(), () => ButtonCommands.ClearListAsync(), 
                                "TilbageKnap.png", "RydListenKnap.png")));
			await PopupNavigation.Instance.PushAsync(new WarningPopupView(viewModel));
		}

		void ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                Trace.WriteLine(Liste.readText());
                List<Product> products = Liste.getProducts();
                var items = products;
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
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

        private async void OnAddItem(object obj)
        {
			Liste.saveProduct(new Product(2, "hello", 3.14f, 1.59f, "kg"));
            OnPropertyChanged(null);
			//await Shell.Current.GoToAsync(nameof(NewItemPage));
		}


		async void OnItemSelected(Product item)
        {
            if (item == null)
                return;

			// This will push the ItemDetailPage onto the navigation stack
			if (!MyPopup.onPopup)
			{
				MyPopup.onPopup = true;
				await PopupNavigation.Instance.PushAsync(new MyPopup(item, true));
			}
		}
	}
}