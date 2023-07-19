using EduHome.App.Context;
using EduHome.Core.Entities;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using x = EduHome.App.Services.Interfaces;
using y = EduHome.App.Services.Implementations;

namespace EduHome.App.ServiceRegistrations
{
    public static class ServiceRegister
    {
        public static void Register(this IServiceCollection service, IConfiguration config)
        {
            service.AddScoped<x.IMailService, y.MailService>();
            service.AddIdentity<AppUser, IdentityRole>()
                   .AddDefaultTokenProviders()
                     .AddEntityFrameworkStores<EduHomeDbContext>();
            service.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.AllowedForNewUsers = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            });
            service.AddDbContext<EduHomeDbContext>(opt =>
            {
                opt.UseSqlServer(config.GetConnectionString("Default"));
            });
        }

    }
}
