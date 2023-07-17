using EduHome.App.Context;
using Microsoft.AspNetCore.Mvc;
using EduHome.Core.Entities;
using Microsoft.EntityFrameworkCore;
using EduHome.App.Helpers;
using EduHome.App.Extentions;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _env;

        public CourseController(EduHomeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Course> courses = await _context.Courses.Where(x => !x.IsDeleted)
                .Include(x => x.CourseLanguage)
                .ToListAsync();
            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Languages = await _context.CourseLanguages.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.CourseAssests = await _context.courseAssests.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Languages = await _context.CourseLanguages.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.CourseAssests = await _context.courseAssests.Where(x => !x.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(course);
            }
            if (course.file is null)
            {
                ModelState.AddModelError("file", "Image can not be empty");
                return View(course);
            }
            if (!Helper.IsImage(course.file))
            {
                ModelState.AddModelError("file", "File must be image");
                return View(course);
            }
            if (!Helper.IsSizeOk(course.file, 2))
            {
                ModelState.AddModelError("file", "Size of Image must less than 2 mb");
                return View(course);
            }
            foreach (var item in course.CategoryIds)
            {
                if (!await _context.Categories.AnyAsync(x => x.Id == item))
                {
                    ModelState.AddModelError("", "Invalid Category Id");
                    return View(course);
                }
                CourseCategory courseCategory = new CourseCategory
                {
                    CreatedDate = DateTime.Now,
                    Course = course,
                    CategoryId = item
                };
                await _context.CourseCategories.AddAsync(courseCategory);
            }
            foreach (var item in course.TagIds)
            {
                if (!await _context.Tags.AnyAsync(x => x.Id == item))
                {
                    ModelState.AddModelError("", "Invalid Tag Id");
                    return View(course);
                }
                CourseTag courseTag = new CourseTag
                {
                    CreatedDate = DateTime.Now,
                    Course = course,
                    TagId = item
                };
                await _context.CourseTags.AddAsync(courseTag);
            }
            course.Image = course.file.CreateImage(_env.WebRootPath, "img/course/");
            course.CreatedDate = DateTime.Now;
            await _context.AddAsync(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Languages = await _context.CourseLanguages.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.CourseAssests = await _context.courseAssests.Where(x => !x.IsDeleted).ToListAsync();

            Course? course = await _context.Courses.Where(x => !x.IsDeleted && x.Id == id)
                .AsNoTracking()
                .Include(x => x.courseCategories).ThenInclude(x => x.Category)
                .Include(x => x.courseTags).ThenInclude(x => x.Tag)
                .Include(x => x.CourseLanguage)
                .Include(x => x.courseAssests)
                .FirstOrDefaultAsync();
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Course course)
        {
            Course? updatedcourse = await _context.Courses.Where(x => !x.IsDeleted && x.Id == id)
                             .AsNoTracking()
                            .Include(x => x.courseCategories).ThenInclude(x => x.Category)
                            .Include(x => x.courseTags).ThenInclude(x => x.Tag)
                             .Include(x => x.CourseLanguage)
                             .Include(x => x.courseAssests)
                            .FirstOrDefaultAsync();

            if (course is null)
            {
                return View(course);
            }
            if (!ModelState.IsValid)
            {
                return View(updatedcourse);
            }

            if(course.file != null)
            {
                if (!Helper.IsImage(course.file))
                {
                    ModelState.AddModelError("file", "File must be image");
                    return View(course);
                }
                if (!Helper.IsSizeOk(course.file, 2))
                {
                    ModelState.AddModelError("file", "Size of Image must less than 2 mb");
                    return View(course);
                }
                Helper.RemoveImage(_env.WebRootPath, "img/course/", updatedcourse.Image);
                course.Image = course.file.CreateImage(_env.WebRootPath, "img/course/");
            }
            else
            {
                course.Image = updatedcourse.Image;
            }

            List<CourseCategory> RemoveableCategory = await _context.CourseCategories.
              Where(x => !course.CategoryIds.Contains(x.CategoryId)).ToListAsync();

            _context.CourseCategories.RemoveRange(RemoveableCategory);
            foreach (var item in course.CategoryIds)
            {
                if (_context.CourseCategories.Where(x => x.CourseId == id && x.CategoryId == item).Count() > 0)
                    continue;

                await _context.CourseCategories.AddAsync(new CourseCategory
                {
                    CourseId = id,
                    CategoryId = item
                });
            }
            List<CourseTag> RemoveableTag = await _context.CourseTags.
            Where(x => !course.TagIds.Contains(x.TagId)).ToListAsync();
            _context.CourseTags.RemoveRange(RemoveableTag);

            foreach (var item in course.TagIds)
            {
                if (_context.CourseTags.Where(x => x.CourseId == id && x.TagId == item).Count() > 0)
                    continue;

                await _context.CourseTags.AddAsync(new CourseTag
                {
                    CourseId = id,
                    TagId = item
                });
            }
            updatedcourse.Name = course.Name;
            updatedcourse.Description = course.Description;
            updatedcourse.UpdatedDate = DateTime.Now;
            updatedcourse.AboutText = course.AboutText; 
            updatedcourse.Apply= updatedcourse.Apply;
            updatedcourse.CourseFee = course.CourseFee;
            updatedcourse.SkillLevel = course.SkillLevel;
            updatedcourse.StartDate = course.StartDate;
            updatedcourse.EndDate = course.EndDate;
            updatedcourse.CourseLanguageId = course.CourseLanguageId;
            updatedcourse.CourseAssestsId = course.CourseAssestsId;
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            Course? course = await _context.Courses.Where(x=>!x.IsDeleted && x.Id==id).FirstOrDefaultAsync();
            if(course == null)
            {
                return NotFound();
            }

            course.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); 
        }

    }
}
