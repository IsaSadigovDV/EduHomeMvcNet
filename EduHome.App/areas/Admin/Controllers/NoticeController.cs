using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NoticeController : Controller
    {
        private readonly EduHomeDbContext _context;

        public NoticeController(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Notice> notices = await _context.Notices.Where(x=>!x.IsDeleted).ToListAsync();
            return View(notices);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Notice notice)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _context.AddAsync(notice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Notice? notice = await _context.Notices
                .Where(x => !x.IsDeleted && x.Id == id)
                                 .FirstOrDefaultAsync();
            if (notice == null)
                return NotFound();

            return View(notice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Notice postnotice)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Notice? notice = await _context.Notices
             .Where(x => !x.IsDeleted && x.Id == id)
                              .FirstOrDefaultAsync();
            if (notice == null)
                return NotFound();

            notice.Description = postnotice.Description;
            notice.dateTime= postnotice.dateTime;
            notice.UpdatedDate = DateTime.UtcNow.AddHours(4);
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
            Notice? notice = await _context.Notices
                    .Where(x => !x.IsDeleted && x.Id == id)
                         .FirstOrDefaultAsync();
            if (notice == null)
                return NotFound();

            notice.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
