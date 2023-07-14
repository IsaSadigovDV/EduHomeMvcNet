using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly EduHomeDbContext _context;
        public CategoryController(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "category");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Category? category = await _context.Categories
                .Where(x => !x.IsDeleted && x.Id == id)
                                 .FirstOrDefaultAsync();
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id ,Category postcategory)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Category? category = await _context.Categories
             .Where(x => !x.IsDeleted && x.Id == id)
                              .FirstOrDefaultAsync();
            if (category == null)
                return NotFound();

            category.Name = postcategory.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Category? category = await _context.Categories
                    .Where(x => !x.IsDeleted && x.Id == id)
                         .FirstOrDefaultAsync();
            if (category == null)
                return NotFound();

            category.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
