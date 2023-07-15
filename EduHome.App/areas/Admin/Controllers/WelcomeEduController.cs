using Microsoft.AspNetCore.Mvc;

namespace EduHome.App.Areas.Admin.Controllers
{
    public class WelcomeEduController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
