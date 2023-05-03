using EcoScanner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
	public class DishesViewModel : BaseViewModel
	{
		public string Title { get;set; }
		public DishesViewModel() 
		{
			Title = "Inspiration til retter";
		}
		
	}
}
