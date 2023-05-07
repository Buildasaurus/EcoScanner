using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;

namespace EcoScanner.Models
{
	public class Product
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public float CO2 { get; set; }
		public int Count { get; set; }
		public float TotCo2 { get
			{
				return Count*CO2*Weight;
			}
			 set {}
		}
		public string Unit { get; set; }
		public float Weight { get; set; }

		public Product(int ID, string name, float CO2, float Weight, string Unit, int count = 1)
		{
			this.ID = ID;
			this.Name = name;
			this.CO2 = CO2;
			this.Count = count;
			this.Weight = Weight;
			this.Unit = Unit;
		}
	}
}
