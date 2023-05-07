using EcoScanner.Models;
using EcoScanner.ViewModels;
using EcoScanner.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcoScanner.Views
{
    public partial class ListePage : ContentPage
    {
        ListeViewModel _viewModel;

        public ListePage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ListeViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
	}
}