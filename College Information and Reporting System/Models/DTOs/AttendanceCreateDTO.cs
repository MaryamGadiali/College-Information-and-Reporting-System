using College_Information_and_Reporting_System.Enums;
using College_Information_and_Reporting_System.Models.Domain;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace College_Information_and_Reporting_System.Models.DTOs
{
    public class AttendanceCreateDTO
    {
        public DateTime attendanceTime { get; set; }
        //[EnumDataType(typeof(AttendanceStatus))]
        
        public string attendanceStatus { get; set; }
        public required int studentId { get; set; }
        public required int courseId { get; set; }
    }
}
