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
		public Dish MyDish { get; set; }
		DishViewModel dishViewModel;
		public DishView (Dish dish)
		{
			this.MyDish = dish;
			dishViewModel = new DishViewModel(MyDish.Name);
			BindingContext = dishViewModel;
			InitializeComponent();
			RetName.Text = MyDish.Name;
			CO2udledning.Text = MyDish.TotalCo2.ToString("0.00") + " kg CO2e pr. person";
		}
		private void Button_Click(object sender, EventArgs e)
		{
			//write code to open file
		}
	}
}