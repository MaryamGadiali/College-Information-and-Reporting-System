using College_Information_and_Reporting_System.Enums;

namespace College_Information_and_Reporting_System.Models
{
    public class Attendance
    {
        public int attendanceId { get; set; }
        public DateOnly attendanceTime { get; set; }
        //public DateTime recordedAt { get; set; } may not be needed
        public AttendanceStatus attendanceStatus {  get; set; }
        public Student student { get; set; }

    }
}
