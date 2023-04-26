using EcoScanner.Models;
using EcoScanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Forms.Internals.GIFBitmap;

namespace EcoScanner.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DishView : ContentView
	{
		Dish dish;
		DishViewModel dishViewModel;
		public DishView (Dish dish)
		{
			InitializeComponent ();
			this.dish = dish;
			dishViewModel = new DishViewModel(dish.Name);
			BindingContext = dishViewModel;
			RetName.Text = dish.Name;
			CO2udledning.Text = dish.TotalCo2 + " kg CO2e pr. person";

		}
		private void Button_Click(object sender, EventArgs e)
		{
			//write code to open file
		}
	}
}