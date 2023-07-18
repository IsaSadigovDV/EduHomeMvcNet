using EduHome.App.Context;
using EduHome.App.Extentions;
using EduHome.App.Helpers;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeacherController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeacherController(EduHomeDbContext context, IWebHostEnvironment env=null)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Teacher> teachers = await _context.Teachers
                .Where(x => !x.IsDeleted)
                .Include(x=>x.Position)
                .Include(x=>x.Degree).ToListAsync();
            return View(teachers);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Position = await _context.Positions.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Degree = await _context.Degrees.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Hobbies = await _context.Hobbies.Where(x => !x.IsDeleted).ToListAsync();
			return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            ViewBag.Position=await _context.Positions.Where(x=>!x.IsDeleted).ToListAsync();
            ViewBag.Degree = await _context.Degrees.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Hobbies = await _context.Hobbies.Where(x => !x.IsDeleted).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (teacher.FormFile == null)
            {
                ModelState.AddModelError("Formfile", "This field is required only image");
                return View(teacher);
            }

            if (!Helper.IsImage(teacher.FormFile))
            {
                ModelState.AddModelError("Formfile", "File type must be image");
            }
            if (!Helper.IsSizeOk(teacher.FormFile, 2))
            {
                ModelState.AddModelError("Formfile", "File size can not be more than 2 mb");
            }

			foreach (var item in teacher.HobbyIds)
			{
				if (!await _context.Hobbies.AnyAsync(x => x.Id == item))
				{
					ModelState.AddModelError("", "Invalid Hobby Id");
					return View(teacher);
				}
				TeacherHobby teacherHobby = new TeacherHobby
				{
					CreatedDate = DateTime.Now,
					Teacher = teacher,
					HobbyId = item
				};
				await _context.TeacherHobbies.AddAsync(teacherHobby);
			}

			teacher.Image = teacher.FormFile.CreateImage(_env.WebRootPath, "img/teacher/");
            teacher.CreatedDate = DateTime.UtcNow.AddHours(4);
            await _context.AddAsync(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
			ViewBag.Position = await _context.Positions.Where(x => !x.IsDeleted).ToListAsync();
			ViewBag.Degree = await _context.Degrees.Where(x => !x.IsDeleted).ToListAsync();
			ViewBag.Hobbies = await _context.Hobbies.Where(x => !x.IsDeleted).ToListAsync();

            Teacher? teacher = await _context.Teachers
                .Where(x => !x.IsDeleted && x.Id == id)
                .Include(x => x.Degree)
                .Include(x => x.Position)
                .Include(x => x.TeacherHobbies)
                    .ThenInclude(x => x.Hobby)
                .FirstOrDefaultAsync();

            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int id, Teacher teacher)
        {
			ViewBag.Position = await _context.Positions.Where(x => !x.IsDeleted).ToListAsync();
			ViewBag.Degree = await _context.Degrees.Where(x => !x.IsDeleted).ToListAsync();
			ViewBag.Hobbies = await _context.Hobbies.Where(x => !x.IsDeleted).ToListAsync();

			Teacher? updatedteacher = await _context.Teachers
				.Where(x => !x.IsDeleted && x.Id == id)
				.Include(x => x.Degree)
				.Include(x => x.Position)
				.Include(x => x.TeacherHobbies)
					.ThenInclude(x => x.Hobby)
				.FirstOrDefaultAsync();

			if (teacher == null)
			{
				return NotFound();
			}

            if (!ModelState.IsValid)
            {
                return View(updatedteacher);
            }

            if(teacher.FormFile != null)
            {
				if (!Helper.IsImage(teacher.FormFile))
				{
					ModelState.AddModelError("Formfile", "File type must be image");
				}
				if (!Helper.IsSizeOk(teacher.FormFile, 2))
				{
					ModelState.AddModelError("Formfile", "File size can not be more than 2 mb");
				}

                Helper.RemoveImage(_env.WebRootPath, "img/teacher/", updatedteacher.Image);
                teacher.Image = teacher.FormFile.CreateImage(_env.WebRootPath, "img/teacher/");
			}
            else
            {
                teacher.Image = updatedteacher.Image;
            }

            teacher.UpdatedDate = DateTime.UtcNow.AddHours(4);
            _context.Update(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
		}

        public async Task<IActionResult> Delete(int id)
        {
            Teacher? teacher = await _context.Teachers
                .Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
            if(teacher == null)
            {
                return NotFound();
            }
            teacher.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
