using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Runtime.CompilerServices;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class FruitController : Controller
    {
        public static List<FruitModel> fruitsList = new List<FruitModel>();

        public IActionResult Index()
        {
            Console.WriteLine("Count: " + fruitsList.Count);
            if (fruitsList.Count == 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "https://www.fruityvice.com/api/fruit/all";
                    var response = client.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        List<Dictionary<string, dynamic>> data = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(Convert2.GetString(client, url));
                        for (int i = 0; i < data.Count; i++)
                        {
                            FruitModel fruit = new FruitModel
                            {
                                Id = data[i]["id"],
                                Name = data[i]["name"],
                                Family = data[i]["family"],
                                Order = data[i]["order"],
                                Genus = data[i]["genus"],

                            };
                            Dictionary<string, dynamic> nutritions = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(Convert2.JsonToString(data[i]["nutritions"]));
                            fruit.Calories = nutritions["calories"];
                            fruit.Fat = nutritions["fat"];
                            fruit.Sugar = nutritions["sugar"];
                            fruit.Carbohydrates = nutritions["carbohydrates"];
                            fruit.Protein = nutritions["protein"];

                            fruitsList.Add(fruit);
                        }
                        fruitsList.Sort((x, y) => x.Id.CompareTo(y.Id));
                    }
                }
            }
            return View(fruitsList);
        }
    }

    class Convert2 
    {
        public static string GetString(HttpClient client, string url) 
        {
            return client.GetStringAsync(url).Result;
        }
        public static string JsonToString(JObject obj) 
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
