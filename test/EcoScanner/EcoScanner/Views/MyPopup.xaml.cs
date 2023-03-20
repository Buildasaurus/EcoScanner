using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms;

namespace MyNamespace
{
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
	}
}
