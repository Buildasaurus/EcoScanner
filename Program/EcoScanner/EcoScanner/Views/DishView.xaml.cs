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
		DishViewModel dishViewModel;
		public DishView (Dish dish)
		{
			dishViewModel = new DishViewModel(dish);
			BindingContext = dishViewModel;
			InitializeComponent();
			
		}

	}
}