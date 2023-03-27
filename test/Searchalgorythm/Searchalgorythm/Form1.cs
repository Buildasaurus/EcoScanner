using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using FireSharp;
using System.Text.Json;

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
            string searchRequest = textBox1.Text;
            
            listBox1.Items.Clear();
            foreach (var element in data)
            {
                
                if (element.Value.Name.ToLower().Contains(searchRequest.ToLower()))
                {
                    listBox1.Items.Add(element.Value.Name);
                }
            }
           if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("Der var intet der matchede din søgning:(");
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}