﻿using EcoScanner.Models;
using EcoScanner.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EcoScanner.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryView : ContentPage
	{

		Dictionary<DateTime, float> boef = new Dictionary<DateTime, float>();
		public static event EventHandler RefreshEventhandler;

		public HistoryView()
		{
			BindingContext = new HistoryViewModel();
			boef = History.getHistory();
			//Bare for at teste
			DateTime key = new DateTime(2023, 3, 23);
			boef.Add(key, 70);
			key = new DateTime(2023, 4, 24);
			boef.Add(key, 100);
			key = new DateTime(2023, 4, 23);
			boef.Add(key, 60);
			RefreshEventhandler += (sender, e) => localRefresh();


			InitializeComponent();
			MakeWeekBarGraph(boef);
		}
		public static void refreshView()
		{
			HistoryViewModel.refreshView();
			try//Might fail, if hasn't been on the histoy page yet.
			{
				RefreshEventhandler.Invoke(null, EventArgs.Empty);
			}
			catch { }
		}
		void localRefresh()
		{
			boef = History.getHistory();
			Button_Click(null, EventArgs.Empty);
			OnPropertyChanged(null);
			IsBusy = true; //this causes the refresh circle to appear - without it though, you can click fast, and it breaks the updating for some reason.

			IsBusy = false;
		}

		BoxView createLine()
		{
			BoxView line = new BoxView();
			line.HeightRequest = 30;
			line.Color = Color.DarkSlateGray;
			line.WidthRequest = 1;
			line.SetValue(Grid.RowProperty, 0);
			return line;
		}
		BoxView createHorizontalLine()
		{
			BoxView line = new BoxView();
			line.HeightRequest = 1;
			line.Color = Color.DarkSlateGray;
			line.WidthRequest = 5;
			line.VerticalOptions = LayoutOptions.Center;
			line.HorizontalOptions = LayoutOptions.Start;
			return line;
		}
		/// <summary>
		/// Creates special rectangle
		/// </summary>
		/// <returns></returns>
		BoxView createRect()
		{
			BoxView rect = new BoxView
			{
				VerticalOptions = LayoutOptions.End,
				Margin = new Thickness(5, 0, 5, 0),
				WidthRequest = 30
			};
			rect.SetValue(Grid.RowProperty, 0);
			rect.BackgroundColor = Color.FromHex("#01ABC9");
			return rect;
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

			while (0 > DateTime.Compare(d1, d2))
			{
				string weekDay = d1.DayOfWeek.ToString();
				int weekNum = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

				Grid weekCategory = new Grid();
				RowDefinition wGridRow1 = new RowDefinition();
				RowDefinition wGridRow2 = new RowDefinition();

				wGridRow1.Height = new GridLength(30);
				weekCategory.RowDefinitions.Add(wGridRow1);
				weekCategory.RowDefinitions.Add(wGridRow2);
				//var myBorder = new Border();
				//myBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01ABC9"));
				//myBorder.Margin = new Thickness(2,0,2,0);
				//weekCategory.Children.Add(myBorder);

				BoxView line = createLine();

				Label textWeek = new Label();
				textWeek.Text = "Uge " + weekNum;
				textWeek.TextColor = Color.White;
				textWeek.VerticalOptions = LayoutOptions.End;
				textWeek.HorizontalOptions = LayoutOptions.Center;
				textWeek.VerticalOptions = LayoutOptions.Center;
				textWeek.SetValue(Grid.RowProperty, 0);

				Grid weekGrid = new Grid();
				weekGrid.SetValue(Grid.RowProperty, 1);
				weekGrid.VerticalOptions = LayoutOptions.End;
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
					gridRow1.Height= new GridLength(120);
					gridRow2.Height= new GridLength(30);

					combiningColGrid.RowDefinitions.Add(gridRow1);
					combiningColGrid.RowDefinitions.Add(gridRow2);

					weekDay = d1.DayOfWeek.ToString();
					weekNum = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

					Label textDay = new Label();
					textDay.Text = weekDay[0].ToString() + weekNum;
					textDay.TextColor = Color.Black;
					textDay.VerticalOptions = LayoutOptions.End;
					textDay.HorizontalOptions = LayoutOptions.Center;
					textDay.VerticalOptions = LayoutOptions.Center;
					textDay.SetValue(Grid.RowProperty, 1);

					float rectHeight = 0;

					BoxView rect = createRect();
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
					rect.HeightRequest = reelHeight;

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
				//var myBorder = new Border();
				//yBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01ABC9"));
				//monthCategory.Children.Add(myBorder);

				BoxView line = createLine();

				Label textMonth = new Label();
				textMonth.Text = d1.ToString("MMMM");
				textMonth.TextColor = Color.White;
				textMonth.VerticalOptions = LayoutOptions.End;
				textMonth.HorizontalOptions = LayoutOptions.Center;
				textMonth.VerticalOptions = LayoutOptions.Center;
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
				monthGrid.VerticalOptions = LayoutOptions.End;
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
					gridRow1.Height= new GridLength(120);
					gridRow2.Height= new GridLength(30);

					combiningColGrid.RowDefinitions.Add(gridRow1);
					combiningColGrid.RowDefinitions.Add(gridRow2);

					int ugeNummer = i + 1;

					Label textDay = new Label();
					textDay.Text = "W " + ugeNummer;
					textDay.TextColor = Color.Black;
					textDay.VerticalOptions = LayoutOptions.End;
					textDay.HorizontalOptions = LayoutOptions.Center;
					textDay.VerticalOptions = LayoutOptions.Center;
					textDay.SetValue(Grid.RowProperty, 1);

					float rectHeight = 0;

					BoxView rect = createRect();
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
					rect.HeightRequest = reelHeight;


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
				yearGrid.VerticalOptions = LayoutOptions.End;
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
				yGridRow1.Height= new GridLength(30);
				yearCategory.RowDefinitions.Add(yGridRow1);
				yearCategory.RowDefinitions.Add(yGridRow2);
				//var myBorder = new Border();
				//myBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#01ABC9"));
				//yearCategory.Children.Add(myBorder);

				BoxView line = createLine();

				Label textYear = new Label();
				textYear.Text = d1.ToString("yyyy");
				textYear.TextColor = Color.White;
				textYear.VerticalOptions = LayoutOptions.End;
				textYear.HorizontalOptions = LayoutOptions.Center;
				textYear.VerticalOptions = LayoutOptions.Center;
				textYear.SetValue(Grid.RowProperty, 0);

				Grid yearGrid = new Grid();
				yearGrid.VerticalOptions = LayoutOptions.End;
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
					gridRow1.Height= new GridLength(120);
					gridRow2.Height= new GridLength(30);

					combiningColGrid.RowDefinitions.Add(gridRow1);
					combiningColGrid.RowDefinitions.Add(gridRow2);

					Label textDay = new Label();
					textDay.Text = d1.ToString("MMM");
					textDay.TextColor = Color.Black;
					textDay.VerticalOptions = LayoutOptions.End;
					textDay.HorizontalOptions = LayoutOptions.Center;
					textDay.VerticalOptions = LayoutOptions.Center;
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

					BoxView rect = createRect();
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
					rect.HeightRequest = reelHeight;


					combiningColGrid.Children.Add(rect);
					combiningColGrid.Children.Add(textDay);

					combiningColGrid.SetValue(Grid.ColumnProperty, i);
					yearGrid.Children.Add(combiningColGrid);
					d1 = d1.AddDays(1);
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
			float maksAkseValue = topValue / 6;
			int akseValue = ((int)Math.Round(maksAkseValue / 10)) * 10;

			for (int i = 0; i < 6; i++)
			{
				int specAkseValue = akseValue * (6 - i);
				string AkseName = " " + specAkseValue;
				Label AkseNum = new Label();
				AkseNum.Text = AkseName;
				AkseNum.SetValue(Grid.RowProperty, 1 + i);
				AkseNum.Margin= new Thickness (10,0,0,0);
				AkseNum.VerticalOptions = LayoutOptions.Center;
				BoxView a = createHorizontalLine();
				a.VerticalOptions = LayoutOptions.Center;
				a.SetValue(Grid.RowProperty, 1 + i);

				ChartRow.Children.Add(AkseNum);
				ChartRow.Children.Add(a);

			}

		}

		void KalibrerGenmVDig(Dictionary<DateTime, float> historik, int Hustandstal)
		{
			DateTime date = historik.ElementAt(0).Key;

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

			if (ditGenm < DanskGenm)
			{
				DinUdledningRect.HeightRequest = ditGenm / DanskGenm * 180;

				DanskUdledningRect.HeightRequest = 180;
				DanskUdledningText.Margin = new Thickness(0);
				if (ditGenm / DanskGenm * 180 < 30)
				{
					DinUdledningText.Margin = new Thickness(0, 0, 0, ditGenm / DanskGenm * 180);
					DinUdledningText.VerticalOptions = LayoutOptions.End;
					DinUdledningText.TextColor = Color.Black;
				}
				else
				{
					DinUdledningText.Margin = new Thickness(0, 180 - ditGenm / DanskGenm * 180, 0, 0);
					DinUdledningText.VerticalOptions = LayoutOptions.Center;
					DinUdledningText.TextColor = Color.White;
				}
			}
			else if (ditGenm > DanskGenm)
			{
				DinUdledningRect.HeightRequest = 180;
				DinUdledningText.Margin = new Thickness(0);
				DanskUdledningRect.HeightRequest = DanskGenm / ditGenm * 180;
				if (DanskGenm / ditGenm * 180 < 30)
				{
					DanskUdledningText.Margin = new Thickness(0, 0, 0, DanskGenm / ditGenm * 180);
					DanskUdledningText.VerticalOptions = LayoutOptions.End;
					DanskUdledningText.TextColor = Color.Black;
				}
				else
				{
					DanskUdledningText.Margin = new Thickness(0, 180 - DanskGenm / ditGenm * 180, 0, 0);
					DanskUdledningText.VerticalOptions = LayoutOptions.Center;
					DanskUdledningText.TextColor = Color.White;
				}
			}
			else
			{
				DinUdledningRect.HeightRequest = 180;
				DinUdledningText.Margin = new Thickness(0);
				DinUdledningText.VerticalOptions = LayoutOptions.Center;
				DinUdledningText.TextColor = Color.White;
				DanskUdledningRect.HeightRequest = 180;
				DanskUdledningText.Margin = new Thickness(0);
				DanskUdledningText.VerticalOptions = LayoutOptions.Center;
				DanskUdledningText.TextColor = Color.White;
			}

		}

		private void Button_Click(object sender, EventArgs e)
		{
			Chartdata.Children.Clear();
			ChartRow.Children.Clear();
			MakeWeekBarGraph(boef);

			SelectedButtonLine.SetValue(Grid.ColumnProperty, 0);
			SelectedButtonRect.SetValue(Grid.ColumnProperty, 0);
			ButtonName0.FontAttributes = FontAttributes.Bold;
			ButtonName1.FontAttributes = FontAttributes.None;
			ButtonName2.FontAttributes = FontAttributes.None;
			ButtonName0.TextColor = Color.White;
			ButtonName1.TextColor = Color.Black;
			ButtonName2.TextColor = Color.Black;

		}

		private void Button_Click_1(object sender, EventArgs e)
		{
			Chartdata.Children.Clear();
			ChartRow.Children.Clear();
			MakeMonthBarGraph(boef);

			SelectedButtonLine.SetValue(Grid.ColumnProperty, 1);
			SelectedButtonRect.SetValue(Grid.ColumnProperty, 1);
			ButtonName0.FontAttributes = FontAttributes.None;
			ButtonName1.FontAttributes = FontAttributes.Bold;
			ButtonName2.FontAttributes = FontAttributes.None;
			ButtonName0.TextColor = Color.Black;
			ButtonName1.TextColor = Color.White;
			ButtonName2.TextColor = Color.Black;
		}

		private void Button_Click_2(object sender, EventArgs e)
		{
			Chartdata.Children.Clear();
			ChartRow.Children.Clear();
			MakeYearBarGraph(boef);

			SelectedButtonLine.SetValue(Grid.ColumnProperty, 2);
			SelectedButtonRect.SetValue(Grid.ColumnProperty, 2);
			ButtonName0.FontAttributes = FontAttributes.None;
			ButtonName1.FontAttributes = FontAttributes.None;
			ButtonName2.FontAttributes = FontAttributes.Bold;
			ButtonName0.TextColor = Color.Black;
			ButtonName1.TextColor = Color.Black;
			ButtonName2.TextColor = Color.White;

			//Bare for at teste det
			KalibrerGenmVDig(boef, 1);
		}
	}
}