using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
	public class SearchViewModel
	{
		public SearchViewModel() 
		{
			SearchInitilized = new Command(search);
		}
		public Command SearchInitilized { get; set; }

		void search()
		{

		}
	}
}
