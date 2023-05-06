using EcoScanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcoScanner.ViewModels
{
	public class ListOfItemsViewModel
	{
		public List<Product> Items { get; set; }
		public float Total { get; set; }
		public ListOfItemsViewModel(List<Product> _items) 
		{
			Total = _items.Sum(p => p.TotCo2);
			Items = _items;
		}
	}
}
