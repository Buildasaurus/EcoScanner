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
		private async void Close_Click(object sender, EventArgs e)
		{
			await PopupNavigation.Instance.PopAsync();
			onPopup = false;
		}

		private void updateNumbers()
		{
			single.Text = "" + product.CO2.ToString("0.0") + " kg CO2e";
			description.Text = "Udledningen af " + number.Text + " bliver:";
			tot.Text = (int.Parse(number.Text) * product.CO2).ToString("0.0") + " kg CO2e";
		}

		private void backgroundClosed(object sender, EventArgs e)
		{
			onPopup = false;
		}

		private void Minus_Clicked(object sender, EventArgs e)
		{
			int num = int.Parse(number.Text);
			if (num > 0)
				{
				number.Text = "" + (num - 1);
			}
			updateNumbers();
		}

		private void Plus_Clicked(object sender, EventArgs e)
		{
			number.Text = "" + (int.Parse(number.Text) + 1);
			updateNumbers();
		}
	}
}
