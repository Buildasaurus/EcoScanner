using EcoScanner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
	public class SearchViewModel
	{
		public SearchViewModel() 
		{
			SearchInitilized = new Command(search);
		}
		public Command SearchInitilized { get; set; }
		public string Text { get; set; }
		void search()
		{
			Databasehandler.LoadAllProducts();
			List<Product> products = Databasehandler.Search(Text);
			foreach (Product product in products)
			{
				Console.WriteLine(product.Name);
			}
		}
	}
}
