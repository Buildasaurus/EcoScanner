using EcoScanner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
	public class SearchViewModel : BaseViewModel
	{
		public SearchViewModel() 
		{
			SearchInitilized = new Command(search);
		}
		public Command SearchInitilized { get; set; }
		public string Text { get; set; }
		public string Info { get; set; }
		async void search()
		{
			await Databasehandler.LoadAllProducts();
			List<Product> products = Databasehandler.Search(Text);
			string builder = "";
			foreach (Product product in products)
			{
				builder += product.Name + "\n";
				Console.WriteLine(product.Name);
			}
			Info = builder;
			OnPropertyChanged(null);
		}
	}
}
