using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SettingController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SettingController(EduHomeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Setting> settings = await _context.Settings.Where(x=>!x.IsDeleted).ToListAsync();
            return View(settings);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Setting setting)
        {
            if (!ModelState.IsValid)
            {
                return View(setting);
            }
            if (setting.file is null)
            {
                ModelState.AddModelError("file", "Image must be added");
                return View(setting);
            }
            if (!Helper.IsImage(setting.file))
            {
                ModelState.AddModelError("file", "File must be image");
                return View(setting);
            }
            if (!Helper.IsSizeOk(setting.file, 1))
            {
                ModelState.AddModelError("file", "Size of Image must less than 1 mb!!!");
                return View(setting);
            }
            setting.AboutImage = setting.file.CreateImage(_env.WebRootPath, "img/slider/");
            setting.CreatedDate = DateTime.Now;
            await _context.AddAsync(setting);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Setting? setting = await _context.Settings.Where(x => x.Id == id && !x.IsDeleted).FirstOrDefaultAsync();
            if (setting is null)
            {
                return NotFound();
            }
            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Setting setting)
        {
            Setting? updatedsetting = await _context.Settings.Where(x => x.Id == id && !x.IsDeleted)
                  .FirstOrDefaultAsync();
            if (setting is null)
            {
                return View(setting);
            }
            if (!ModelState.IsValid)
            {
                return View(updatedsetting);
            }

            if (setting.file is not null)
            {
                if (!Helper.IsImage(setting.file))
                {
                    ModelState.AddModelError("file", "File must be image");
                    return View(setting);
                }
                if (!Helper.IsSizeOk(setting.file, 1))
                {
                    ModelState.AddModelError("file", "Size of Image must less than 1 mb!!!");
                    return View(setting);
                }
                Helper.RemoveImage(_env.WebRootPath, "img/about/", updatedsetting.AboutImage);
                updatedsetting.AboutImage = setting.file.CreateImage(_env.WebRootPath, "img/about/");
            }
            updatedsetting.AboutText = setting.AboutText;
            updatedsetting.AboutTitle = setting.AboutTitle;
            updatedsetting.VideoLink = setting.VideoLink;
            updatedsetting.Address = setting.Address;
            updatedsetting.Number1 = setting.Number1;
            updatedsetting.Number2 = setting.Number2;
            updatedsetting.Email = setting.Email;
            updatedsetting.FooterLogo = setting.FooterLogo;
            updatedsetting.HeaderLogo = setting.HeaderLogo;
            updatedsetting.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            Setting? setting = await _context.Settings.Where(x => x.Id == id && !x.IsDeleted)
                .FirstOrDefaultAsync();
            if (setting is null)
            {
                return NotFound();
            }
            setting.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
