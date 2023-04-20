using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
					if (matchingProducts.ElementAt(i).Name.IndexOf(item + text) > -1)
					{
						suffix = true;
					}
					if (matchingProducts.ElementAt(i).Name.IndexOf(text + item) > -1)
					{
						prefix = true;
					}

					if (matchingProducts.ElementAt(i).Name.LastIndexOf(item + text) > -1)
					{
						suffix = true;
					}
					if (matchingProducts.ElementAt(i).Name.LastIndexOf(text + item) > -1)
					{
						prefix = true;
					}
				}

				if (prefix && suffix)
				{
					sortedMatchingProducts.Add(matchingProducts.ElementAt(i));
				}
			}

			//sort these products
			sortedMatchingProducts = sortAfterIndex(sortedMatchingProducts, text, 0);



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
					if (matchingProducts.ElementAt(i).Name.IndexOf(item + text) > -1)
					{
						suffix = true;
					}
					if (matchingProducts.ElementAt(i).Name.IndexOf(text + item) > -1)
					{
						prefix = true;
					}

					if (matchingProducts.ElementAt(i).Name.LastIndexOf(item + text) > -1)
					{
						suffix = true;
					}
					if (matchingProducts.ElementAt(i).Name.LastIndexOf(text + item) > -1)
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
			sortAfterIndex(sortedMatchingProducts, text, alreadySorted);

			//add the rest, that contains the word
			for (int i = 0; i < matchingProducts.Count; i++)
			{
				if (!sortedMatchingProducts.Contains(matchingProducts.ElementAt(i)))
				{
					sortedMatchingProducts.Add(matchingProducts.ElementAt(i));
				}
			}

			return matchingProducts;
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
						int index1 = word1.Name.IndexOf(text);
						int index2 = word2.Name.IndexOf(text);
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
	}
}
