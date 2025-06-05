using Microsoft.EntityFrameworkCore;
using TaskFlowAPI.Services;
using TasksManagerAPI.Models.Entity;

namespace TasksManagerAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = 1,
                    Email = "admin@gmail.com",
                    Username = "Test",
                    PasswordHash = "pruebaharcord"
                }
            );
            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = 1,
                    Name = "Test",
                }
            );
        }

    }
}
