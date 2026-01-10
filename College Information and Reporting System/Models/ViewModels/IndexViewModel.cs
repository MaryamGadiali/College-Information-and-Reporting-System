using College_Information_and_Reporting_System.Enums;
using College_Information_and_Reporting_System.Models.Domain;
using College_Information_and_Reporting_System.Models.DTOs;

namespace College_Information_and_Reporting_System.Models.ViewModels
{
    //Model attributes passed to the home page
    public class IndexViewModel
    {
        public List<Student> Students { get; set; }

        public List<StudentRiskDTO> StudentRisks { get; set; }
    }
}
