using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SkillController : Controller
    {
        private readonly EduHomeDbContext _context;

        public SkillController(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Skill> skills = await _context.Skills.Where(x=>!x.IsDeleted).ToListAsync();
            return View(skills);
        }

        public async Task<IActionResult> Create()
        {
             return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Skill skill)
        {
            ViewBag.Teacher = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            if(!ModelState.IsValid)
            {
                return View();
            }
            await _context.AddAsync(skill);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Teacher = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();
            
            Skill? skill = await _context.Skills
                    .Where(x=>!x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if(skill == null)
            {
                return NotFound();
            }
            return View(skill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Skill postskill)
        {
            ViewBag.Teacher = await _context.Teachers.Where(x => !x.IsDeleted).ToListAsync();

            if(!ModelState.IsValid)
            {
                return View();
            }
            Skill? skill = await _context.Skills
                    .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (skill == null)
            {
                return NotFound();
            }
            skill.Name = postskill.Name;
            skill.SkillPercent = postskill.SkillPercent;
            skill.Teacher = postskill.Teacher;
            skill.CreatedDate = DateTime.Now;
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
            Skill? skill = await _context.Skills
                    .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (skill == null)
            {
                return NotFound();
            }

            skill.IsDeleted= true;
            skill.UpdatedDate = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
