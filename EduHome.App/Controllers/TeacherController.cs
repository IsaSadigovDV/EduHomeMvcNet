using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Controllers
{
    public class TeacherController : Controller
    {
        private readonly EduHomeDbContext _context;

        public TeacherController(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Teacher> teachers = await _context.Teachers.Where(x => !x.IsDeleted)
                .Include(x=>x.Socials)
                .Include(x=>x.Position)
                .ToListAsync();
            return View(teachers);
        }


    }
}
