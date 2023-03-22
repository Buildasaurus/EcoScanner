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
	}
}
