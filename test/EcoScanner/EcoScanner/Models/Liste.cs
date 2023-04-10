using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms.PlatformConfiguration;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Net.NetworkInformation;
using Xamarin.Forms;

namespace EcoScanner.Models
{
	internal class Liste
	{
		string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Liste";
		public Liste()
		{
			Directory.CreateDirectory(path);
			string filename = "myFile";
			writeText(filename, "Hello World");
			Trace.WriteLine(readText(filename));
		}
		public void writeText(string fileName, string text)
		{
			string filePath = path + "/" + fileName;
			if (!File.Exists(filePath))
			{
				File.WriteAllText(filePath, text);
			}
			else //file exists
			{
				//read file, then add to end of file.
				string modtext = readText(fileName) + text;
				File.WriteAllText(filePath, modtext);
			}
		}
		public void saveProduct(Product product, string fileName)
		{
			string filePath = path + "/" + fileName;
			if (!File.Exists(filePath))
			{
				List<Product> products = new List<Product>();
				products.Add(product);
				string json = JsonSerializer.Serialize(products);
				File.WriteAllText(filePath, json);
			}
			else //file exists
			{
				//read file, then add to end of file.
				List<Product> products = JsonSerializer.Deserialize<List<Product>>(File.ReadAllText(filePath));
				products.Add(product);
				string json = JsonSerializer.Serialize(products);
				File.WriteAllText(filePath, json);
			}
			
		}
		public string readText(string fileName)
		{
			string filePath = path + "/" + fileName; 
			if (File.Exists(filePath))
			{
				return File.ReadAllText(filePath);
			}
			return "Error, no file with such name";
		}


	}
}
