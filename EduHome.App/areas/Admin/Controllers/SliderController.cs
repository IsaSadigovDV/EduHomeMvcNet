using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _env;


        public SliderController(EduHomeDbContext context, IWebHostEnvironment env = null)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {

            return View();
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        

    }
}
