using Microsoft.AspNetCore.Mvc;

namespace Salus.Controllers
{
    public class DietController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
