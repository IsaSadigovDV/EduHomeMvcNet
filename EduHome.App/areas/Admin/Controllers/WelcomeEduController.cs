using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WelcomeEduController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _env;

        public WelcomeEduController(EduHomeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<WelcomeEdu> welcomeEdus = await _context.WelcomeEdus
                .Where(x => !x.IsDeleted).ToListAsync();
            return View(welcomeEdus);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WelcomeEdu welcomeEdu)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if(welcomeEdu == null)
            {
                return NotFound();
            }

            if(welcomeEdu.FormFile == null)
            {
                ModelState.AddModelError("Formfile", "This field is required only image");
            }
            if (!Helper.IsImage(welcomeEdu.FormFile))
            {
                ModelState.AddModelError("Formfile", "File type must be image");
            }
            if (!Helper.IsSizeOk(welcomeEdu.FormFile,2))
            {
                ModelState.AddModelError("Formfile", "File size can not be more than 2 mb");
            }

            welcomeEdu.Image = welcomeEdu.FormFile.CreateImage(_env.WebRootPath, "/img/about");
            welcomeEdu.CreatedDate = DateTime.UtcNow.AddHours(4);
            await _context.AddAsync(welcomeEdu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            WelcomeEdu? welcomeEdu = await _context.WelcomeEdus
                    .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if(welcomeEdu == null)
            {
                return NotFound();
            }
            return View(welcomeEdu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, WelcomeEdu welcomeEdu)
        {
            WelcomeEdu? updatedWelcomeEdu = await _context.WelcomeEdus
                   .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (welcomeEdu == null)
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
            {
                return View(welcomeEdu);
            }

            if(welcomeEdu.FormFile!= null)
            {
                if (!Helper.IsImage(welcomeEdu.FormFile))
                {
                    ModelState.AddModelError("Formfile", "File type must be image");
                }
                if (!Helper.IsSizeOk(welcomeEdu.FormFile, 2))
                {
                    ModelState.AddModelError("Formfile", "File size can not be more than 2 mb");
                }
                Helper.RemoveImage(_env.WebRootPath, "/img/about", updatedWelcomeEdu.Image);
                updatedWelcomeEdu.Image = welcomeEdu.FormFile.CreateImage(_env.WebRootPath, "/img/about");
            }

            updatedWelcomeEdu.Title = welcomeEdu.Title;
            updatedWelcomeEdu.Description = welcomeEdu.Description;
            updatedWelcomeEdu.Image = welcomeEdu.Image;
            updatedWelcomeEdu.UpdatedDate = DateTime.UtcNow.AddHours(4);
            updatedWelcomeEdu.Link = welcomeEdu.Link;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            WelcomeEdu? welcomeEdu = await _context.WelcomeEdus
                   .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if (welcomeEdu == null)
            {
                return NotFound();
            }
            welcomeEdu.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
