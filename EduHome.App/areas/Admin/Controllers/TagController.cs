using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TagController : Controller
    {
        private readonly EduHomeDbContext _context;
        public TagController(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Tag> tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();
            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tag tag)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            await _context.AddAsync(tag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Tag? tag = await _context.Tags
                .Where(x => !x.IsDeleted && x.Id == id)
                                 .FirstOrDefaultAsync();
            if (tag == null)
                return NotFound();

            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id ,Tag posttag)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Tag? tag = await _context.Tags
             .Where(x => !x.IsDeleted && x.Id == id)
                              .FirstOrDefaultAsync();
            if (tag == null)
                return NotFound();

            tag.Name = posttag.Name;
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
            Tag? tag = await _context.Tags
                    .Where(x => !x.IsDeleted && x.Id == id)
                         .FirstOrDefaultAsync();
            if (tag == null)
                return NotFound();

            tag.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
