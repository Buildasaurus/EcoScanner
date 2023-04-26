using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using static System.Net.Mime.MediaTypeNames;
namespace EcoScanner.Models
{
    public static class Databasehandler
    {
		static IFirebaseConfig ifc = new FirebaseConfig()
		{
			AuthSecret = "TlXCyZsxp3gJw2IXUP3266N6xvk95GWsUMXZLzdh",
			BasePath = "https://foedevareklimabelastning-default-rtdb.europe-west1.firebasedatabase.app/"
		};

		static IFirebaseClient client = new FirebaseClient(ifc);
		static bool allProductsLoaded = false;
		static Dictionary<string, Product> data;


		static public Product GetProduct(int number)
		{
			var result = client.Get("Madvare/" + number);
			Product product = result.ResultAs<Product>();
			product.Count = 1;
			return product;
		}

		public async static Task LoadAllProducts()
		{
		
			FirebaseResponse response = await client.GetAsync("Madvare");
			data = response.ResultAs<Dictionary<string, Product>>();
			//There are some data in the database that aren't products, that must be removed.
			var keysToRemove = new List<string>();

			foreach (var element in data)
			{
				if (element.Value == null)
				{
					keysToRemove.Add(element.Key);
				}
			}
			foreach (var key in keysToRemove)
			{
				data.Remove(key);
			}
			allProductsLoaded = true;
		}

		static public List<Product> Search(string text)
		{
			if (!allProductsLoaded) 
			{
				throw new NullReferenceException("CALL LOADALLPRODUCTS FIRST");
			}
			List<Product> matchingProducts = new List<Product>();
			List<Product> sortedMatchingProducts = new List<Product>();
			text = text.ToLower();

			//First find all elements that should be displayed (if they contain the searched word)
			foreach (var element in data)
			{
				Product product = element.Value;
				if (product.Name.ToLower().Contains(text.ToLower()))
				{
					matchingProducts.Add(product);
				}
			}

			//First, add the products where there is a " " and , before og after the word you are searching for. 
			//basically prioritizing the clear form of the word, where it is not inside another word.
			for (int i = 0; i < matchingProducts.Count; i++)
			{
				bool suffix = false;
				bool prefix = false;

				string[] alfabet = {" ", "," };
				foreach (string item in alfabet)
				{
					if (matchingProducts.ElementAt(i).Name.ToLower().IndexOf(item + text) > -1 || matchingProducts.ElementAt(i).Name.ToLower().StartsWith(text + item))
					{
						suffix = true;
					}
					if (matchingProducts.ElementAt(i).Name.ToLower().IndexOf(text + item) > -1 || matchingProducts.ElementAt(i).Name.ToLower().EndsWith(item + text))
					{
						prefix = true;
					}

					if (matchingProducts.ElementAt(i).Name.ToLower().LastIndexOf(item + text) > -1)
					{
						suffix = true;
					}
					if (matchingProducts.ElementAt(i).Name.ToLower().LastIndexOf(text + item) > -1)
					{
						prefix = true;
					}
				}

				if (prefix && suffix) //add all to new list, that has " " or "," before or after
				{
					sortedMatchingProducts.Add(matchingProducts.ElementAt(i));
				}
			}

			//sort these products
			sortedMatchingProducts = sortAfterIndex(sortedMatchingProducts, text.ToLower(), 0);



			//save how many we already have sorted for later use
			int alreadySorted = sortedMatchingProducts.Count;
			//now find all products where the word is in the start or end of another word.
			for (int i = 0; i < matchingProducts.Count; i++)
			{
				bool suffix = false;
				bool prefix = false;

				string[] alfabet = { " ", ",", null };
				foreach (string item in alfabet)
				{
					if (matchingProducts.ElementAt(i).Name.ToLower().IndexOf(item + text) > -1 || matchingProducts.ElementAt(i).Name.ToLower().StartsWith(text + item))
					{
						suffix = true;
					}
					if (matchingProducts.ElementAt(i).Name.ToLower().IndexOf(text + item) > -1 || matchingProducts.ElementAt(i).Name.ToLower().EndsWith(item + text))
					{
						prefix = true;
					}

					if (matchingProducts.ElementAt(i).Name.ToLower().LastIndexOf(item + text) > -1)
					{
						suffix = true;
					}
					if (matchingProducts.ElementAt(i).Name.ToLower().LastIndexOf(text + item) > -1)
					{
						prefix = true;
					}
				}

				if ((prefix || suffix) && !sortedMatchingProducts.Contains(matchingProducts.ElementAt(i)))
				{
					sortedMatchingProducts.Add(matchingProducts.ElementAt(i));
				}
			}

			// now sort the rest of the elements again.
			sortAfterIndex(sortedMatchingProducts, text.ToLower(), alreadySorted);

			//add the rest, that contains the word
			for (int i = 0; i < matchingProducts.Count; i++)
			{
				if (!sortedMatchingProducts.Contains(matchingProducts.ElementAt(i)))
				{
					sortedMatchingProducts.Add(matchingProducts.ElementAt(i));
				}
			}

			return sortedMatchingProducts;
		}

		/// <summary>
		/// // sort the sorted list, so that the earlier the word appears, the earlier it is in the sorted list.
		//eg. "skinke og ost" is sorted later than "ost og skinke", since "ost" appears earlier in 2nd string.
		/// </summary>
		/// <param name="list"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		private static List<Product> sortAfterIndex(List<Product> list, string text, int startindex)
		{
			bool sorteret = false;
			if (list.Count - startindex > 1) //requires that there is at least one word, to be sorted.
			{
				while (!sorteret)
				{
					bool done = true;

					for (int j = startindex; j < list.Count - 1; j++)
					{
						Product word1 = list[j];
						Product word2 = list[j + 1];
						int index1 = word1.Name.ToLower().IndexOf(text);
						int index2 = word2.Name.ToLower().IndexOf(text);
						if (index1 > index2) //if in wrong order
						{
							list.RemoveAt(j);
							list.RemoveAt(j);
							list.Insert(j, word2);
							list.Insert(j + 1, word1);
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
			return list;
		}

		//Handling of dishes
		static string[] vareData;
		static string[] amountData;
		static string navn;
		static int Index = 0;

		public static Dictionary<string, Dish> dishes;

		public static async Task loadDishes()
		{
			FirebaseResponse allretter = await client.GetAsync("Retter");
			dishes = allretter.ResultAs<Dictionary<string, Dish>>();
			if (data == null)
			{
				await LoadAllProducts();
			}

			List<string> keysToRemove = new List<string>();
			foreach (var element in dishes)
			{
				if (element.Value == null || element.Value.CO2 == null)
				{
					keysToRemove.Add(element.Key);
				}
				else
				{
					element.Value.TotalCo2 = CalculateWeight(element.Value);
				}
			}
			foreach (var key in keysToRemove)
			{
				dishes.Remove(key);
			}
		}
		/// <summary>
		/// Returns the total weight of a dish, with the dish as input.
		/// </summary>
		/// <param name="dish"></param>
		/// <returns></returns>
		public static float CalculateWeight(Dish dish)
		{
			int count = 0;
			float totalCo2 = 0;
			foreach(string ingredientweight in dish.CO2.Amount)
			{
				foreach (var element in data)
				{
					if (element.Value.Name == dish.CO2.Vare[count])
					{
						//Add the weight of the ingredient that is used timed by how much the ingredient emits.
						totalCo2 += element.Value.CO2 * float.Parse(dish.CO2.Amount[count], System.Globalization.CultureInfo.GetCultureInfo("en-GB"));
					}

				}
				count++;
			}
			return totalCo2;
		}
	}
}
