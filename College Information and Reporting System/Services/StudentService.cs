using College_Information_and_Reporting_System.Data;
using College_Information_and_Reporting_System.Models.Domain;
using College_Information_and_Reporting_System.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Core.Mapping;
using System.Linq;

namespace College_Information_and_Reporting_System.Services
{
    public class StudentService : IStudentService
    {

        private readonly AppDbContext _db;

        public StudentService(AppDbContext db)
        {
            _db = db;
        }

        public List<Student> getStudents()
        {
            return _db.students.ToList();
        }

        public List<StudentRiskDTO> getStudentRisks()
        {
            return _db.studentRisks.ToList();
        }

        public async Task<Student> getStudentByIdAsync(int id)
        {
            //Student foundStudent = (Student)_db.students.Where(s => s.studentId == id);
            //Console.WriteLine(foundStudent.studentLastName);
            return await _db.students.SingleOrDefaultAsync(s=>s.studentId==id);
        }
    }
}
