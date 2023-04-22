﻿using EcoScanner.Models;
using EcoScanner.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
    public class ListeViewModel : BaseViewModel
	{
        private Product _selectedItem;
        public ObservableCollection<Product> Items { get; set; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Product> ItemTapped { get; }
		public string fileText {get;set;}
		public static event EventHandler SumChanged; 
        public Command MinusClicked { get; set; }
        public Command PlusClicked { get; set; }
        public int Number { get; set; } = 0;
        
        public string Total
		{
			get
            {
				return Liste.getSum().ToString("0.00 "); 
            }
		}
		public ListeViewModel()
        {
            Title = "Liste";
            Items = new ObservableCollection<Product>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Product>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
            PlusClicked = new Command<Product>(plusClicked);
			MinusClicked = new Command<Product>(minusClicked);

			fileText = "nothing yet";
            SumChanged += (sender, e) => OnTotalChanged();
		}
        public static void invoke()
        {
			SumChanged.Invoke(null, EventArgs.Empty);
		}
		public void OnTotalChanged()
		{
			OnPropertyChanged((nameof(Total)));
		}
        void updateItem(ref Product item)
        {
			Liste.saveProduct(item);

			int count = 0;
			//cursed way of making sure that Items realises it has been changed.
			IsBusy = true; //this causes the refresh circle to appear - without it though, you can click fast, and it breaks the updating for some reason.

			foreach (Product it in Items)
			{
				if (it.Name == item.Name)
				{
					break;
				}
				count++;
			}
			IsBusy = false;
		}
        void plusClicked(Product item)
        {
			if (item == null)
				return;

            item.Count = 1;
			updateItem(ref item);
		}
        void minusClicked(Product item)
        {
			if (item == null)
				return;

            item.Count = -1;
            updateItem(ref item);
		}
		async Task ExecuteLoadItemsCommand()
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
			Liste.saveProduct(new Product(2, "hello", (float)3.14, 3));
            fileText = Liste.readText();
            OnPropertyChanged(null);
			//await Shell.Current.GoToAsync(nameof(NewItemPage));
		}


		async void OnItemSelected(Product item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.Name)}={item.Name}");
        }
    }
}