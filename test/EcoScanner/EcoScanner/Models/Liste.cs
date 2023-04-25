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
using System.Threading;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices.ComTypes;

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
		
		/// <summary>
		/// saves a product to the list. If the product already exists in the list, it simply adds to the count.
		/// </summary>
		/// <param name="product"></param>
		public static void saveProduct(Product product)
		{
			if (product.Count == 0)
			{
				return;
			}
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

				//search for product with identical name

				var index = products.FindIndex(x => x.Name == product.Name);
				if((index != -1)) //if exists, and isn't 0
				{
					//update the count of the product
					products[index].Count += product.Count;
					if (products[index].Count == 0)
					{
						products.RemoveAt(index);
					}
				}
				else if(index == -1 && product.Count > 0) //if doesn't exist, and has count more than 0.
				{
					products.Add(product);
				}
				string json = JsonSerializer.Serialize(products);
				File.WriteAllText(filePath, json);
			}
			ListeViewModel.invoke();


		}

		/// <summary>
		/// Takes the new product. Looks for product with equal name, and sets old product to new product.
		/// </summary>
		/// <param name="product"></param>
		public static void overrideProduct(Product product)
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

				var index = products.FindIndex(x => x.Name == product.Name);
				if ((index != -1) && products[index].Count > 0) //if exists, and isn't 0
				{
					products[index] = product;
				}
				else if (index == -1 && product.Count > 0) //if doesn't exist, and has count more than 0.
				{
					products.Add(product);
				}
				//search for product with identical name
				

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
		/// <summary>
		/// clears all the text in a file
		/// </summary>
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
