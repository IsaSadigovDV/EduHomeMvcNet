﻿using EduHome.App.Context;
using EduHome.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduHome.App;

namespace EduHome.App.Controllers
{
    public class AboutController : Controller
    {
        private readonly EduHomeDbContext _context;

        public AboutController(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            AboutVM aboutVM = new AboutVM()
            {
                Notices = _context.Notices.Where(x => !x.IsDeleted).ToList(),
                Teachers = _context.Teachers.Where(x => !x.IsDeleted)
                .Include(x => x.Position)
                  .Include(x => x.Degree)
                 .Include(x => x.Socials)
                .ToList(),
                Setting = _context.Settings.Where(x => !x.IsDeleted)
                   .FirstOrDefault(),
            };
            return View(aboutVM);
        }
    }
}
