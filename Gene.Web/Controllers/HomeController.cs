using Gene.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gene.Web.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Info
        [HttpGet]
        public IActionResult Info(InfoViewModel model)
        {
            return View(model);
        }

        // GET: /Home/Error
        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(ErrorViewModel model)
        {
            return View(model);
        }

        // GET: /Home/NotExist
        [HttpGet]
        public IActionResult NotExist()
        {
            return View();
        }
    }
}
