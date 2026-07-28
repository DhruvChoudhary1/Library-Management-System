using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    public class AboutUSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}