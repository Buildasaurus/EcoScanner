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
	public partial class SearchView : ContentPage
	{
		SearchViewModel viewModel;
		public SearchView()
		{
			InitializeComponent();
			viewModel = new SearchViewModel();
		}
		protected async override void OnAppearing()
		{
			base.OnAppearing();

			await Task.Run(async () =>
			{
				await Task.Delay(100);
				Device.BeginInvokeOnMainThread(async () =>
				{
					søgebar.Focus();
				});
			});
		}

	}
}