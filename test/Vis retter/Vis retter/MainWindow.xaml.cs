using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vis_retter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Retter ret = new Retter();
		public MainWindow(Retter ret)
		{
			InitializeComponent();
			RetName.Text = ret.Name;
			CO2udledning.Text = ValueAfRet + "kg CO2e pr. person";
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start(new ProcessStartInfo
			{
				FileName = ret.URL,
				UseShellExecute = true
			});
		}
	}
}
