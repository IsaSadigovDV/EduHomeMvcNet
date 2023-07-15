using EduHome.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Context
{
    public class EduHomeDbContext:DbContext
    {
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<WelcomeEdu> WelcomeEdus { get; set; }
        public EduHomeDbContext(DbContextOptions<EduHomeDbContext> options) : base(options) { }

    }
}
