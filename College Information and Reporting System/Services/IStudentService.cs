using College_Information_and_Reporting_System.Enums;
using College_Information_and_Reporting_System.Models.Domain;
using College_Information_and_Reporting_System.Models.DTOs;

namespace College_Information_and_Reporting_System.Services
{
    public interface IStudentService
    {
        List<Student> getStudents();

        List<StudentRiskDTO> getStudentRisks();
        Task<Student> getStudentByIdAsync(int id);
        Task<Course> getCourseByIdAsync(int courseId);
        Task addAttendanceRecord(Attendance attendance);
        Task<bool> isStudentCourseMatchAsync(int studentId, int courseId);
        AttendanceStatus? isAttendanceStatusCheck(string attendanceStatus);
        Task SaveChangesAsync();
        Task<Course> getCourseByNameAsync(string oldCourseName);
        Task deleteStudentAsync(Student student);
        //AttendanceStatus attendanceStatusEnumTransform(string attendanceStatus);
    }
}
