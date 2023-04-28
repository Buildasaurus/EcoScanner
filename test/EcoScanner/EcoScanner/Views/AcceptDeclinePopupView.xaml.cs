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
	public partial class AcceptDeclinePopupView : ContentView
	{
		AcceptDeclinePopupViewModel viewModel;
		public AcceptDeclinePopupView(Dish dish)
		{
			viewModel = new AcceptDeclinePopupViewModel(dish);
			BindingContext = viewModel;
			InitializeComponent();
		}
	}
}