using College_Information_and_Reporting_System.Enums;
using System.ComponentModel.DataAnnotations;

namespace College_Information_and_Reporting_System.Models.Domain
{
    public class Attendance
    {
        [Key]
        public int attendanceId { get; set; }
        public DateTime attendanceTime { get; set; }
        public AttendanceStatus attendanceStatus {  get; set; }
        public required Student student { get; set; }
        public required Course course { get; set; }
        //public DateTime recordedAt { get; set; } could be used in future
    }
}
