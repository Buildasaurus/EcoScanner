using System;
using System.Collections.Generic;
using System.Text;

namespace EcoScanner.Models
{
	public class HistoryData
	{
		public float Total { get; set; }
		public List<Product> Products { get; set; }
		public HistoryData(float _total, List<Product> _products)
		{
			Total = _total;
			Products = _products;
		}
	}
}
