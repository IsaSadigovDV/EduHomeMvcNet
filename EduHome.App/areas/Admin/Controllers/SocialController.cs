using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SocialController : Controller
    {
        private readonly EduHomeDbContext _context;

        public SocialController(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Social> socials = await _context.Socials.Where(x=>!x.IsDeleted).ToListAsync();
            return View(socials);
        }

        public async Task<IActionResult> Create()
        {
             return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Social social)
        {
            ViewBag.Teacher = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            if(!ModelState.IsValid)
            {
                return View();
            }
            await _context.AddAsync(social);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Teacher = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            
            Social? social = await _context.Socials
                    .Where(x=>!x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if(social == null)
            {
                return NotFound();
            }
            return View(social);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Social postsocial)
        {
            ViewBag.Teacher = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();

            if(!ModelState.IsValid)
            {
                return View();
            }
            Social? social = await _context.Socials
                    .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (social == null)
            {
                return NotFound();
            }
            social.Name = postsocial.Name;
            social.Link= postsocial.Link;
            social.Teacher = postsocial.Teacher;
            social.CreatedDate = DateTime.Now;
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
            Social? social = await _context.Socials
                    .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (social == null)
            {
                return NotFound();
            }

            social.IsDeleted= true;
            social.UpdatedDate = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
