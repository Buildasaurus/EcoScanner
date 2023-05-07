using EcoScanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcoScanner.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ThreeButtonView : ContentView
	{
		ThreeButtonViewModel viewModel;
		public ThreeButtonView(ThreeButtonViewModel _viewmodel)
		{
			viewModel = _viewmodel;
			BindingContext = viewModel;
			InitializeComponent();
		}
	}
}