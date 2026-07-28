using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    public class ContactUSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}