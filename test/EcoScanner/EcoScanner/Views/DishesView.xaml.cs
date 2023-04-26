using EcoScanner.Models;
using EcoScanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
		async void setupContent()
		{
			await Databasehandler.loadDishes();
			foreach (Dish dish in Databasehandler.dishes.Values)
			{
				Frame frame = new Frame();
				frame.CornerRadius = 10;
				frame.Content = new DishView(dish);
				frame.Margin = 10;


				var tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "TapCommand");
				tapGestureRecognizer.CommandParameter = dish;
				frame.GestureRecognizers.Add(tapGestureRecognizer);


				frame.BackgroundColor = Color.LightBlue;
				stack.Children.Add(frame);
			}
		}
	}
}