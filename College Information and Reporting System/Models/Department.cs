namespace College_Information_and_Reporting_System.Models
{
    public class Department
    {
        public int departmentId { get; set; }
        public string departmentName { get; set; }
        public string departmentCode { get; set; }
        public string departmentHead {  get; set; }

        public ICollection<Course> courseList { get; set; }

    }
}
