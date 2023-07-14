using Microsoft.EntityFrameworkCore;

namespace EduHome.App.Context
{
    public class EduHomeDbContext:DbContext
    {
        public EduHomeDbContext(DbContextOptions<EduHomeDbContext> options) : base(options) { }

    }
}
