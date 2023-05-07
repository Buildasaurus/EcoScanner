using EcoScanner.Services;
using EcoScanner.ViewModels;
using EcoScanner.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EcoScanner.Models
{
	public static class ButtonCommands
	{
		/// <summary>
		/// Closes current popup
		/// </summary>
		public static async Task ClosePopupAsync()
		{
			WarningPopupView.onPopup = false;
			await PopupNavigation.Instance.PopAsync();

		}
		/// <summary>
		/// Goes to the history page, and closes the current popup.
		/// </summary>
		public static async Task GoToHistoryAsync()
		{
			History.addToHistory();
			ListeViewModel.invokeClearList();
			HistoryView.refreshView();
			await ClosePopupAsync();
			await Shell.Current.GoToAsync("//HistoryView");
		}
		/// <summary>
		/// Clears the list, and closes the current popup
		/// </summary>
		public static async Task ClearListAsync()
		{
			ListeViewModel.invokeClearList();
			await ClosePopupAsync();
		}
		/// <summary>
		/// Goes to Url of dish, and closes the current popup.
		/// </summary>
		/// <param name="dish"></param>
		public static async Task GoToUrlAsync(Dish dish)
		{
			Trace.WriteLine("acceptdeclineViewmodel");
			await ClosePopupAsync();
			Browser.OpenAsync(dish.URL).Wait();
		}
		public static async Task AddDishToListAsync(Dish dish)
		{
			var numbersAndWords = dish.CO2.Amount.Zip(dish.CO2.Vare, (n, w) => new { Weight = n, Name = w });
			// inefficient O(n^2), since i don't have the keys, but just the names of the individual dishes. Could be O(n) if i saved id with name
			foreach (var ingredient in numbersAndWords)
			{
				Product product = Databasehandler.GetProductByName(ingredient.Name);
				try
				{
					if(product != null)
					{
						product.Count = (int)Math.Ceiling(float.Parse(ingredient.Weight, CultureInfo.GetCultureInfo("en-GB")) / product.Weight);
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
			await ClosePopupAsync();
		}
	}
}
