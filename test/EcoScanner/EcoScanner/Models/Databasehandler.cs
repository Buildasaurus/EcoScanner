using System;
using System.Collections.Generic;
using System.Text;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace EcoScanner.Models
{
    public class Databasehandler
    {
		IFirebaseConfig ifc = new FirebaseConfig()
		{
			AuthSecret = "TlXCyZsxp3gJw2IXUP3266N6xvk95GWsUMXZLzdh",
			BasePath = "https://foedevareklimabelastning-default-rtdb.europe-west1.firebasedatabase.app/"
		};

		IFirebaseClient client;

		public Databasehandler() { }
		Product GetProduct(int number)
		{
			var result = client.Get("Madvare/" + number);
			Product product = result.ResultAs<Product>();
			return product;
		}
	}
}
