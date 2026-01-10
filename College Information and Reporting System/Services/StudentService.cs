using College_Information_and_Reporting_System.Data;
using College_Information_and_Reporting_System.Enums;
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
            return _db.students
                .Include(s => s.courses)
                .ToList();
        }

        public List<StudentRiskDTO> getStudentRisks()
        {
            return _db.studentRisks.ToList();
        }

        public async Task<Student> getStudentByIdAsync(int id)
        {
            return await _db.students.SingleOrDefaultAsync(s=>s.studentId==id);
        }

        public async Task<Course> getCourseByIdAsync(int courseId)
        {
            return await _db.courses.SingleOrDefaultAsync(c => c.courseId == courseId);
        }

        public async Task addAttendanceRecord(Attendance attendance)
        {
            _db.attendances.Add(attendance);
            await _db.SaveChangesAsync();
        }
           
        //Checks if the student is enrolled on the course
        public async Task<bool> isStudentCourseMatchAsync(int studentId, int courseId)
        {
            return await _db.students.AnyAsync(s=>s.studentId==studentId && s.courses.Any(c=>c.courseId==courseId));
          
        }

        //Checks if the attendance status field is a valid value matching the pre existing enums
        public AttendanceStatus? isAttendanceStatusCheck(string attendanceStatus)
        {
            if (Enum.TryParse<AttendanceStatus>(attendanceStatus, out var status))
            {
                return status;
            }
            return null;
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<Course> getCourseByNameAsync(string oldCourseName)
        {
            return await _db.courses.FirstOrDefaultAsync(c => c.courseName == oldCourseName);
        }

        public async Task deleteStudentAsync(Student student)
        {
             _db.students.Remove(student);
            await _db.SaveChangesAsync();

        }

        public async Task<List<Attendance>> getAttendanceForStudentId(string studentId)
        {
            return await _db.attendances
                .Include(a=>a.student)
                .Include(c=>c.course)
                .Where(s=>s.student.studentId.ToString() == studentId).ToListAsync();
        }
    }
}
