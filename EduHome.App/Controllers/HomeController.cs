using EduHome.App.Context;
using EduHome.App.ViewModels;
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

        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Sliders = _context.Sliders.Where(x => !x.IsDeleted).ToList(),
                Notices = _context.Notices.Where(x => !x.IsDeleted).ToList(),
                Teachers = _context.Teachers.Where(x => !x.IsDeleted)
                .Include(x => x.Position)
                  .Include(x => x.Degree)
                 .Include(x => x.Socials).Take(3)
                .ToList(),
                Courses = _context.Courses.Where(x => !x.IsDeleted)
                 .ToList(),
                Settings = _context.Settings.Where(x => !x.IsDeleted)
                   .FirstOrDefault(),
                Blogs = _context.Blogs.Where(x => !x.IsDeleted)
                .Take(3)
                 .ToList(),
            };
            return View(homeVM);
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