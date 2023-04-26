using System;
using System.Collections.Generic;
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
using InteractiveDataDisplay.WPF;

namespace Visning_af_historik
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow(Dictionary<string, float> CO2)
		{
			InitializeComponent();
			MakeBarGraph();
		}

		void MakeBarGraph()
		{
			Rectangle box = new Rectangle();
			box.VerticalAlignment = VerticalAlignment.Bottom;
			box.Margin = new Thickness(0, 5, 0,0);
			box.Fill = Brushes.Blue;
			box.Height = 100;
			box.Width = 50;
			Chartdata.Children.Add(box);
		}

	}
}
