using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EcoScanner.ViewModels
{
	public class HistoryViewModel : BaseViewModel
	{
		public static event EventHandler RefreshEventhandler;

		public HistoryViewModel()
		{
			Title = "Historik";
			RefreshEventhandler += (sender, e) => localRefresh();

		}
		public static void refreshView()
		{
			try //Might fail, since the history may never have been opened before.
			{
				RefreshEventhandler.Invoke(null, EventArgs.Empty);
			}
			catch { }
		}
		void localRefresh()
		{
			OnPropertyChanged(null);
			IsBusy = true; //this causes the refresh circle to appear - without it though, you can click fast, and it breaks the updating for some reason.

			IsBusy = false;
		}
	}
}
