using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HobbyController : Controller
    {
        private readonly EduHomeDbContext _context;

        public HobbyController(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Hobby> hobbies = await _context.Hobbies
                        .Where(x => !x.IsDeleted).ToListAsync();
            return View(hobbies);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Hobby hobby)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _context.AddAsync(hobby);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Hobby? hobby = await _context.Hobbies
                  .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if(hobby == null)
            {
                return NotFound();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Hobby updatedHobby)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Hobby? hobby = await _context.Hobbies
                  .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (updatedHobby == null)
            {
                return NotFound();
            }

            hobby.Name= updatedHobby.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            Hobby? hobby = await _context.Hobbies
                 .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (hobby == null)
            {
                return NotFound();
            }

            hobby.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
