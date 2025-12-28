using College_Information_and_Reporting_System.Enums;

namespace College_Information_and_Reporting_System.Models.Domain
{
    public class Attendance
    {
        public int attendanceId { get; set; }
        public DateTime attendanceTime { get; set; }
        //public DateTime recordedAt { get; set; } may not be needed
        public AttendanceStatus attendanceStatus {  get; set; }
        public required Student student { get; set; }
        public required Course course { get; set; }

    }
}
