using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DegreeController : Controller
    {
        private readonly EduHomeDbContext _context;

        public DegreeController(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Degree> degrees = await _context.Degrees
                        .Where(x => !x.IsDeleted).ToListAsync();
            return View(degrees);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Degree degree)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _context.AddAsync(degree);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Degree? degree = await _context.Degrees
                  .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if(degree == null)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Degree updatedDegree)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Degree? degree = await _context.Degrees
                  .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (updatedDegree == null)
            {
                return NotFound();
            }

            degree.Name= updatedDegree.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Degree? degree = await _context.Degrees
                 .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (degree == null)
            {
                return NotFound();
            }

            degree.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }
}
