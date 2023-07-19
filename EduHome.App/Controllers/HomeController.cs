using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace EduHome.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly EduHomeDbContext _context;

        public HomeController(EduHomeDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostSubscribe(string email)
        {
            if (email == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid Email");
            }

            _context.Subscribes?.AddAsync(new Subscribe { Email = email, CreatedDate = DateTime.Now });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}