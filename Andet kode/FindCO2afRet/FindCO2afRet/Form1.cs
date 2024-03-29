using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using FireSharp;
using System.Collections.Generic;
using System.Globalization;

namespace FindCO2afRet
{
	public partial class Form1 : Form
	{
		float samletUdledning = 0;
		public Form1()
		{
			InitializeComponent();
		}

		IFirebaseConfig ifc = new FirebaseConfig()
		{
			AuthSecret = "TlXCyZsxp3gJw2IXUP3266N6xvk95GWsUMXZLzdh",
			BasePath = "https://foedevareklimabelastning-default-rtdb.europe-west1.firebasedatabase.app/"
		};

		IFirebaseClient client;
		string[] vareData;
		string[] amountData;
		string navn;
		int Index = 0;

		Dictionary<string, Vare> data;

		private async void Form1_Load(object sender, EventArgs e)
		{
			await Connect(); 
		}

		private async void button1_Click(object sender, EventArgs e)
		{
			Index = Int32.Parse(textBox1.Text);
			await Connect();
			
			listBox1.Items.Clear();
			listBox2.Items.Clear();

			for (int i = 0; i < vareData.Length; i++)
			{
				matematik(i);
			}
			label1.Text = samletUdledning.ToString();
			label2.Text = navn;
		}

		public async Task Connect()
		{
			try
			{
				client = new FirebaseClient(ifc);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Problem der er");
			}

			FirebaseResponse responseVare = await client.GetAsync("Retter/" + Index + "/CO2/Vare");
			vareData = responseVare.ResultAs<string[]>();

			FirebaseResponse responseAmount = await client.GetAsync("Retter/" + Index + "/CO2/Amount");
			amountData = responseAmount.ResultAs<string[]>();

			FirebaseResponse response = await client.GetAsync("Madvare");
			data = response.ResultAs<Dictionary<string, Vare>>();

			FirebaseResponse responseNavn = await client.GetAsync("Retter/" + Index + "/Name");
			navn = responseNavn.ResultAs<string>();

			foreach (var element in data)
			{
				if (element.Value == null)
				{
					data.Remove(element.Key);
				}
			}
		}

		private void matematik(int i)
		{
			float vareUdledning;
			string BennysBukserBraendte = amountData[i].ToString();
			BennysBukserBraendte = BennysBukserBraendte.Replace(".",",");
			float amount = float.Parse(BennysBukserBraendte);
			
			foreach (var element in data)
			{

				if (element.Value.Name == vareData[i])
				{
					vareUdledning = element.Value.CO2;
					float Svend = vareUdledning * amount;
					samletUdledning += Svend;

					// Noget mindre relevant
					listBox1.Items.Add(element.Value.Name);
					listBox2.Items.Add(Svend);

				}
			}
		}
	}
}