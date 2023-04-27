using System;
using System.Collections.Generic;
using System.Text;

namespace EcoScanner.ViewModels
{
    public class DishViewModel : BaseViewModel
    {
        public string Source { get; set; }
        public DishViewModel(string source)
        {
            source = source.Replace("ø", "oe");
			source = source.Replace("æ", "ae");
			source = source.Replace("å", "aa");
			source = source.Replace(" ", "_");
            this.Source = source + ".png";
        }
    }
}
