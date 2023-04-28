using System;
using System.Collections.Generic;
using System.Text;

namespace EcoScanner.ViewModels
{
    public class InformationViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public InformationViewModel() 
        {
            Title = "Information";
        }
    }
}
