using Rg.Plugins.Popup.Services;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyNamespace
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyPopup : Rg.Plugins.Popup.Pages.PopupPage
	{
		public MyPopup()
		{
			InitializeComponent();
		}
		private async void Close_Click(object sender, EventArgs e)
		{
			await PopupNavigation.Instance.PopAsync();
		}

		private void Add_Clicked(object sender, EventArgs e)
		{

		}

		private void Close_Clicked(object sender, EventArgs e)
		{

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
