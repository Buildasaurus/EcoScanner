using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using FireSharp;
using Syncfusion.Windows.Forms.Tools;

namespace LavRet
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		IFirebaseConfig ifc = new FirebaseConfig()
		{
			AuthSecret = "TlXCyZsxp3gJw2IXUP3266N6xvk95GWsUMXZLzdh",
			BasePath = "https://foedevareklimabelastning-default-rtdb.europe-west1.firebasedatabase.app/"
		};

		private void Form1_Load(object sender, EventArgs e)
		{
			try
			{
				client = new FirebaseClient(ifc);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Problem der er");
			}
		}

	IFirebaseClient client;

		private void button1_Click(object sender, EventArgs e)
		{
			List<string> Alist = new List<string>();
			foreach (var bob in AmountList.Items)
			{
				Alist.Add(bob.ToString());
			}
			List<string> Vlist = new List<string>();
			foreach (var bob in VareList.Items)
			{
				Vlist.Add(bob.ToString());
			}

			co2 CO = new co2()
			{
				Amount = Alist,
				Vare = Vlist
			};

			Retter retter = new Retter()
			{
				ID = Int32.Parse(IDtext.Text),
				Name = NavnText.Text,
				CO2 = CO

			};

			var setter = client.Set("Retter/" + IDtext.Text, retter);
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{

		}

		private void IDtext_TextChanged(object sender, EventArgs e)
		{

		}
	}
}