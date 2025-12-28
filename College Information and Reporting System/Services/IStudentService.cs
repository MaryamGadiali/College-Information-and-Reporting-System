using College_Information_and_Reporting_System.Models.Domain;
using College_Information_and_Reporting_System.Models.DTOs;

namespace College_Information_and_Reporting_System.Services
{
    public interface IStudentService
    {
        List<Student> getStudents();

        List<StudentRiskDTO> getStudentRisks();
    }
}
