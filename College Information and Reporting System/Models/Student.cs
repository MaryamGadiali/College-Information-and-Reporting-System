using College_Information_and_Reporting_System.Enums;

namespace College_Information_and_Reporting_System.Models
{
    public class Student
    {
        public int studentId { get; set; }

        public string studentFirstName { get; set; }

        public string studentLastName { get; set; }

        public Gender studentGender { get; set; }

        public DateOnly studentDateOfBirth { get; set; }

        public Ethnicity studentEthnicity { get; set; }   

        public DateOnly studentStartDate { get; set; }

        public DateOnly? studentEndDate { get; set; }

        public StudentStatus status { get; set; }

        public DateTime createdAt { get; set; }

        public ICollection<Course> courses { get; set; } = new List<Course>();







    }
}
