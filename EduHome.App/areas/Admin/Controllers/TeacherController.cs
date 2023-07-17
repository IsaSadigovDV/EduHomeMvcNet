using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeacherController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeacherController(EduHomeDbContext context, IWebHostEnvironment env=null)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Teacher> teachers = await _context.Teachers
                .Where(x => !x.IsDeleted)
                .Include(x=>x.Position)
                .Include(x=>x.Degree).ToListAsync();
            return View(teachers);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Position = await _context.Positions.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Degree = await _context.Degrees.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Skill = await _context.Skills.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            ViewBag.Position=await _context.Positions.Where(x=>!x.IsDeleted).ToListAsync();
            ViewBag.Degree = await _context.Degrees.Where(x => !x.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (teacher.FormFile == null)
            {
                ModelState.AddModelError("Formfile", "This field is required only image");
            }

            if (!Helper.IsImage(teacher.FormFile))
            {
                ModelState.AddModelError("Formfile", "File type must be image");
            }
            if (!Helper.IsSizeOk(teacher.FormFile, 2))
            {
                ModelState.AddModelError("Formfile", "File size can not be more than 2 mb");
            }

            teacher.Image = teacher.FormFile.CreateImage(_env.WebRootPath, "img/slider/");
            teacher.CreatedDate = DateTime.UtcNow.AddHours(4);
            await _context.AddAsync(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
