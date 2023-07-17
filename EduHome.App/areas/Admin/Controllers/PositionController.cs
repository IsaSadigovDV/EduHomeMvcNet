using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PositionController : Controller
    {
        private readonly EduHomeDbContext _context;
        public PositionController(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Position> positions = await _context.Positions.Where(x => !x.IsDeleted).ToListAsync();
            return View(positions);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Position position)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            await _context.AddAsync(position);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Position? position = await _context.Positions
                .Where(x => !x.IsDeleted && x.Id == id)
                                 .FirstOrDefaultAsync();
            if (position == null)
                return NotFound();

            return View(position);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id ,Position postposition)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Position? position = await _context.Positions
             .Where(x => !x.IsDeleted && x.Id == id)
                              .FirstOrDefaultAsync();
            if (position == null)
                return NotFound();

            position.Name = postposition.Name;
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
            Position? position = await _context.Positions
                    .Where(x => !x.IsDeleted && x.Id == id)
                         .FirstOrDefaultAsync();
            if (position == null)
                return NotFound();

            position.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
