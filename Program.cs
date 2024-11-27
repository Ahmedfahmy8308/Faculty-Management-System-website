using FacultyWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FacultyWebsite.Data;
using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Configuration;

namespace FacultyWebsite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<AppDbcontext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDbContext<DBAppContext>(options =>
                           options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<AppDbcontext>();

            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<AppDbcontext>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("student", policy => policy.RequireClaim(ClaimTypes.Role, "student"));
                options.AddPolicy("doctor", policy => policy.RequireClaim(ClaimTypes.Role, "doctor"));
                options.AddPolicy("affaire", policy => policy.RequireClaim(ClaimTypes.Role, "affaire"));
            });
            var loginPath = builder.Configuration.GetValue<string>("Application:Loginpath");
            Console.WriteLine(loginPath);
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = loginPath;
                options.AccessDeniedPath = loginPath;
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}