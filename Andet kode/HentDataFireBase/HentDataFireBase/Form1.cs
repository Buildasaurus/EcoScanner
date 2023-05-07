using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json.Linq;

namespace HentDataFireBase
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

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

        private void button1_Click(object sender, EventArgs e)
        {
            int number;
            bool success = int.TryParse(textBox1.Text, out number);
            if (success)
            {
                var result = client.Get("Madvare/" + textBox1.Text);
                Vare vr = result.ResultAs<Vare>();
                label1.Text = vr.Name;
                label2.Text = vr.CO2.ToString();
            }
            else
            {
                MessageBox.Show("Nope");
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
