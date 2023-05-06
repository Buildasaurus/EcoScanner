using EcoScanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EcoScanner.ViewModels
{
	public class ListOfItemsViewModel
	{
		public List<Product> Items { get; set; }
		public float Total { get; set; }
		public ListOfItemsViewModel(List<Product> _items) 
		{
			Total = 3.1415926535f;
			Items = _items;
		}
	}
}
