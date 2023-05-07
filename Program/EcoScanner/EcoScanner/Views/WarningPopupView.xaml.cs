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
	public partial class WarningPopupView : Rg.Plugins.Popup.Pages.PopupPage
	{
		BaseViewModel viewModel;
		public static bool onPopup = false;
		public WarningPopupView(BaseViewModel viewModel)
		{
			InitializeComponent();
			BindingContext = viewModel;
		}
		private void backgroundClosed(object sender, EventArgs e)
		{
			onPopup = false;
		}
	}
}