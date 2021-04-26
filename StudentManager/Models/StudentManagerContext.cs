using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace StudentManager.Models
{
    public class StudentManagerContext : IdentityDbContext<IdentityUser>
    {
        public StudentManagerContext(DbContextOptions<StudentManagerContext> options)
        : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Mark> Marks { get; set; }
    }
}
