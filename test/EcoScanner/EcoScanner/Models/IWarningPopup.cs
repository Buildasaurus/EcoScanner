using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
    public interface IWarningPopup
    {
		 string Message { get; set; }
		 ContentView ButtonContent { get; set; }
	}
}
