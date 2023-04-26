using EcoScanner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
	public class DishesViewModel
	{
		public Command<Dish> TapCommand { get; set; }
		public DishesViewModel() 
		{
			TapCommand = new Command<Dish>(tapCommand);
		}
		public void tapCommand(Dish dish)
		{
			Trace.WriteLine(dish.Name);
		}
	}
}
