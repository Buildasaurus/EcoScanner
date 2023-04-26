using EcoScanner.Models;
using EcoScanner.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcoScanner.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyPopup : Rg.Plugins.Popup.Pages.PopupPage
	{
		public static bool onPopup = false;
		MyPopupViewModel _viewModel;

		public Product product;
		public MyPopup(Product product, bool onListe = false)
		{
			
			InitializeComponent();
			BindingContext = _viewModel = new MyPopupViewModel(product, onListe);

			this.product = product;
		}
		private void backgroundClosed(object sender, EventArgs e)
		{
			onPopup = false;
		}
	}
}
