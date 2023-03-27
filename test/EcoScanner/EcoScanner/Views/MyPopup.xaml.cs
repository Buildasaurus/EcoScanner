using EcoScanner.Models;
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
		public Product product;
		public MyPopup(Product product)
		{
			this.product = product;
			InitializeComponent();
			heading.Text = product.Name;
			double[] intervals = { 0.0, 1.0, 2.0, 3.0, 4.0, 5.0, 6.0 };
			int mappedNum = Array.IndexOf(intervals, intervals.First(x => x > product.CO2));
			string path = "SkalaKlasse" + mappedNum + ".png";
			
			scale.Source = path;

		}
		private async void Close_Click(object sender, EventArgs e)
		{
			await PopupNavigation.Instance.PopAsync();
			onPopup = false;
		}

		private void Add_Clicked(object sender, EventArgs e)
		{

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
		}

		private void Plus_Clicked(object sender, EventArgs e)
		{
			number.Text = "" + (int.Parse(number.Text) + 1);
		}
	}
}
