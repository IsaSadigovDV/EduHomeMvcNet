using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AssestmentController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _env;
        public AssestmentController(EduHomeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseAssests CourseAssests)
        {
            if (!ModelState.IsValid)
            {
                return View(CourseAssests);  
            }
            CourseAssests.CreatedDate = DateTime.UtcNow.AddHours(4);
            await _context.AddAsync(CourseAssests);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            CourseAssests? CourseAssests = await _context.courseAssests
                .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
        
            if(CourseAssests == null)
            {
                return NotFound();
            }
            return View(CourseAssests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, CourseAssests CourseAssests)
        {
            CourseAssests? updatedcourseassets= await _context.courseAssests
                .Where(x=>!x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if(CourseAssests == null)
            {
                return NotFound();
            }
            if(!ModelState.IsValid)
            {
                return View(updatedcourseassets);
            }


            updatedcourseassets.Name = CourseAssests.Name;
            updatedcourseassets.UpdatedDate = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {

            CourseAssests? CourseAssests = await _context.courseAssests
                .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();

            if (CourseAssests == null)
            {
                return NotFound();
            }

            CourseAssests.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
