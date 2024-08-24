using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class PersonController : Controller
    {
        private static List<PersonModel> personList = new List<PersonModel>();
        public IActionResult Index()
        {
            return View(personList);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(PersonModel person) 
        {
            personList.Add(person);
            return RedirectToAction("Index");
        }
    }
}
