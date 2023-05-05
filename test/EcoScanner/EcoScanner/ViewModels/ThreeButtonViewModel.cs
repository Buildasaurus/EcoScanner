using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
	public class ThreeButtonViewModel
	{
		public Command RightPressed { get; set; }
		public Command MiddlePressed { get; set; }
		public Command LeftPressed { get; set; }
		public string LeftImage { get; set; }
		public string MiddleImage { get; set; }
		public string RightImage { get; set; }
		public ThreeButtonViewModel(Action leftButton, Action middleButton, Action rightButton, string leftImageSource, 
			string middleImageSource, string rightImageSource)
		{
			LeftPressed = new Command(leftButton);
			RightPressed = new Command(rightButton);
			MiddlePressed = new Command(middleButton);
			LeftImage = leftImageSource;
			MiddleImage = middleImageSource;
			RightImage = rightImageSource;
		}
	}
}
