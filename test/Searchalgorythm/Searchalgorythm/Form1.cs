using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using FireSharp;
using System.Text.Json;
using System.Linq;

namespace Searchalgorythm
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

        IFirebaseClient client;

        public class Vare
        {
            public int ID;
            public string Name;
            public float CO2;

        }

        Dictionary<string, Vare> data;

        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FirebaseClient(ifc);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Problem der er");
            }

            FirebaseResponse response = await client.GetAsync("Madvare");
			data = response.ResultAs<Dictionary<string, Vare>>();
            foreach (var element in data)
            {
                if (element.Value == null)
                {
                    data.Remove(element.Key);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            sortering();

            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("Der var intet der matchede din søgning:(");
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void sortering()
        {
            List<string> list = new List<string>();
			listBox1.Items.Clear();

			foreach (var element in data)
			{

				if (element.Value.Name.ToLower().Contains(textBox1.Text.ToLower()))
				{
					list.Add(element.Value.Name);
				}
			}

			for (int i = 0; i < list.Count; i++)
			{


				bool suffix = false;
				bool prefix = false;

					string[] alfabet = { " ", ","};
					foreach (string item in alfabet)
					{
							if (list.ElementAt(i).IndexOf(item + textBox1.Text) > -1)
							{
								suffix = true;
							}
							if (list.ElementAt(i).IndexOf(textBox1.Text + item) > -1)
							{
								prefix = true;
							}

							if (list.ElementAt(i).LastIndexOf(item + textBox1.Text) > -1)
							{
								suffix = true;
							}
							if (list.ElementAt(i).LastIndexOf(textBox1.Text + item) > -1)
							{
								prefix = true;
							}
					}

					if (prefix && suffix)
					{
						listBox1.Items.Add(list.ElementAt(i));
					}
			}

			bool sorteret = false;

			if (listBox1.Items.Count > 1)
			{
				while (!sorteret)
				{
					bool done = true;

					for (int j = 0; j < listBox1.Items.Count - 1; j++)
					{

						string word1 = listBox1.Items[j].ToString();
						string word2 = listBox1.Items[j + 1].ToString();
						int index1 = word1.IndexOf(textBox1.Text);
						int index2 = word2.IndexOf(textBox1.Text);
						if (index1 > index2)
						{
							listBox1.Items.RemoveAt(j);
							listBox1.Items.RemoveAt(j);
							listBox1.Items.Insert(j, word2);
							listBox1.Items.Insert(j + 1, word1);
							done = false;
						}
						if (done)
						{
							sorteret = true;
						}
						else
						{
							sorteret = false;
						}
					}
				}
			}

			int dumtTal = listBox1.Items.Count;

			
			for (int i = 0; i < list.Count; i++)
			{
				bool suffix = false;
				bool prefix = false;

				string[] alfabet = { " ", ",", null};
				foreach (string item in alfabet)
				{
					string Vare = list.ElementAt(i).ToLower();
					if (Vare.IndexOf(item + textBox1.Text.ToLower()) > -1)
					{
						suffix = true;
					}
					if (Vare.IndexOf(item + textBox1.Text.ToLower()) > -1)
					{
						prefix = true;
					}

					if (Vare.LastIndexOf(item + textBox1.Text.ToLower()) > -1)
					{
						suffix = true;
					}
					if (Vare.LastIndexOf(item + textBox1.Text.ToLower()) > -1)
					{
						prefix = true;
					}
				}

				if ((prefix || suffix) && !listBox1.Items.Contains(list.ElementAt(i)))
				{
					listBox1.Items.Add(list.ElementAt(i));
				}
			}

			sorteret = false;

			if (listBox1.Items.Count > 1)
			{
				while (!sorteret)
				{
					bool done = true;

					for (int j = dumtTal; j < listBox1.Items.Count - 1; j++)
					{

						string word1 = listBox1.Items[j].ToString().ToLower();
						string word2 = listBox1.Items[j + 1].ToString().ToLower();
						int index1 = word1.IndexOf(textBox1.Text.ToLower());
						int index2 = word2.IndexOf(textBox1.Text.ToLower());
						if (index1 > index2)
						{
							listBox1.Items.RemoveAt(j);
							listBox1.Items.RemoveAt(j);
							listBox1.Items.Insert(j, word2);
							listBox1.Items.Insert(j + 1, word1);
							done = false;
						}
						if (done)
						{
							sorteret = true;
						}
						else
						{
							sorteret = false;
						}
					}
				}
			}

			for (int i = 0; i < list.Count; i++)
			{
				if (!listBox1.Items.Contains(list.ElementAt(i)))
				{
					listBox1.Items.Add(list.ElementAt(i));
				}
			}

		}
    }
}