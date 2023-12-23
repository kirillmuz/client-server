using Microsoft.EntityFrameworkCore;
using WebAPI.Models.Departments;
using WebAPI.Models.Employees;
using WebAPI.Models.Users;

namespace WebAPI.DbContexts
{
    public class ApplicationContext: DbContext
    {
        public DbSet<User> Users { get; set; } = null;
        public DbSet<Department> Departments { get; set; } = null;
        public DbSet<Employee> Employees { get; set; } = null;
        public DbSet<Education> Educations { get; set; } = null;
        public DbSet<WorkExperience> WorkExperience { get; set; } = null;
        public DbSet<UserFile> UserFiles { get; set; } = null;

        public ApplicationContext()
        {
            // Database.EnsureCreated();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=WebAPIDb;Username=postgres;Password=1qaz!QAZ");
        }
    }
}
