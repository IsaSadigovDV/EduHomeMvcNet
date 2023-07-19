using EduHome.App.Context;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Controllers
{
    public class ContactController : Controller
    {
        private readonly EduHomeDbContext _context;

        public ContactController(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ContactVM contactVM = new ContactVM()
            {
                Setting = await _context.Settings.Where(x => !x.IsDeleted).FirstOrDefaultAsync()
            };
            return View(contactVM);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }

            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
