using College_Information_and_Reporting_System.Enums;

namespace College_Information_and_Reporting_System.Models.DTOs
{
    //Object class that maps to vw_atriskstudents
    //Used for the home page
    public class StudentRiskDTO
    {
        public int studentId { get; set; }
        public string studentFirstName { get; set; }
        public string studentLastName { get; set; }
        public decimal attendanceAvg { get; set; }
        public string riskStatus { get; set; }
    }
}
