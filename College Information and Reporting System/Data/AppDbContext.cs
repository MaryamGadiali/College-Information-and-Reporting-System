using College_Information_and_Reporting_System.Models;
using Microsoft.EntityFrameworkCore;
namespace College_Information_and_Reporting_System.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        public DbSet<Student> students { get; set; }
       
    }
}
