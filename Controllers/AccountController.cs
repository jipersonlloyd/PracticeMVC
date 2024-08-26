using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System;
using WebApplication2.Models;
using System.Security.Principal;
using Newtonsoft.Json.Linq;
using System.Security;
using Humanizer;
using System.Text.Json.Nodes;

namespace WebApplication2.Controllers
{

    public class AccountController : Controller
    {
        private static List<AccountModel> accountList = new List<AccountModel>();
        public IActionResult Index()
        {
            accountList.Clear();
            using (HttpClient client = new HttpClient())
            {
                var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes("11191807:60-dayfreetrial"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
                string url = "http://strawberrydiaz-001-site1.ftempurl.com/api/Account/Accounts";
                var response = client.GetAsync(url).Result;
                string jsonData = client.GetStringAsync(url).Result;
                Dictionary<string, dynamic> jsonMap = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonData);
                Console.WriteLine($"status: {response.IsSuccessStatusCode}, Result: {jsonMap["Result"]}");
                if (!response.IsSuccessStatusCode || !jsonMap["Result"])
                {
                    AccountModel account = new AccountModel
                    {
                        FirstName = "No",
                        MiddleName = "Data",
                        LastName = "Found",
                        UserName = "Test",
                        Email = "test@gmail.com",
                        Password = "0"
                    };
                    accountList.Add(account);
                    return View(accountList);
                }

                JArray jsonArr = jsonMap["Message"];
                List<Dictionary<string, dynamic>> accounts = ConvertData.JsonToDictionary(jsonArr);
                for (int i = 0; i < accounts.Count; i++)
                {
                    AccountModel account = new AccountModel
                    {
                        FirstName = accounts[i]["firstName"],
                        MiddleName = accounts[i]["middleName"],
                        LastName = accounts[i]["lastName"],
                        UserName = accounts[i]["userName"],
                        Email = accounts[i]["email"],
                        Password = accounts[i]["password"],
                    };
                    accountList.Add(account);
                }
            }
            accountList.ForEach(item => { Console.WriteLine(item.UserName); });
            return View(accountList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateAccount(AccountModel account)
        {
            using (HttpClient client = new HttpClient())
            {
                var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes("11191807:60-dayfreetrial"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
                string url = "http://strawberrydiaz-001-site1.ftempurl.com/api/Account/Create";
                //string url = "https://localhost:7233/api/Account/Create";
                string jsonContent = JsonConvert.SerializeObject(account);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = client.PostAsync(url, content).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest();
                }
                accountList.Add(account);
                return RedirectToAction("Index");
            }
        }
    }

    class ConvertData
    {
        public static List<Dictionary<string, dynamic>> JsonToDictionary(JArray jsonArr)
        {
            List<Dictionary<string, dynamic>> accountList = new List<Dictionary<string, dynamic>>();
            for (int i = 0; i < jsonArr.Count; i++)
            {
                string account = JsonConvert.SerializeObject(jsonArr[i]);
                Dictionary<string, dynamic> accountPair = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(account);
                accountList.Add(accountPair);
            }

            return accountList;
        }
    }
}
