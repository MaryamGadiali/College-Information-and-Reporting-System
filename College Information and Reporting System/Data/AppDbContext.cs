using College_Information_and_Reporting_System.Models.Domain;
using College_Information_and_Reporting_System.Models.DTOs;
using Microsoft.EntityFrameworkCore;
namespace College_Information_and_Reporting_System.Data
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        public DbSet<Student> students { get; set; }

        public DbSet<Course> courses { get; set; }

        public DbSet<Department> departments { get; set; }

        public DbSet<Attendance> attendances { get; set; }

        public DbSet<StudentRiskDTO> studentRisks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentRiskDTO>()
                .HasNoKey()
                .ToView("vw_atriskstudents");
        }
    }
}
