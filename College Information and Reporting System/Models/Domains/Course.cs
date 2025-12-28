namespace College_Information_and_Reporting_System.Models.Domain
{
    public class Course
    {
        public int courseId { get; set; }
        //public string courseCode { get; set; }
        public string courseName { get; set; }
        public decimal budget { get; set; }
        public bool isActive { get; set; } = true;
        public DateOnly startYear { get; set; }
        public Department department { get; set; }
        public ICollection<Student> students { get; set; }

    }
}
