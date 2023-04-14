using EcoScanner.ViewModels;
using MyNamespace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.PlatformConfiguration.iOSSpecific;
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
			viewModel.onAppearing();
			søgebar.Text = string.Empty;
			ItemsListView.ItemsSource = viewModel.Items;

			await Task.Run(async () =>
			{
				await Task.Delay(100);
				Device.BeginInvokeOnMainThread(async () =>
				{
					søgebar.Focus();

				});
			});
		}
		protected async override void OnDisappearing()
		{
			base.OnDisappearing();
			Trace.WriteLine("Search Disappearing");
		}
	}
}