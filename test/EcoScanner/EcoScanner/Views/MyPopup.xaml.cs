using Xamarin.Forms;

namespace MyNamespace
{
	public class MyPopup : Rg.Plugins.Popup.Pages.PopupPage
	{
		public MyPopup()
		{
			// set the page properties
			BackgroundColor = Color.FromRgb(200, 10, 53);
			Padding = new Thickness(20);

			// add content to the popup
			Content = new StackLayout
			{
				Children = {
					new Label { Text = "This is a popup!", TextColor = Color.Green },
					new Button { Text = "Close", BackgroundColor = Color.Red, TextColor = Color.Yellow }
				}
			};

			//Creates a cool close button, and "subscribes" to the click event. When it is clicked it pops (closes) the popup.
			var closeButton = new Button { Text = "Close", BackgroundColor = Color.Red, TextColor = Color.Black };
			closeButton.Clicked += async (sender, args) =>
			{
				await Navigation.PopModalAsync();
			};

		}
	}
}
