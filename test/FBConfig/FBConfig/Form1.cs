using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using FireSharp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Security.Cryptography.Xml;
using IronXL;

namespace FBConfig
{
    public partial class Form1 : Form
    {
        int vareID = 0;
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
            WorkBook book = WorkBook.Load("C:\\Users\\Jonat\\Documents\\HCØ\\3.G\\Teknik - DDU\\Eksamen\\Results_FINAL_20210201v4.xlsx");
            WorkSheet sheet = book.GetWorkSheet("Ra_500food");
            for (int i = 1; i < 501; i++)
            {
                string name = sheet.GetCellAt(i,1).ToString();
                float co2 = float.Parse(sheet.GetCellAt(i, 12).ToString());

                Vare vr = new Vare()
                {
                    ID = vareID,
                    Name = name,
                    CO2 = co2,
                };
                var setter = client.Set("Madvare/" + vareID.ToString(), vr);
                vareID++;
            }
            
        }
    }
}