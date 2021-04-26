using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace StudentManager.Models
{
    public class DbInitialize
    {
        private static StudentManagerContext _context;
        private static UserManager<IdentityUser> _userManager;
        private static RoleManager<IdentityRole> _roleManager;

        public static void InitializeAsync(IServiceProvider serviceProvider)
        {
            _context = serviceProvider.GetRequiredService<StudentManagerContext>();
            _userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _context.Database.Migrate();

            SeedDatabaseStudents();
            SeedDatabaseMarks();
            SeedDatabaseAdmin();
        }

        public static void SeedDatabaseStudents()
        {
            for (int i=0;i<24;i++)
            {
                var student = new Student
                {
                    Name = "Example Student " + i,
                    PhoneNumber = "+36409837294" + i,
                    BirthDate = DateTime.Now,
                    ClassYear = i%12
                };
                _context.Students.Add(student);
            }
            _context.SaveChanges();
        }

        public static void SeedDatabaseMarks()
        {
            var student1 = _context.Students.Where(x => x.Id == 1).FirstOrDefault();
            var student2 = _context.Students.Where(x => x.Id == 2).FirstOrDefault();

            var mark1 = new Mark
            {
                Value = 1,
                Student = student1,
                StudentId = 1
            };

            var mark2 = new Mark
            {
                Value = 2,
                Student = student1,
                StudentId = 1
            };

            student1.MarksAverage = 1.5;

            var mark3 = new Mark
            {
                Value = 4,
                Student = student2,
                StudentId = 2
            };

            var mark4 = new Mark
            {
                Value = 5,
                Student = student2,
                StudentId = 2
            };

            student2.MarksAverage = 4.5;

            _context.Marks.Add(mark1);
            _context.Marks.Add(mark2);
            _context.Marks.Add(mark3);
            _context.Marks.Add(mark4);

            _context.SaveChanges();
        }

        public static void SeedDatabaseAdmin()
        {
            var adminRole = new IdentityRole("admin");

            var admin1 = new IdentityUser
            {
                UserName = "admin",
            };

            _userManager.CreateAsync(admin1, "edutest2021");
            _roleManager.CreateAsync(adminRole);
            _userManager.AddToRoleAsync(admin1, adminRole.Name);
        }
    }
}
