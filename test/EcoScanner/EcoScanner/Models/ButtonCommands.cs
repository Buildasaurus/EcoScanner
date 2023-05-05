using EcoScanner.Services;
using EcoScanner.ViewModels;
using EcoScanner.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EcoScanner.Models
{
	public static class ButtonCommands
	{
		/// <summary>
		/// Closes current popup
		/// </summary>
		public static async void ClosePopup()
		{
			WarningPopupView.onPopup = false;
			await PopupNavigation.Instance.PopAsync();

		}
		/// <summary>
		/// Goes to the history page, and closes the current popup.
		/// </summary>
		public static async void GoToHistoryAsync()
		{
			History.addToHistory();
			HistoryView.refreshView();
			ClosePopup();
			await Shell.Current.GoToAsync("//HistoryView");

		}
		/// <summary>
		/// Clears the list, and closes the current popup
		/// </summary>
		public static async void ClearListAsync()
		{
			ListeViewModel.invokeClearList();
			ClosePopup();
		}
		/// <summary>
		/// Goes to Url of dish, and closes the current popup.
		/// </summary>
		/// <param name="dish"></param>
		public static async void GoToUrlAsync(Dish dish)
		{
			Trace.WriteLine("acceptdeclineViewmodel");
			ClosePopup();
			Browser.OpenAsync(dish.URL).Wait();
		}
		public static async void AddDishToListAsync(Dish dish)
		{
			var numbersAndWords = dish.CO2.Amount.Zip(dish.CO2.Vare, (n, w) => new { Name = n, Weight = w });
			// inefficient O(n^2), since i don't have the keys, but just the names of the individual dishes. Could be O(n) if i saved id with name
			foreach (var ingredient in numbersAndWords)
			{
				Product product = Databasehandler.GetProductByName(ingredient.Name);
				try
				{
					if(product != null)
					{
						product.Count = (int)Math.Ceiling(product.Weight * float.Parse(ingredient.Weight));
						Liste.saveProduct(product);
					}
					else
					{
						Trace.WriteLine("Product " + product.Name + " could not be found");
					}
				}
				catch
				{
					Trace.WriteLine("Couldn't read weight as float");
				}
			}
			ClosePopup();
		}
	}
}
