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
			DateTime key = new DateTime(2023, 4, 24);
			boef.Add(key,100);
			key = new DateTime(2023, 4, 23);
			boef.Add(key, 60);

			InitializeComponent();
			MakeWeekBarGraph(boef);
			ChartScroller.ScrollToRightEnd();
		}

		void MakeWeekBarGraph(Dictionary<DateTime, float> historik)
		{
			DateTime d1 = new DateTime(2023, 3, 20);
			DateTime d2 = DateTime.Now;

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
				myBorder.Margin = new Thickness(2,0,2,0);
				weekCategory.Children.Add(myBorder);

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
					gridRow1.Height = new GridLength(80);
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

					Rectangle rect = new Rectangle();
					rect.VerticalAlignment = VerticalAlignment.Bottom;
					rect.Margin = new Thickness(5, 0, 5, 0);
					rect.Width = 30;
					rect.SetValue(Grid.RowProperty, 0);
					rect.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01ABC9"));
					if (historik.ContainsKey(d1.Date))
					{
						rect.Height = historik[d1.Date];
					}
					else
					{
						rect.Height = 0;
					}

					combiningColGrid.Children.Add(rect);
					combiningColGrid.Children.Add(textDay);

					combiningColGrid.SetValue(Grid.ColumnProperty, i);
					weekGrid.Children.Add(combiningColGrid);

					d1 = d1.AddDays(1);
				}

				weekCategory.Children.Add(textWeek);
				weekCategory.Children.Add(weekGrid);
				Chartdata.Children.Add(weekCategory);

			}
		}

		void MakeMonthBarGraph(Dictionary<DateTime, float> historik)
		{
			DateTime d1 = new DateTime(2023, 3, 20);
			DateTime d2 = DateTime.Now;

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
				myBorder.Margin = new Thickness(2, 0, 2, 0);
				myBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01ABC9"));
				monthCategory.Children.Add(myBorder);

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
					gridRow1.Height = new GridLength(80);
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
					rect.Height = rectHeight;
					

					combiningColGrid.Children.Add(rect);
					combiningColGrid.Children.Add(textDay);

					combiningColGrid.SetValue(Grid.ColumnProperty, i);
					monthGrid.Children.Add(combiningColGrid);
				}

				monthCategory.Children.Add(textMonth);
				monthCategory.Children.Add(monthGrid);
				Chartdata.Children.Add(monthCategory);

			}
		}

		void MakeYearBarGraph(Dictionary<DateTime, float> historik)
		{
			DateTime d1 = new DateTime(2023, 1, 20);
			DateTime d2 = DateTime.Now;

			while (0 > DateTime.Compare(d1, d2))
			{

				Grid yearCategory = new Grid();
				RowDefinition yGridRow1 = new RowDefinition();
				RowDefinition yGridRow2 = new RowDefinition();
				yGridRow1.Height = new GridLength(30);
				yearCategory.RowDefinitions.Add(yGridRow1);
				yearCategory.RowDefinitions.Add(yGridRow2);
				var myBorder = new Border();
				myBorder.Margin = new Thickness(2, 0, 2, 0);
				myBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01ABC9"));
				yearCategory.Children.Add(myBorder);

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
					gridRow1.Height = new GridLength(80);
					gridRow2.Height = new GridLength(30);

					combiningColGrid.RowDefinitions.Add(gridRow1);
					combiningColGrid.RowDefinitions.Add(gridRow2);

					TextBlock textDay = new TextBlock();
					textDay.Text = d1.ToString("MMMM")[0].ToString().ToUpper();
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
					rect.Height = rectHeight;

					int weeksInMonth = 0;
					

					combiningColGrid.Children.Add(rect);
					combiningColGrid.Children.Add(textDay);

					combiningColGrid.SetValue(Grid.ColumnProperty, i);
					yearGrid.Children.Add(combiningColGrid);

					d1 = d1.AddDays(1);
				}

				yearCategory.Children.Add(textYear);
				yearCategory.Children.Add(yearGrid);
				Chartdata.Children.Add(yearCategory);

			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Chartdata.Children.Clear();
			MakeWeekBarGraph(boef);
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Chartdata.Children.Clear();
			MakeMonthBarGraph(boef);
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			Chartdata.Children.Clear();
			MakeYearBarGraph(boef);
		}
	}
}
