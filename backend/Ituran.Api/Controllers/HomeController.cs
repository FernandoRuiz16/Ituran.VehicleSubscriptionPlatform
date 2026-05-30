using Microsoft.AspNetCore.Mvc;

namespace Ituran.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
