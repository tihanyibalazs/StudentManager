using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StudentManager.Models;
using StudentManager.Services;
using System;
using Microsoft.AspNetCore.Identity;

namespace StudentManager
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            services.AddDbContext<StudentManagerContext>(options =>
                   options.UseSqlite(connection));

            services.AddTransient<IStudentManagerService, StudentManagerService>();
            services.AddControllersWithViews();

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                var allowed = options.User.AllowedUserNameCharacters + "·¡È…ÌÕÛ”ˆ÷ı’˙⁄¸‹";
                options.User.AllowedUserNameCharacters = allowed;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<StudentManagerContext>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Students}/{action=Index}/{id?}");
            });

            DbInitialize.InitializeAsync(serviceProvider);
        }
    }
}
