using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

namespace Historikvisning
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Dictionary<DateTime, float> boef = new Dictionary<DateTime, float>();
		
		public MainWindow()
		{
			//Bare for at teste
			DateTime key = new DateTime(2023, 3, 23);
			boef.Add(key, 70);
			key = new DateTime(2023, 4, 24);
			boef.Add(key,100);
			key = new DateTime(2023, 4, 23);
			boef.Add(key, 60);
			key = new DateTime(2023, 5, 6);
			boef.Add(key, 60);


			InitializeComponent();
			MakeWeekBarGraph(boef);
		}

		void MakeWeekBarGraph(Dictionary<DateTime, float> historik)
		{
			DateTime d1 = new DateTime(2023, 3, 20);
			DateTime d2 = DateTime.Now;

			List<float> testForBest = new List<float>();

			while (0 > DateTime.Compare(d1, d2))
			{
				string weekDay = d1.DayOfWeek.ToString();
				int weekNum = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

				for (int i = 0; i < 7; i++)
				{

					weekDay = d1.DayOfWeek.ToString();
					weekNum = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

					float dayValue = 0;

					if (historik.ContainsKey(d1.Date))
					{
						dayValue = historik[d1.Date];
					}
					d1 = d1.AddDays(1);

					testForBest.Add(dayValue);
				}
			}

			d1 = new DateTime(2023, 3, 20);
			float best = 0;

			foreach (float f in testForBest)
			{
				if (f > best)
				{
					best = f;
				}
			}

			while ( 0 > DateTime.Compare(d1, d2))
			{
				string weekDay = d1.DayOfWeek.ToString();
				int weekNum = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

				Grid weekCategory = new Grid();
				RowDefinition wGridRow1 = new RowDefinition();
				RowDefinition wGridRow2 = new RowDefinition();
				wGridRow1.Height = new GridLength(30);
				weekCategory.RowDefinitions.Add(wGridRow1);
				weekCategory.RowDefinitions.Add(wGridRow2);
				var myBorder = new Border();
				myBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01ABC9"));
				//myBorder.Margin = new Thickness(2,0,2,0);
				weekCategory.Children.Add(myBorder);

				Line line = new Line();
				line.X1 = 0;
				line.Y1 = 0;
				line.X2 = 0;
				line.Y2 = 30;
				line.Stroke = Brushes.DarkSlateGray;
				line.StrokeThickness = 1;
				line.SetValue(Grid.RowProperty, 0);

				TextBlock textWeek = new TextBlock();
				textWeek.Text = "Uge " + weekNum;
				textWeek.Foreground = Brushes.White;
				textWeek.VerticalAlignment = VerticalAlignment.Bottom;
				textWeek.HorizontalAlignment = HorizontalAlignment.Center;
				textWeek.VerticalAlignment = VerticalAlignment.Center;
				textWeek.SetValue(Grid.RowProperty, 0);

				Grid weekGrid = new Grid();
				weekGrid.SetValue(Grid.RowProperty, 1);
				weekGrid.VerticalAlignment = VerticalAlignment.Bottom;
				for (int i = 0; i < 7; i++)
				{
					ColumnDefinition gridCol = new ColumnDefinition();
					weekGrid.ColumnDefinitions.Add(gridCol);
				}

				for (int i = 0; i < 7; i++)
				{
					Grid combiningColGrid = new Grid();
					RowDefinition gridRow1 = new RowDefinition();
					RowDefinition gridRow2 = new RowDefinition();
					gridRow1.Height = new GridLength(120);
					gridRow2.Height = new GridLength(30);

					combiningColGrid.RowDefinitions.Add(gridRow1);
					combiningColGrid.RowDefinitions.Add(gridRow2);
					
					weekDay = d1.DayOfWeek.ToString();
					weekNum = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

					TextBlock textDay = new TextBlock();
					textDay.Text = weekDay[0].ToString() + weekNum;
					textDay.Foreground = Brushes.Black;
					textDay.VerticalAlignment = VerticalAlignment.Bottom;
					textDay.HorizontalAlignment = HorizontalAlignment.Center;
					textDay.VerticalAlignment = VerticalAlignment.Center;
					textDay.SetValue(Grid.RowProperty, 1);

					float rectHeight = 0;

					Rectangle rect = new Rectangle();
					rect.VerticalAlignment = VerticalAlignment.Bottom;
					rect.Margin = new Thickness(5, 0, 5, 0);
					rect.Width = 30;
					rect.SetValue(Grid.RowProperty, 0);
					rect.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01ABC9"));
					if (historik.ContainsKey(d1.Date))
					{
						rectHeight = historik[d1.Date];
					}
					float maksAkseValue = ((int)Math.Round(best / 60)) * 60;
					float reelHeight = 120 * rectHeight / maksAkseValue;
					if (maksAkseValue == 0)
					{
						reelHeight = 0;
					}
						rect.Height = reelHeight;

					combiningColGrid.Children.Add(rect);
					combiningColGrid.Children.Add(textDay);

					combiningColGrid.SetValue(Grid.ColumnProperty, i);
					weekGrid.Children.Add(combiningColGrid);

					d1 = d1.AddDays(1);
				}

				weekCategory.Children.Add(textWeek);
				weekCategory.Children.Add(line);
				weekCategory.Children.Add(weekGrid);
				Chartdata.Children.Add(weekCategory);

			}
			
			JusterAkse(best);

		}

		void MakeMonthBarGraph(Dictionary<DateTime, float> historik)
		{
			DateTime d1 = new DateTime(2023, 3, 6);
			DateTime d2 = DateTime.Now;

			List<float> testForBest = new List<float>();

			while (0 > DateTime.Compare(d1, d2))
			{
				string weekDay = d1.DayOfWeek.ToString();
				int weekNum = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

				int weeksInMonth = 0;
				DateTime d1Keeper = d1;
				d1 = d1.AddDays(-d1.Day + 1);
				DateTime dCompare = d1.AddMonths(1);
				while (0 > DateTime.Compare(d1, dCompare))
				{
					if (d1.DayOfWeek.ToString() == "Monday")
					{
						weeksInMonth++;
					}
					d1 = d1.AddDays(1);
				}
				d1 = d1Keeper;

				for (int i = 0; i < weeksInMonth; i++)
				{

					float weekValue = 0;

					for (int j = 0; j < 7; j++)
					{
						if (historik.ContainsKey(d1.Date))
						{
							weekValue += historik[d1];
						}
						d1 = d1.AddDays(1);
					}

					testForBest.Add(weekValue);
				}

			}
				
			d1 = new DateTime(2023, 3, 6);
			float best = 0;

			foreach (float f in testForBest)
			{
				if (f > best)
				{
					best = f;
				}
			}

			while (0 > DateTime.Compare(d1, d2))
			{
				string weekDay = d1.DayOfWeek.ToString();
				int weekNum = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

				Grid monthCategory = new Grid();
				RowDefinition mGridRow1 = new RowDefinition();
				RowDefinition mGridRow2 = new RowDefinition();
				mGridRow1.Height = new GridLength(30);
				monthCategory.RowDefinitions.Add(mGridRow1);
				monthCategory.RowDefinitions.Add(mGridRow2);
				var myBorder = new Border();
				myBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01ABC9"));
				monthCategory.Children.Add(myBorder);

				Line line = new Line();
				line.X1 = 0;
				line.Y1 = 0;
				line.X2 = 0;
				line.Y2 = 30;
				line.Stroke = Brushes.DarkSlateGray;
				line.StrokeThickness = 1;
				line.SetValue(Grid.RowProperty, 0);

				TextBlock textMonth = new TextBlock();
				textMonth.Text = d1.ToString("MMMM");
				textMonth.Foreground = Brushes.White;
				textMonth.VerticalAlignment = VerticalAlignment.Bottom;
				textMonth.HorizontalAlignment = HorizontalAlignment.Center;
				textMonth.VerticalAlignment = VerticalAlignment.Center;
				textMonth.SetValue(Grid.RowProperty, 0);

				int weeksInMonth = 0;
				DateTime d1Keeper = d1;
				d1 = d1.AddDays(-d1.Day + 1);
				DateTime dCompare = d1.AddMonths(1);
				while (0 > DateTime.Compare(d1, dCompare))
				{
					if (d1.DayOfWeek.ToString() == "Monday")
					{
						weeksInMonth++;
					}
					d1 = d1.AddDays(1);
				}
				d1 = d1Keeper;

				Grid monthGrid = new Grid();
				monthGrid.VerticalAlignment = VerticalAlignment.Bottom;
				monthGrid.SetValue(Grid.RowProperty, 1);

				for (int i = 0; i < weeksInMonth; i++)
				{
					ColumnDefinition gridCol = new ColumnDefinition();
					monthGrid.ColumnDefinitions.Add(gridCol);
				}

				for (int i = 0; i < weeksInMonth; i++)
				{
					Grid combiningColGrid = new Grid();
					RowDefinition gridRow1 = new RowDefinition();
					RowDefinition gridRow2 = new RowDefinition();
					gridRow1.Height = new GridLength(120);
					gridRow2.Height = new GridLength(30);

					combiningColGrid.RowDefinitions.Add(gridRow1);
					combiningColGrid.RowDefinitions.Add(gridRow2);

					int ugeNummer = i + 1;

					TextBlock textDay = new TextBlock();
					textDay.Text = "W " + ugeNummer;
					textDay.Foreground = Brushes.Black;
					textDay.VerticalAlignment = VerticalAlignment.Bottom;
					textDay.HorizontalAlignment = HorizontalAlignment.Center;
					textDay.VerticalAlignment = VerticalAlignment.Center;
					textDay.SetValue(Grid.RowProperty, 1);

					float rectHeight = 0;

					Rectangle rect = new Rectangle();
					rect.VerticalAlignment = VerticalAlignment.Bottom;
					rect.Margin = new Thickness(5, 0, 5, 0);
					rect.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01ABC9"));
					rect.Width = 30;
					for (int j = 0; j < 7; j++)
					{
						rect.SetValue(Grid.RowProperty, 0);
						if (historik.ContainsKey(d1.Date))
						{
							rectHeight += historik[d1];
						}
						d1 = d1.AddDays(1);
					}
					float maksAkseValue = ((int)Math.Round(best / 60)) * 60;
					float reelHeight = 120 * rectHeight / maksAkseValue;
					if (maksAkseValue == 0)
					{
						reelHeight = 0;
					}
					rect.Height = reelHeight;


					combiningColGrid.Children.Add(rect);
					combiningColGrid.Children.Add(textDay);

					combiningColGrid.SetValue(Grid.ColumnProperty, i);
					monthGrid.Children.Add(combiningColGrid);
				}

				monthCategory.Children.Add(textMonth);
				monthCategory.Children.Add(line);
				monthCategory.Children.Add(monthGrid);
				Chartdata.Children.Add(monthCategory);

			}

			JusterAkse(best);

		}

		void MakeYearBarGraph(Dictionary<DateTime, float> historik)
		{
			DateTime d1 = new DateTime(2023, 1, 20);
			DateTime d2 = DateTime.Now;

			List<float> testForBest = new List<float>();

			while (0 > DateTime.Compare(d1, d2))
			{
				Grid yearGrid = new Grid();
				yearGrid.VerticalAlignment = VerticalAlignment.Bottom;
				yearGrid.SetValue(Grid.RowProperty, 1);
				for (int i = 0; i < 12; i++)
				{
					ColumnDefinition gridCol = new ColumnDefinition();
					yearGrid.ColumnDefinitions.Add(gridCol);
				}

				for (int i = 0; i < 12; i++)
				{
					float daysInMonth = 0;
					DateTime d1Keeper = d1;
					d1 = d1.AddDays(-d1.Day + 1);
					DateTime dCompare = d1.AddMonths(1);
					while (0 > DateTime.Compare(d1, dCompare))
					{
						daysInMonth++;
						d1 = d1.AddDays(1);
					}
					d1 = d1Keeper;

					float monthValue = 0;
					for (int j = 0; j < daysInMonth; j++)
					{
						if (historik.ContainsKey(d1.Date))
						{
							monthValue += historik[d1];
						}
						d1 = d1.AddDays(1);
					}

					testForBest.Add(monthValue);
				}

			}

			d1 = new DateTime(2023, 1, 20);
			float best = 0;

			foreach (float f in testForBest)
			{
				if (f > best)
				{
					best = f;
				}
			}

			while (0 > DateTime.Compare(d1, d2))
			{

				Grid yearCategory = new Grid();
				RowDefinition yGridRow1 = new RowDefinition();
				RowDefinition yGridRow2 = new RowDefinition();
				yGridRow1.Height = new GridLength(30);
				yGridRow2.Height = new GridLength(170);
				yearCategory.RowDefinitions.Add(yGridRow1);
				yearCategory.RowDefinitions.Add(yGridRow2);
				var myBorder = new Border();
				myBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01ABC9"));
				yearCategory.Children.Add(myBorder);

				Line line = new Line();
				line.X1 = 0;
				line.Y1 = 0;
				line.X2 = 0;
				line.Y2 = 30;
				line.Stroke = Brushes.DarkSlateGray;
				line.StrokeThickness = 1;
				line.SetValue(Grid.RowProperty, 0);

				TextBlock textYear = new TextBlock();
				textYear.Text = d1.ToString("yyyy");
				textYear.Foreground = Brushes.White;
				textYear.VerticalAlignment = VerticalAlignment.Bottom;
				textYear.HorizontalAlignment = HorizontalAlignment.Center;
				textYear.VerticalAlignment = VerticalAlignment.Center;
				textYear.SetValue(Grid.RowProperty, 0);

				Grid yearGrid = new Grid();
				yearGrid.VerticalAlignment = VerticalAlignment.Bottom;
				yearGrid.SetValue(Grid.RowProperty, 1);
				for (int i = 0; i < 12; i++)
				{
					ColumnDefinition gridCol = new ColumnDefinition();
					yearGrid.ColumnDefinitions.Add(gridCol);
				}

				for (int i = 0; i < 12; i++)
				{
					Grid combiningColGrid = new Grid();
					RowDefinition gridRow1 = new RowDefinition();
					RowDefinition gridRow2 = new RowDefinition();
					gridRow1.Height = new GridLength(140);
					gridRow2.Height = new GridLength(30);

					combiningColGrid.RowDefinitions.Add(gridRow1);
					combiningColGrid.RowDefinitions.Add(gridRow2);

					TextBlock textDay = new TextBlock();
					textDay.Text = d1.ToString("MMM");
					textDay.Foreground = Brushes.Black;
					textDay.VerticalAlignment = VerticalAlignment.Bottom;
					textDay.HorizontalAlignment = HorizontalAlignment.Center;
					textDay.VerticalAlignment = VerticalAlignment.Center;
					textDay.SetValue(Grid.RowProperty, 1);

					float daysInMonth = 0;
					DateTime d1Keeper = d1;
					d1 = d1.AddDays(-d1.Day + 1);
					DateTime dCompare = d1.AddMonths(1);
					while (0 > DateTime.Compare(d1, dCompare))
					{
						daysInMonth++;
						d1 = d1.AddDays(1);
					}
					d1 = d1Keeper;

					float rectHeight = 0;

					d1Keeper = d1;
					d1 = d1.AddDays(d1.Day);
					Rectangle rect = new Rectangle();
					rect.VerticalAlignment = VerticalAlignment.Bottom;
					rect.Margin = new Thickness(5, 0, 5, 0);
					rect.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01ABC9"));
					rect.Width = 30;
					for (int j = 0; j < daysInMonth; j++)
					{
						rect.SetValue(Grid.RowProperty, 0);
						if (historik.ContainsKey(d1.Date))
						{
							rectHeight += historik[d1];
						}
						d1 = d1.AddDays(1);
					}
					float maksAkseValue = ((int)Math.Round(best / 60)) * 60;
					float reelHeight = 120 * rectHeight / maksAkseValue;
					if (maksAkseValue == 0)
					{
						reelHeight = 0;
					}
					rect.Height = reelHeight;
					//d1 = d1Keeper;
					

					combiningColGrid.Children.Add(rect);
					combiningColGrid.Children.Add(textDay);

					combiningColGrid.SetValue(Grid.ColumnProperty, i);
					yearGrid.Children.Add(combiningColGrid);
				}

				yearCategory.Children.Add(textYear);
				yearCategory.Children.Add(line);
				yearCategory.Children.Add(yearGrid);
				Chartdata.Children.Add(yearCategory);

			}

			JusterAkse(best);

		}

		void JusterAkse(float topValue)
		{
			float maksAkseValue = topValue/6;
			int akseValue = ((int)Math.Round(maksAkseValue / 10)) * 10;

			for (int i = 0; i < 6; i++)
			{
				int specAkseValue = akseValue * (6 - i);
				string AkseName =  " " + specAkseValue;
				TextBlock AkseNum = new TextBlock();
				AkseNum.Text = AkseName;
				AkseNum.SetValue(Grid.RowProperty, 1 + i);
				AkseNum.VerticalAlignment= VerticalAlignment.Center;
				ChartRow.Children.Add(AkseNum);
			}

		}

		void KalibrerGenmVDig(Dictionary<DateTime, float> historik, int Hustandstal)
		{
			DateTime date =historik.ElementAt(0).Key;

			double DanskGenm = 57.5;
			float SumCO2 = 0;
			int divisor = 0;
			while (0 > DateTime.Compare(date, DateTime.Now))
			{
				if (historik.ContainsKey(date.Date))
				{
					SumCO2 += historik[date];
				}

				divisor++;
				date = date.AddDays(1);
			}

			double ditGenm = SumCO2 * 7 / (divisor * Hustandstal);
			DinUdledningText.Text = ditGenm.ToString() + "kg";

			if ( ditGenm < DanskGenm)
			{
				DinUdledningRect.Height = ditGenm / DanskGenm * 180;

				DanskUdledningRect.Height = 180;
				DanskUdledningText.Margin = new Thickness(0);
				if (ditGenm / DanskGenm * 180 < 30)
				{
					DinUdledningText.Margin = new Thickness(0, 0, 0, ditGenm / DanskGenm * 180);
					DinUdledningText.VerticalAlignment = VerticalAlignment.Bottom;
					DinUdledningText.Foreground = Brushes.Black;
				}
				else
				{
					DinUdledningText.Margin = new Thickness(0, 180 - ditGenm / DanskGenm * 180, 0, 0);
					DinUdledningText.VerticalAlignment = VerticalAlignment.Center;
					DinUdledningText.Foreground = Brushes.White;
				}
			} 
			else if (ditGenm > DanskGenm)
			{
				DinUdledningRect.Height = 180;
				DinUdledningText.Margin = new Thickness(0);
				DanskUdledningRect.Height = DanskGenm / ditGenm * 180;
				if (DanskGenm / ditGenm * 180 < 30)
				{
					DanskUdledningText.Margin = new Thickness(0, 0, 0, DanskGenm / ditGenm * 180);
					DanskUdledningText.VerticalAlignment = VerticalAlignment.Bottom;
					DanskUdledningText.Foreground = Brushes.Black;
				}
				else
				{
					DanskUdledningText.Margin = new Thickness(0, 180 - DanskGenm / ditGenm * 180, 0, 0);
					DanskUdledningText.VerticalAlignment = VerticalAlignment.Center;
					DanskUdledningText.Foreground = Brushes.White;
				}
			}
			else
			{
				DinUdledningRect.Height = 180;
				DinUdledningText.Margin = new Thickness(0);
				DinUdledningText.VerticalAlignment = VerticalAlignment.Center;
				DinUdledningText.Foreground = Brushes.White;
				DanskUdledningRect.Height = 180;
				DanskUdledningText.Margin = new Thickness(0);
				DanskUdledningText.VerticalAlignment = VerticalAlignment.Center;
				DanskUdledningText.Foreground = Brushes.White;
			}

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Chartdata.Children.Clear();
			ChartRow.Children.Clear();
			MakeWeekBarGraph(boef);

			SelectedButtonLine.SetValue(Grid.ColumnProperty, 0);
			SelectedButtonRect.SetValue(Grid.ColumnProperty, 0);
			ButtonName0.FontWeight = FontWeights.Bold;
			ButtonName1.FontWeight = FontWeights.Normal;
			ButtonName2.FontWeight = FontWeights.Normal;
			ButtonName0.Foreground = Brushes.White;
			ButtonName1.Foreground = Brushes.Black;
			ButtonName2.Foreground = Brushes.Black;

		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Chartdata.Children.Clear();
			ChartRow.Children.Clear();
			MakeMonthBarGraph(boef);

			SelectedButtonLine.SetValue(Grid.ColumnProperty,1);
			SelectedButtonRect.SetValue(Grid.ColumnProperty, 1);
			ButtonName0.FontWeight = FontWeights.Normal;
			ButtonName1.FontWeight = FontWeights.Bold;
			ButtonName2.FontWeight = FontWeights.Normal;
			ButtonName0.Foreground = Brushes.Black;
			ButtonName1.Foreground = Brushes.White;
			ButtonName2.Foreground = Brushes.Black;
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			Chartdata.Children.Clear();
			ChartRow.Children.Clear();
			MakeYearBarGraph(boef);

			SelectedButtonLine.SetValue(Grid.ColumnProperty, 2);
			SelectedButtonRect.SetValue(Grid.ColumnProperty, 2);
			ButtonName0.FontWeight = FontWeights.Normal;
			ButtonName1.FontWeight = FontWeights.Normal;
			ButtonName2.FontWeight = FontWeights.Bold;
			ButtonName0.Foreground = Brushes.Black;
			ButtonName1.Foreground = Brushes.Black;
			ButtonName2.Foreground = Brushes.White;

			//Bare for at teste det
			KalibrerGenmVDig(boef, 1);
		}


	}
}
