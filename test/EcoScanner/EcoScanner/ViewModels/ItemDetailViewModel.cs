using EcoScanner.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EcoScanner.ViewModels
{
    [QueryProperty(nameof(ID), nameof(ID))]
    public class ItemDetailViewModel : BaseViewModel
    {
		public int ID  { get; set;}
		public string Name { get; set; }
		public float CO2 { get; set; }




        public void LoadItemId(int itemId)
        {
            try
            {
                var item = Databasehandler.GetProduct(itemId);
                Name = item.Name;
                CO2 = item.CO2;
                ID = item.ID;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
