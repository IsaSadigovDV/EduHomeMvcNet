using EduHome.App.Context;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CourseLanguageController : Controller
	{
		private readonly EduHomeDbContext _context;
		private readonly IWebHostEnvironment _env;

		public CourseLanguageController(EduHomeDbContext context, IWebHostEnvironment env)
		{
			_context = context;
			_env = env;
		}

		public async Task<IActionResult> Index()
		{
			IEnumerable<CourseLanguage> courseLanguages = await _context.CourseLanguages
				.Where(x => !x.IsDeleted).ToListAsync();
			return View(courseLanguages);
		}
		[HttpGet]
		public async Task<IActionResult> Create()
		{
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CourseLanguage courseLanguage)
		{

			if (!ModelState.IsValid)
			{
				return View(courseLanguage);
			}
			courseLanguage.CreatedDate = DateTime.Now;
			await _context.AddAsync(courseLanguage);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		[HttpGet]
		public async Task<IActionResult> Update(int id)
		{
			CourseLanguage? courseLanguage = await _context.CourseLanguages
				.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
			
			if (courseLanguage == null)
			{
				return NotFound();
			}
			return View(courseLanguage);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]

		public async Task<IActionResult> Update(int id, CourseLanguage courseLanguage)
		{
			CourseLanguage? updatedcourselanguage = await _context.CourseLanguages
					.Where(x=>!x.IsDeleted &&x.Id == id).FirstOrDefaultAsync();
			if (!ModelState.IsValid)
			{
				return View(updatedcourselanguage);
			}
			if(courseLanguage == null)
			{
				return NotFound();
			}
			updatedcourselanguage.UpdatedDate = DateTime.Now;
			updatedcourselanguage.Name = courseLanguage.Name;
			updatedcourselanguage.Courses = courseLanguage.Courses;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));

		}


		public async Task<IActionResult> Delete(int id)
		{
			CourseLanguage? courseLanguage = await _context.CourseLanguages
				.Where(x => !x.IsDeleted && x.Id == id).FirstOrDefaultAsync();
			if (!ModelState.IsValid)
			{
				return NotFound();
			}
			if(courseLanguage == null)
			{
				return NotFound();
			}
			courseLanguage.IsDeleted = true;
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));	
		}
	}
}
