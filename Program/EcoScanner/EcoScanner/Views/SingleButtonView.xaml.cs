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
	public partial class SingleButtonView : ContentView
	{
		BaseViewModel ViewModel;
		public SingleButtonView(BaseViewModel viewModel)
		{
			ViewModel = viewModel;
			BindingContext = ViewModel;
			InitializeComponent();
		}
	}
}