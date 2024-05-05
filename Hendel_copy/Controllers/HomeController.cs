using Hendel_copy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Hendel_copy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public readonly MainContext _mainContext;

        public HomeController(ILogger<HomeController> logger, MainContext mainContext)
        {
            _logger = logger;
            _mainContext = mainContext;
        }

        public IActionResult ManWatch()
        {
            return View();
        }

        public IActionResult HumanWathc()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NewCollectionWatch()
        {
            return View();
        }

        public IActionResult NewCollectionWatch2()
        {
            return View();
        }

        public IActionResult NewCollectionWatch3()
        {
            return View();
        }

        public IActionResult BuyCollectionWatch()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}