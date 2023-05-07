using EcoScanner.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        public int ID {get; set;}
		public string Name { get; set; }
		public float CO2 { get; set; }

		public NewItemViewModel()
        {
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }



        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }


    }
}
