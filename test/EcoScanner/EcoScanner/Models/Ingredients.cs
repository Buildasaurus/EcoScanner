using System;
using System.Collections.Generic;
using System.Text;

namespace EcoScanner.Models
{
    public class Ingredients
    {
		public List<string> Amount { get; set; } //Can't change the name - but is the weights of the ingredeints
		/// <summary>
		/// The name of all the ingredients
		/// </summary>
		public List<string> Vare { get; set; } 
	}
}
