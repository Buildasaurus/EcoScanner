using EcoScanner.Models;
using EcoScanner.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace EcoScanner.Services
{
    public static class History
    {
        public static Dictionary<DateTime, List<Product>> historyData = new Dictionary<DateTime, List<Product>>();
        static string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Liste";
        static string filePath = path + "/History";
        public static Dictionary<DateTime, List<Product>> getHistory()
        {
            //read file
            setup();
			Dictionary<DateTime, List<Product>> newdata = JsonSerializer.Deserialize<Dictionary<DateTime, List<Product>>> (File.ReadAllText(filePath));
            historyData = newdata;
            return historyData;
        }
        /// <summary>
        /// Constructor - makes sure file always exists - Mandatory to call
        /// </summary>
        public static void setup()
        {
            Directory.CreateDirectory(path);
            try
            {
				if (!File.Exists(filePath))
				{
					historyData = new Dictionary<DateTime, List<Product>>();
					string json = JsonSerializer.Serialize(historyData);
					File.WriteAllText(filePath, json);
				}
			}
            catch (Exception e)//if it couldn't succesfully read the data, it just removed it all.
            {
				clearHistory();
            }
            
        }
        public static void addToHistory()
        {
			List<Product> products = Liste.getProducts();
			if (products.Count == 0)
			{
				return;
			}
			string a = File.ReadAllText(filePath);

			//read file, then add to end of file.
			Dictionary<DateTime, List<Product>> oldData = JsonSerializer.Deserialize<Dictionary<DateTime, List<Product>>>(File.ReadAllText(filePath));

			// Create a new dictionary to store the merged data
			Dictionary<DateTime, List<Product>> mergedData = new Dictionary<DateTime, List<Product>>(oldData);

			// Get the current date
			DateTime currentDate = DateTime.Today;

			// Check if the oldData dictionary contains the current date as a key
			if (oldData.ContainsKey(currentDate))
			{
				// Merge the products into the value of the oldData dictionary where the key is the current date
				mergedData[currentDate] = oldData[currentDate]
					.Concat(products)
					.GroupBy(p => p.Name)
					.Select(g =>
					{
						var firstProduct = g.First();
						firstProduct.Count = g.Sum(p => p.Count);
						return firstProduct;
					})
					.ToList();
			}
			else
			{
				// If the oldData dictionary does not contain the current date as a key, add it with the products list as its value
				mergedData.Add(currentDate, products);
			}

			/*if (oldData.ContainsKey(DateTime.Today))
            {
                oldData[DateTime.Today] += sum;
            }
            else
            {
                oldData.Add(DateTime.Today, sum);
            }*/
			historyData = mergedData;
            string json = JsonSerializer.Serialize(mergedData);
            File.WriteAllText(filePath, json);
        }
        public static void clearHistory()
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
        public static void cheatadd()
        {
            setup();
            float sum = 10;
            if (sum == 0)
            {
                return;
            }

            //read file, then add to end of file.
            Dictionary<DateTime, List<Product>> oldData = JsonSerializer.Deserialize<Dictionary<DateTime, List<Product>>>(File.ReadAllText(filePath));
            if (oldData.ContainsKey(new DateTime(2023, 4, 27)))
            {
            }
            else
            {
                oldData.Add(new DateTime(2023, 4, 27), new List<Product>{ new Product(2, "hej", 2.3f, 2.1f, "kg"), new Product(2, "hej", 2.3f, 2.1f, "kg") });
            }
            historyData = oldData;
            string json = JsonSerializer.Serialize(oldData);
            File.WriteAllText(filePath, json);
        }
    }
}
