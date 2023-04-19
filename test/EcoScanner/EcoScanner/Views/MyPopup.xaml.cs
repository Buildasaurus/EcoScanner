using EcoScanner.Models;
using EcoScanner.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyNamespace
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyPopup : Rg.Plugins.Popup.Pages.PopupPage
	{
		public static bool onPopup = false;
		MyPopupViewModel _viewModel;

		public Product product;
		public MyPopup(Product product)
		{
			
			InitializeComponent();
			BindingContext = _viewModel = new MyPopupViewModel(product);

			this.product = product;
			//heading.Text = product.Name;
			/*double[] intervals = {1.0, 2.5, 4.0, 7.5, 10.0, 1000.0};
			int mappedNum = Array.IndexOf(intervals, intervals.First(x => x > product.CO2));
			string path = "SkalaKlasse" + mappedNum + ".png";
			updateNumbers();
			scale.Source = path;*/
		}
		public MyPopup(Product product, float weight, string unit)
		{

			InitializeComponent();
			BindingContext = _viewModel = new MyPopupViewModel(product, weight, unit);

			this.product = product;
		}
		private void backgroundClosed(object sender, EventArgs e)
		{
			onPopup = false;
		}
	}
}
