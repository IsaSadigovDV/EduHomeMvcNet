using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using EduHome.App.Helpers;
using EduHome.App.Extentions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EduHome.App.areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _env;


        public SliderController(EduHomeDbContext context, IWebHostEnvironment env = null)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Slider> sliders = await _context.Sliders.Where(x => !x.IsDeleted)
                    .ToListAsync();
            return View(sliders);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (slider.FormFile == null)
            {
                ModelState.AddModelError("Formfile", "This field is required only image");
            }

            if (!Helper.IsImage(slider.FormFile))
            {
                ModelState.AddModelError("Formfile", "File type must be image");
            }
            if (!Helper.IsSizeOk(slider.FormFile,2))
            {
                ModelState.AddModelError("Formfile", "File size can not be more than 2 mb");
            }
            slider.Image = slider.FormFile.CreateImage(_env.WebRootPath, "/img");
            slider.CreatedDate = DateTime.UtcNow.AddHours(4);
            await _context.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }






    }
}
