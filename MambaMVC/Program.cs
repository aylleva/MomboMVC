using MambaMVC.DAL;
using MambaMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MambaMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDBContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
            );

            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;

                opt.Password.RequiredLength = 8;
                opt.Password.RequireNonAlphanumeric = false;

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
                opt.Lockout.MaxFailedAccessAttempts = 3;
            }
            ).AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders();


            var app = builder.Build();


            app.UseStaticFiles();

            app.MapControllerRoute(
                "admin",
                "{area:exists}/{controller=home}/{action=index}/{Id?}"
                );
            app.MapControllerRoute(
               "default",
               "{controller=home}/{action=index}/{Id?}"
               );
            app.Run();
        }
    }
}
