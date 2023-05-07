using EcoScanner.Models;
using EcoScanner.Services;
using EcoScanner.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using static Xamarin.Forms.Internals.Profile;

namespace EcoScanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HistoryView : ContentPage
	{
		public int houseMemberCount { get; set; }
		Dictionary<DateTime, List<Product>> boef = new Dictionary<DateTime, List<Product>>();
		public static event EventHandler RefreshEventhandler;
		DateTime startDate = new DateTime(2023, 03, 20);

		public HistoryView()
		{
			BindingContext = new HistoryViewModel();
			boef = History.getHistory();
			RefreshEventhandler += (sender, e) => localRefresh();

			InitializeComponent();
			MakeWeekBarGraph(boef);
			Plus_Clicked(null, EventArgs.Empty);
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
			KalibrerGenmVDig(boef, int.Parse(EntryNumber.Text));
			Button_Click(null, EventArgs.Empty);
			OnPropertyChanged(null);
			IsBusy = true; //this causes the refresh circle to appear - without it though, you can click fast, and it breaks the updating for some reason.

			IsBusy = false;
		}
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			scroll(0);
		}
		static int GetWeekNumberOfMonth(DateTime date)
		{
			date = date.Date;
			DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
			DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
			if (firstMonthMonday > date)
			{
				firstMonthDay = firstMonthDay.AddMonths(-1);
				firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
			}
			return (date - firstMonthMonday).Days / 7 + 1;
		}
		/// <summary>
		/// scrolls the most right until the current date
		/// Takes int as input, where 0 is day, 1 is week, and 2 is year.
		/// </summary>
		async void scroll(int a)
		{
			//width of rectangles in graph, including right margin
			int boxwidth = 30 + 5;

			//calculate scrolllength
			int extraColumns = 0;
			DateTime date = DateTime.Now.AddDays(-1);


			if (a == 0)
			{
				extraColumns = 6 - (int)date.DayOfWeek;
			}
			else if(a == 1)
			{
				extraColumns = 3 - GetWeekNumberOfMonth(DateTime.Today);
			}
			else
			{
				extraColumns = 11 - DateTime.Now.Month;
			}

			await Task.Delay(100);
			double scrollLength = scroller.ContentSize.Width - boxwidth * extraColumns - (graphGrid.Width - 75);

			await scroller.ScrollToAsync(scrollLength, 0, true);
		}

		BoxView createLine()
		{
			BoxView line = new BoxView();
			line.HeightRequest = 30;
			line.Color = Color.DarkSlateGray;
			line.WidthRequest = 1;
			line.SetValue(Grid.RowProperty, 0);
			line.HorizontalOptions = LayoutOptions.End;
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
		BoxView createBlueBackground()
		{
			BoxView line = new BoxView();
			line.HeightRequest = 30;
			line.Color = Color.FromHex("#01ABC9");
			line.SetValue(Grid.RowProperty, 0);
			return line;
		}
		/// <summary>
		/// Creates special rectangle
		/// </summary>
		/// <returns></returns>
		BoxView createRect(DateTime startdate, DateTime endDate)
		{
			BoxView rect = new BoxView
			{
				VerticalOptions = LayoutOptions.End,
				Margin = new Thickness(5, 0, 5, 0),
				WidthRequest = 30
			};
			rect.HorizontalOptions = LayoutOptions.Center;
			rect.SetValue(Grid.RowProperty, 0);
			rect.BackgroundColor = Color.FromHex("#01ABC9");
			// Add a TapGestureRecognizer to the BoxView
			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += async (s, e) => {
				await rectClickedAsync(startdate, endDate);
			};
			rect.GestureRecognizers.Add(tapGestureRecognizer);

			return rect;
		}

		async Task rectClickedAsync(DateTime startdate, DateTime endDate)
		{
			if(!WarningPopupView.onPopup)
			{
				WarningPopupView.onPopup = true;
				List<Product> products = History.GetProductsInInterval(startdate, endDate);
				WarningPopupViewModel viewmodel = new WarningPopupViewModel(
					"I denne tidsperiode har du købt følgende:",
					new SingleButtonView(new StandardTwoButtonViewModel(
						async () => await ButtonCommands.ClosePopupAsync(), async () => await ButtonCommands.ClosePopupAsync(),
						"TilbageKnap.png", "")),
					new ListOfItemsView(new ListOfItemsViewModel(products))
					);
				await PopupNavigation.Instance.PushAsync(new WarningPopupView(viewmodel));
			}
		}
		async void MakeWeekBarGraph(Dictionary<DateTime, List<Product>> historik)
		{
			DateTime d1 = new DateTime(2023, 3, 20);
			DateTime d2 = DateTime.Now;

			List<float> testForBest = new List<float>();
			//figuring out which date has most emission
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
						dayValue = historik[d1.Date].Sum(p => p.TotCo2);
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
				int graphheight = 140;
				int margin = 15;
				int buttomHeight = 40;

				Grid weekCategory = new Grid();
				RowDefinition wGridRow1 = new RowDefinition();
				RowDefinition wGridRow2 = new RowDefinition();

				wGridRow1.Height = new GridLength(30);
				wGridRow2.Height = new GridLength(graphheight + buttomHeight + margin);
				weekCategory.RowDefinitions.Add(wGridRow1);
				weekCategory.RowDefinitions.Add(wGridRow2);


				BoxView line = createLine();
				BoxView boxbehind = createBlueBackground();

				Label textWeek = new Label();
				textWeek.Text = "Uge " + weekNum;
				textWeek.TextColor = Color.White;
				textWeek.VerticalOptions = LayoutOptions.End;
				textWeek.HorizontalOptions = LayoutOptions.Center;
				textWeek.VerticalOptions = LayoutOptions.Center;
				textWeek.SetValue(Grid.RowProperty, 0);

				Grid weekGrid = new Grid();
				weekGrid.SetValue(Grid.RowProperty, 1);
				weekGrid.VerticalOptions = LayoutOptions.Start;
				weekGrid.Margin = new Thickness(0, margin, 0 ,0);
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
					gridRow1.Height= new GridLength(graphheight);
					gridRow2.Height= new GridLength(buttomHeight);
					combiningColGrid.RowDefinitions.Add(gridRow1);
					combiningColGrid.RowDefinitions.Add(gridRow2);

					weekDay = d1.DayOfWeek.ToString();
					weekNum = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d1, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

					Label textDay = new Label();
					textDay.Text = d1.ToString("ddd")+ "\n"+d1.ToString("dd/MM");
					if(d1 == DateTime.Today)
					{
						textDay.FontAttributes = FontAttributes.Bold;
					}
					textDay.HorizontalTextAlignment = TextAlignment.Center;
					textDay.TextColor = Color.Black;
					textDay.HorizontalOptions = LayoutOptions.Center;
					textDay.VerticalOptions = LayoutOptions.Center;
					textDay.SetValue(Grid.RowProperty, 1);

					float rectHeight = 0;

					BoxView rect = createRect(d1, d1);
					if (historik.ContainsKey(d1.Date))
					{
						rectHeight = historik[d1.Date].Sum(p => p.TotCo2);
					}
					float maksAkseValue = ((int)Math.Ceiling(best / 60)) * 60;
					float reelHeight = graphheight * rectHeight / maksAkseValue;
					if (maksAkseValue == 0)
					{
						reelHeight = 0;
					}
					rect.HeightRequest = reelHeight;

					//Label emission = createText(reelHeight, graphheight, rectHeight);
					combiningColGrid.Children.Add(rect);
					/*if (emission != null)
					{
						combiningColGrid.Children.Add(emission);
					}*/
					combiningColGrid.Children.Add(textDay);

					combiningColGrid.SetValue(Grid.ColumnProperty, i);
					weekGrid.Children.Add(combiningColGrid);

					d1 = d1.AddDays(1);
				}

				weekCategory.Children.Add(boxbehind);
				weekCategory.Children.Add(line);
				weekCategory.Children.Add(textWeek);
				weekCategory.Children.Add(weekGrid);
				Chartdata.Children.Add(weekCategory);

			}

			JusterAkse(best);
		}

		void MakeMonthBarGraph(Dictionary<DateTime, List<Product>> historik)
		{
			DateTime d1 = new DateTime(2023, 3, 6);
			DateTime d2 = DateTime.Now;
			int graphHeight = 140;
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
							weekValue += historik[d1].Sum(p => p.TotCo2);
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
				int margin = 15;
				Grid monthCategory = new Grid();
				RowDefinition mGridRow1 = new RowDefinition();
				RowDefinition mGridRow2 = new RowDefinition();
				mGridRow1.Height = new GridLength(30);
				mGridRow2.Height = new GridLength(graphHeight + 30 + margin); //30 for week names at buttom, and 10 for margin

				monthCategory.RowDefinitions.Add(mGridRow1);
				monthCategory.RowDefinitions.Add(mGridRow2);

				BoxView line = createLine();
				BoxView boxbehind = createBlueBackground();

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
				monthGrid.Margin = new Thickness(0, margin, 0, 0);

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
					gridRow1.Height= new GridLength(graphHeight);
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

					BoxView rect = createRect(d1, d1.AddDays(7));
					for (int j = 0; j < 7; j++)
					{
						rect.SetValue(Grid.RowProperty, 0);
						if (historik.ContainsKey(d1.Date))
						{
							rectHeight += historik[d1].Sum(p => p.TotCo2);
						}
						if (d1 == DateTime.Today)
						{
							textDay.FontAttributes = FontAttributes.Bold;
						}
						d1 = d1.AddDays(1);
					}
					float maksAkseValue = ((int)Math.Ceiling(best / 60)) * 60;
					float reelHeight = graphHeight * rectHeight / maksAkseValue;
					if (maksAkseValue == 0)
					{
						reelHeight = 0;
					}
					rect.HeightRequest = reelHeight;

					// emission = createText(reelHeight, graphHeight, rectHeight);
					combiningColGrid.Children.Add(rect);
					/*if (emission != null)
					{
						combiningColGrid.Children.Add(emission);
					}*/
					combiningColGrid.Children.Add(textDay);

					combiningColGrid.SetValue(Grid.ColumnProperty, i);
					monthGrid.Children.Add(combiningColGrid);
				}
				monthCategory.Children.Add(boxbehind);
				monthCategory.Children.Add(line);
				monthCategory.Children.Add(textMonth);
				monthCategory.Children.Add(monthGrid);
				Chartdata.Children.Add(monthCategory);

			}

			JusterAkse(best);

		}

		void MakeYearBarGraph(Dictionary<DateTime, List<Product>> historik)
		{
			int graphHeight = 140;
			DateTime startdate = new DateTime(2023, 1, 1);
			DateTime d1 = startdate;
			DateTime d2 = DateTime.Now;

			List<float> testForBest = new List<float>();

			while (0 > DateTime.Compare(d1, d2))
			{
				Grid yearGrid = new Grid();
				yearGrid.VerticalOptions = LayoutOptions.End;
				yearGrid.SetValue(Grid.RowProperty, 1);
				//create columns for all months
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
							monthValue += historik[d1].Sum(p => p.TotCo2);
						}
						d1 = d1.AddDays(1);
					}

					testForBest.Add(monthValue);
				}

			}

			d1 = startdate;
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
				yGridRow2.Height = new GridLength(graphHeight + 30 + 15);

				yearCategory.RowDefinitions.Add(yGridRow1);
				yearCategory.RowDefinitions.Add(yGridRow2);


				BoxView line = createLine();
				BoxView boxbehind = createBlueBackground();

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
				yearGrid.Margin = new Thickness(0, 15, 0, 0);

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
					gridRow1.Height= new GridLength(graphHeight);
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

					BoxView rect = createRect(d1, d1.AddDays(daysInMonth));
					for (int j = 0; j < daysInMonth; j++)
					{
						rect.SetValue(Grid.RowProperty, 0);
						if (historik.ContainsKey(d1.Date))
						{
							rectHeight += historik[d1].Sum(p => p.TotCo2);
						}
						if (d1 == DateTime.Today)
						{
							textDay.FontAttributes = FontAttributes.Bold;
						}
						d1 = d1.AddDays(1);
					}
					float maksAkseValue = ((int)Math.Ceiling(best / 60)) * 60;
					float reelHeight = graphHeight * rectHeight / maksAkseValue;
					if (maksAkseValue == 0)
					{
						reelHeight = 0;
					}
					rect.HeightRequest = reelHeight;
					//Label emission = createText(reelHeight, graphHeight, rectHeight);
					combiningColGrid.Children.Add(rect);
					/*if(emission != null)
					{
						combiningColGrid.Children.Add(emission);
					}*/
					combiningColGrid.Children.Add(textDay);

					combiningColGrid.SetValue(Grid.ColumnProperty, i);
					yearGrid.Children.Add(combiningColGrid);
					d1 = d1.AddDays(1);
				}

				yearCategory.Children.Add(boxbehind);
				yearCategory.Children.Add(line);
				yearCategory.Children.Add(textYear);
				yearCategory.Children.Add(yearGrid);
				Chartdata.Children.Add(yearCategory);
			}
			JusterAkse(best);
		}

		void JusterAkse(float topValue)
		{
			float maksAkseValue = topValue / 6;
			int akseValue = ((int)Math.Ceiling(maksAkseValue / 10)) * 10;

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

		void KalibrerGenmVDig(Dictionary<DateTime, List<Product>> historik, int Hustandstal)
		{
			if (historik.Count == 0)
			{
				return;
			}
			DateTime date = DateTime.Now; //find earliest entry
			foreach (var a in historik)
			{
				if (DateTime.Compare(a.Key, date) < 0)
				{
					date = a.Key;
				}
			}

			double DanskGenm = 57.5;
			float SumCO2 = 0;
			//Go through all days, and find the sum of the co2 emitted.
			while (0 > DateTime.Compare(date, DateTime.Now))
			{
				if (historik.ContainsKey(date.Date)) 
				{
					SumCO2 += historik[date].Sum(p => p.TotCo2);
				}

				date = date.AddDays(1);
			}

			double weeksSinceStart = Math.Ceiling((date - DateTime.Today).TotalDays / 7);
			double ditGenm = SumCO2  / (weeksSinceStart * Hustandstal);
			DinUdledningText.Text = ditGenm.ToString("0.00") + " kg";

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
			else //if you are exactly at average
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
		/// <summary>
		/// creates a text, with a margin, depending on the height of a box, that is given as parameter
		/// </summary>
		Label createText(float currentHeight, float graphheight, float emission)
		{
			if (currentHeight == 0) 
			{
				return null;
			}
			Label label = new Label();
			float ythickness;
			if (currentHeight > 30) //if the box is high enough
			{
				ythickness = graphheight - currentHeight;
				label.VerticalOptions = LayoutOptions.Center;
				label.TextColor = Color.White;
			}
			else // if its not
			{
				ythickness = currentHeight + 10;
				label.VerticalOptions = LayoutOptions.End;
				label.TextColor = Color.Black;
			}
			label.WidthRequest = 30;
			if (emission < 1000)
			{
				label.Text = emission.ToString("0.0");
			}
			else
			{
				label.Text = emission.ToString("0");
			}

			//manual centering...
			if (emission < 100 && emission > 0)
			{
				if (emission < 10)
				{
					label.Margin = new Thickness(13, ythickness, 0, 0);

				}
				else
				{
					label.Margin = new Thickness(9, ythickness, 0, 0);
				}
			}
			else
			{
				label.Margin = new Thickness(5, ythickness, 0, 0);

			}

			label.FontSize = 11;
			label.LineBreakMode = LineBreakMode.WordWrap;
			label.HorizontalOptions = LayoutOptions.Start;

			return label;
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
			scroll(0);

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
			scroll(1);

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
			scroll(2);


		}
		public void Plus_Clicked(object sender, EventArgs e)
		{
			int number = int.Parse(EntryNumber.Text) + 1;
			EntryNumber.Text = number.ToString();
			boef = History.getHistory();
			KalibrerGenmVDig(boef, number);
		}

		private void Minus_Clicked(object sender, EventArgs e)
		{
			int number = int.Parse(EntryNumber.Text) - 1;
			if (number > 0)
			{
				boef = History.getHistory();
				EntryNumber.Text = number.ToString();
				KalibrerGenmVDig(boef, number);
			}
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			History.cheatadd();
        }
    }
}