using EcoScanner.Models;
using EcoScanner.Views;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
    public class DishViewModel : BaseViewModel
    {
        public string Source { get; set; }
		public Command<Dish> TapCommand { get; set; }
		public Dish MyDish { get; set; }
		public string DishName { get; set; }
		public string CO2udledning { get; set; }

		public DishViewModel(Dish dish)
        {
			this.MyDish = dish;
			string source = MyDish.Name;
			this.DishName = dish.Name;
			this.CO2udledning = dish.TotalCo2.ToString("0.00") + " kg CO₂e pr. person";
			source = source.Replace("ø", "oe");
			source = source.Replace("æ", "ae");
			source = source.Replace("å", "aa");
			source = source.Replace(" ", "_");
            this.Source = source + ".png";
			TapCommand = new Command<Dish>(tapCommand);
		}
		public async void tapCommand(Dish dish)
		{
			WarningPopupViewModel viewmodel = new WarningPopupViewModel("Åben opskriften på COOPs hjemmeside eller tilføj ingredienser til listen", 
				new ThreeButtonView( new ThreeButtonViewModel(
					async () => await ButtonCommands.ClosePopupAsync(),
					async () => await ButtonCommands.AddDishToListAsync(dish),
					async () => await ButtonCommands.GoToUrlAsync(dish),
					"TilbageKnap.png", "TilfoejTilListeKnap.png", "TilHjemmeside.png"
					)));
			await PopupNavigation.Instance.PushAsync(new WarningPopupView(viewmodel));
		}
	}
}
