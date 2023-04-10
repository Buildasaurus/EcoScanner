using System;
using System.Collections.Generic;
using System.Text;

namespace EcoScanner.Models
{
    public class Product
    {
        public int ID { get; set; }
		public string Name { get; set; }
		public float CO2 { get; set; }
		public Product(int ID, string name, float CO2)
		{
			this.ID = ID;
			this.Name = name;
			this.CO2 = CO2;
		}
	}
}
