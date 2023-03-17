using Xamarin.Forms;

namespace MyNamespace
{
	public class MyPopup : ContentPage
	{
		public MyPopup()
		{
			// set the page properties
			BackgroundColor = Color.FromHex("#80000000");
			Padding = new Thickness(20);

			// add content to the popup
			Content = new StackLayout
			{
				Children = {
					new Label { Text = "This is a popup!", TextColor = Color.White },
					new Button { Text = "Close", BackgroundColor = Color.Red, TextColor = Color.White }
				}
			};

			//Creates a cool close button, and "subscribes" to the click event. When it is clicked it pops (closes) the popup.
			var closeButton = new Button { Text = "Close", BackgroundColor = Color.Red, TextColor = Color.White };
			closeButton.Clicked += async (sender, args) =>
			{
				await Navigation.PopModalAsync();
			};

		}
	}
}
