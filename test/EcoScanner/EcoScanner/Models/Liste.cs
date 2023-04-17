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
using EcoScanner.ViewModels;

namespace EcoScanner.Models
{
	public static class Liste
	{
		static string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Liste";
		static string filePath = path + "/Products";
		public static void createFolder()
		{
			Directory.CreateDirectory(path);
		}
		public static void writeText(string fileName, string text)
		{
			string filePath = path + "/" + fileName;
			if (!File.Exists(filePath))
			{
				File.WriteAllText(filePath, text);
			}
			else //file exists
			{
				//read file, then add to end of file.
				string modtext = readText() + text;
				File.WriteAllText(filePath, modtext);
			}
		}
		public static void saveProduct(Product product)
		{
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
			ListeViewModel.invoke();


		}
		public static List<Product> getProducts()
		{
			if (!File.Exists(filePath))
			{
				return new List<Product>();
			}
			else //file exists
			{
				//return products
				List<Product> products = JsonSerializer.Deserialize<List<Product>>(File.ReadAllText(filePath));
				return products;
			}
		}

		public static float getSum()
		{
			if (!File.Exists(filePath))
			{
				return 0;
			}
			else //file exists
			{
				//return products
				List<Product> products = JsonSerializer.Deserialize<List<Product>>(File.ReadAllText(filePath));
				return products.Sum(item => item.CO2);
			}
		}
		public static string readText()
		{
			if (File.Exists(filePath))
			{
				return File.ReadAllText(filePath);
			}
			return "No data yet";
		}

		public static void clearFile()
		{
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
				Trace.WriteLine("---- Removed file -----");
			}
			else
			{
				Trace.WriteLine("--- No file to remove ---");
			}
				
		}


	}
}
