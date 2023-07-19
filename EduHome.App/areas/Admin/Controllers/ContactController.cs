using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ContactController(EduHomeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Contact> contacts = await _context.Contacts.Where(x => !x.IsDeleted).ToListAsync();
            return View(contacts);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Contact? contact = await _context.Contacts.Where(x=>!x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (contact == null)
            {
                return NotFound();
            }
            contact.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
