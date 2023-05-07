using System;
using System.Collections.Generic;
using System.Text;

namespace EcoScanner.Models
{
    public class Dish
    {
		public Ingredients CO2 { get; set; }
		public int ID { get; set; }
		public string Name { get; set; }
		public string URL { get; set; }
		public float TotalCo2 { get; set; }
	}
}
