using EduHome.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Context
{
    public class EduHomeDbContext:DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<WelcomeEdu> WelcomeEdus { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherHobby> TeacherHobbies  { get; set; }
        public DbSet<Social> Socials  { get; set; }
        public DbSet<Skill> Skills  { get; set; }
        public DbSet<Position> Positions  { get; set; }
        public DbSet<Hobby> Hobbies  { get; set; }
        public DbSet<Degree> Degrees  { get; set; }
        public DbSet<Tag> Tags  { get; set; }
        public EduHomeDbContext(DbContextOptions<EduHomeDbContext> options) : base(options) { }

    }
}
