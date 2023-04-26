using System;
using System.Collections.Generic;
using System.Text;

namespace EcoScanner.ViewModels
{
    public class DishViewModel
    {
        public string Source { get; set; }
        public DishViewModel(string source)
        {
            this.Source = source;
        }
    }
}
