﻿using EduHome.App.Context;
using EduHome.App.ViewModels;
using EduHome.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Controllers
{
    public class TeacherController : Controller
    {
        private readonly EduHomeDbContext _context;

        public TeacherController(EduHomeDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Teacher> teachers = await _context.Teachers.Where(x => !x.IsDeleted)
                .Include(x=>x.Socials)
                .Include(x=>x.Position)
                .ToListAsync();
            return View(teachers);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Teacher? teacher = await _context.Teachers.Where(x => !x.IsDeleted)
                .Include(x => x.TeacherHobbies)
                 .ThenInclude(x => x.Hobby)
                .Include(x => x.Socials)
                .Include(x=>x.Skills)
                .Include(x=>x.Position)
                .Include(x=>x.Degree)
                .FirstOrDefaultAsync();

            if(teacher == null)
            {
                return NotFound();
            }

            TeacherVM teacherVM = new TeacherVM
            {
                Teacher = teacher,
            };
            return View(teacherVM);
        }


    }
}
