using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubscribeController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SubscribeController(EduHomeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Subscribe> subscribes = await _context.Subscribes.Where(x=>!x.IsDeleted).ToListAsync();
            return View(subscribes);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Subscribe? subscribe = await _context.Subscribes.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (subscribe != null)
            {
                return NotFound();
            }
            subscribe.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
