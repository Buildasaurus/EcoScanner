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
	public partial class ListOfItemsView : ContentView
	{
		ListOfItemsViewModel viewModel;
		public ListOfItemsView(ListOfItemsViewModel _viewModel)
		{
			BindingContext = viewModel = _viewModel;
			InitializeComponent();
		}
	}
}