using Microsoft.EntityFrameworkCore;

namespace ApplicationCQRSApplication.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public DbSet<Employee> Employees => Set<Employee>();
    }
}
