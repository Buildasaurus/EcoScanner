using EcoScanner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
    public class DishViewModel : BaseViewModel
    {
        public string Source { get; set; }
		public Command<Dish> TapCommand { get; set; }
		public Dish MyDish { get; set; }
		public string DishName { get; set; }
		public string CO2udledning { get; set; }

		public DishViewModel(Dish dish)
        {
			this.MyDish = dish;
			string source = MyDish.Name;
			this.DishName = dish.Name;
			this.CO2udledning = dish.TotalCo2.ToString("0.00") + " kg CO2e pr. person";
			source = source.Replace("ø", "oe");
			source = source.Replace("æ", "ae");
			source = source.Replace("å", "aa");
			source = source.Replace(" ", "_");
            this.Source = source + ".png";
			TapCommand = new Command<Dish>(tapCommand);
		}
		public void tapCommand(Dish dish)
		{
			Trace.WriteLine(dish.Name);
			Browser.OpenAsync(dish.URL).Wait();
		}
	}
}
