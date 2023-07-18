using EduHome.App.Context;
using Microsoft.AspNetCore.Mvc;
using EduHome.Core.Entities;
using Microsoft.EntityFrameworkCore;
using EduHome.App.Helpers;
using EduHome.App.Extentions;

namespace EduHome.App.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BlogController(EduHomeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Blog> blogs = await _context.Blogs.Where(x => !x.IsDeleted)
                .ToListAsync();
            return View(blogs);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();

            if (!ModelState.IsValid)
            {
                return View(blog);
            }
            if (blog.file is null)
            {
                ModelState.AddModelError("file", "Image can not be empty");
                return View(blog);
            }
            if (!Helper.IsImage(blog.file))
            {
                ModelState.AddModelError("file", "File must be image");
                return View(blog);
            }
            if (!Helper.IsSizeOk(blog.file, 2))
            {
                ModelState.AddModelError("file", "Size of Image must less than 2 mb");
                return View(blog);
            }
            foreach (var item in blog.CategoryIds)
            {
                if (!await _context.Categories.AnyAsync(x => x.Id == item))
                {
                    ModelState.AddModelError("", "Invalid Category Id");
                    return View(blog);
                }
                BlogCategory blogCategory = new BlogCategory
                {
                    CreatedDate = DateTime.Now,
                    Blog = blog,
                    CategoryId = item
                };
                await _context.BlogCategories.AddAsync(blogCategory);
            }
            foreach (var item in blog.TagIds)
            {
                if (!await _context.Tags.AnyAsync(x => x.Id == item))
                {
                    ModelState.AddModelError("", "Invalid Tag Id");
                    return View(blog);
                }
                BlogTag blogTag = new BlogTag
                {
                    CreatedDate = DateTime.Now,
                    Blog = blog,
                    Tagid = item
                };
                await _context.BlogTags.AddAsync(blogTag);
            }
            blog.Image = blog.file.CreateImage(_env.WebRootPath, "img/blog/");
            blog.CreatedDate = DateTime.Now;
            await _context.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
            ViewBag.Tags = await _context.Tags.Where(x => !x.IsDeleted).ToListAsync();

            Blog? blog = await _context.Blogs.Where(x => !x.IsDeleted && x.Id == id)
                .AsNoTracking()
                .Include(x => x.BlogCategories).ThenInclude(x => x.Category)
                .Include(x => x.BlogTags).ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync();
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, Blog blog)
        {
            Blog? updatedblog = await _context.Blogs.Where(x => !x.IsDeleted && x.Id == id)
                             .AsNoTracking()
                            .Include(x => x.BlogCategories).ThenInclude(x => x.Category)
                            .Include(x => x.BlogTags).ThenInclude(x => x.Tag)
                            .FirstOrDefaultAsync();

            if (blog is null)
            {
                return View(blog);
            }
            if (!ModelState.IsValid)
            {
                return View(updatedblog);
            }

            if(blog.file != null)
            {
                if (!Helper.IsImage(blog.file))
                {
                    ModelState.AddModelError("file", "File must be image");
                    return View(blog);
                }
                if (!Helper.IsSizeOk(blog.file, 2))
                {
                    ModelState.AddModelError("file", "Size of Image must less than 2 mb");
                    return View(blog);
                }
                Helper.RemoveImage(_env.WebRootPath, "img/blog/", updatedblog.Image);
                blog.Image = blog.file.CreateImage(_env.WebRootPath, "img/blog/");
            }
            else
            {
                blog.Image = updatedblog.Image;
            }

            List<BlogCategory> RemoveableCategory = await _context.BlogCategories.
              Where(x => !blog.CategoryIds.Contains(x.CategoryId)).ToListAsync();

            _context.BlogCategories.RemoveRange(RemoveableCategory);
            foreach (var item in blog.CategoryIds)
            {
                if (_context.BlogCategories.Where(x => x.BlogId == id && x.CategoryId == item).Count() > 0)
                    continue;

                await _context.BlogCategories.AddAsync(new BlogCategory
                {
                    BlogId = id,
                    CategoryId = item
                });
            }
            List<BlogTag> RemoveableTag = await _context.BlogTags.
            Where(x => !blog.TagIds.Contains(x.Tagid)).ToListAsync();
            _context.BlogTags.RemoveRange(RemoveableTag);

            foreach (var item in blog.TagIds)
            {
                if (_context.BlogTags.Where(x => x.BlogId == id && x.Tagid == item).Count() > 0)
                    continue;

                await _context.BlogTags.AddAsync(new BlogTag
                {
                    BlogId = id,
                    Tagid = item
                });
            }
            updatedblog.UpdatedDate = DateTime.Now;
            updatedblog.Author = blog.Author;
            updatedblog.Description = blog.Description;
            updatedblog.Title = blog.Title;
            updatedblog.Image=blog.Image;

            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            Blog? blog = await _context.Blogs.Where(x=>!x.IsDeleted && x.Id==id).FirstOrDefaultAsync();
            if(blog == null)
            {
                return NotFound();
            }

            blog.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); 
        }

    }
}
