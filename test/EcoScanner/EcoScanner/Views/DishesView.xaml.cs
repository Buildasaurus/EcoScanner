using EcoScanner.Models;
using EcoScanner.Services;
using EcoScanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcoScanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DishesView : ContentPage
	{
		DishesViewModel dishesViewModel;
		public DishesView()
		{
			InitializeComponent();
			dishesViewModel = new DishesViewModel();
			BindingContext = dishesViewModel;
			setupContent();
		}
		int mod(int x, int m)
		{
			int r = x % m;
			return r < 0 ? r + m : r;
		}
		async void setupContent()
		{
			await Databasehandler.loadDishes();
			int modDay  = mod((DateTime.Today - new DateTime(1519, 03, 14)).Days, 3);

			Dish dayDish = Databasehandler.dishes["" + modDay];
			DayDish.Content = new DishView(dayDish);
			int count = 0;
			foreach (Dish dish in Databasehandler.dishes.Values)
			{
				if (count == modDay)
				{
					count++;
					continue;
				}
				count++;
				stack.Children.Add(new DishView(dish));
			}
		}
	}
}