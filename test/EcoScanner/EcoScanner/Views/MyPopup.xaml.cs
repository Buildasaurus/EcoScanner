using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MyNamespace
{
	public partial class MyPopup : Rg.Plugins.Popup.Pages.PopupPage
	{
		public MyPopup()
		{
			InitializeComponent();

			// set the page properties
			BackgroundColor = Color.FromRgb(200, 10, 53);
			Padding = new Thickness(20);

			// add content to the popup
			Content = new StackLayout
			{
				Children = {
					new Label { Text = "This is a popup!", TextColor = Color.Green },
				}
			};
			//Creates a cool close button, and "subscribes" to the click event. When it is clicked it pops (closes) the popup.
			var closeButton = new Button { Text = "Close", BackgroundColor = Color.Red, TextColor = Color.Black };
			closeButton.Clicked += async (sender, args) =>
			{
				await PopupNavigation.Instance.PopAsync();
			};
			((StackLayout)Content).Children.Add(closeButton);

		}
	}
}
