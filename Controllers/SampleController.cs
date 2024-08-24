using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class SampleController : Controller
    {
        private static List<SampleModel> sampleList = new List<SampleModel>();
        public IActionResult Index()
        {
            List<FruitModel> fruits = FruitController.fruitsList;
            
            if (sampleList.Count == 0) 
            {
                Console.WriteLine("Walay sulod sampleList");
                fruits.ForEach(f =>
                {
                    SampleModel sample = new SampleModel
                    {
                        Name = f.Name,
                        Family = f.Family,
                    };
                    sampleList.Add(sample);
                });
            }
            Console.WriteLine("Count Sample: " + sampleList.Count);
            return View(sampleList);
        }
    }
}
