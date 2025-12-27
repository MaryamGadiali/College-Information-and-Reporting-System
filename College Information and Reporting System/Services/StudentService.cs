using College_Information_and_Reporting_System.Data;
using College_Information_and_Reporting_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace College_Information_and_Reporting_System.Services
{
    public class StudentService : IStudentService
    {

        private readonly AppDbContext _db;

        public StudentService(AppDbContext db)
        {
            _db = db;
        }

        public List<Student> getStudentDetails()
        {
            return _db.students.ToList();
        }
    }
}
