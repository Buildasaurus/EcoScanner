﻿using EcoScanner.Models;
using EcoScanner.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
    public class ListeViewModel : BaseViewModel
    {
        private Product _selectedItem;

        public ObservableCollection<Product> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Product> ItemTapped { get; }

        public ListeViewModel()
        {
            Title = "Liste";
            Items = new ObservableCollection<Product>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Product>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                List<Product> products = new List<Product>();
                for (int i = 0; i < 20; i++) 
                {
                    products.Add(Databasehandler.GetProduct(i));
				}
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
            await Shell.Current.GoToAsync(nameof(NewItemPage));
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