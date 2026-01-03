using College_Information_and_Reporting_System.Data;
using College_Information_and_Reporting_System.Enums;
using College_Information_and_Reporting_System.Models.Domain;
using College_Information_and_Reporting_System.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace College_Information_and_Reporting_System.Tests
{
    public class StudentServiceTests
    {
        private AppDbContext CreateDb()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        private Student createValidStudent()
        {
            return new Student
            {
                studentId = 1,
                studentFirstName = "Mary",
                studentLastName = "Smith",
                studentGender = Gender.Female,
                studentDateOfBirth = new DateOnly(2000, 1, 1),
                studentStartDate = new DateOnly(2020, 9, 1),
                status = StudentStatus.Active,
                createdAt = DateTime.UtcNow
            };
        }


        [Fact]
        public void isAttendanceStatusCheck_ReturnsEnum_WhenValid()
        {
            //Arrange
            StudentService studentService = new StudentService(null); //Doesn't need db acion

            //Act 
            var result =studentService.isAttendanceStatusCheck("Present");

            //Assert
            result.Should().Be(Enums.AttendanceStatus.Present);

        }

        [Fact]
        public void isAttendanceStatusCheck_ReturnsNull_WhenInvalid()
        {
            //Arrange
            StudentService studentService = new StudentService(null); //Doesn't need db acion

            //Act 
            var result = studentService.isAttendanceStatusCheck("NotValid");

            //Assert
            result.Should().Be(null);

        }

        [Fact]
        public void isStudentCourseMatch_ReturnsTrue_WhenMatches()
        {
            //Arrange
            StudentService studentService = new StudentService(null); //Doesn't need db acion

            Course course = new Course();
            course.courseId = 1;

            Student student = new Student();
            student.courses.Add(course);

            //Act 
            var result = studentService.isStudentCourseMatch(student,course);

            //Assert
            result.Should().BeTrue();

        }

        [Fact]
        public void isStudentCourseMatch_ReturnsFalse_WhenNoMatches()
        {
            //Arrange
            StudentService studentService = new StudentService(null); //Doesn't need db acion

            Course course = new Course();
            course.courseId = 1;

            Student student = new Student();

            //Act 
            var result = studentService.isStudentCourseMatch(student, course);

            //Assert
            result.Should().BeFalse();

        }

        [Fact]
        public async Task getStudentByIdAsync_ReturnsStudent_WhenExists()
        {
            //Arrange
            var db = CreateDb();
            Student student = createValidStudent();

            db.students.Add(student);
            await db.SaveChangesAsync();

            StudentService studentService = new StudentService(db);

            //Act 
            var result = await studentService.getStudentByIdAsync(1);

            //Assert
            result.Should().NotBeNull();
            result!.studentFirstName.Should().Be("Mary");

        }

        [Fact]
        public async Task getStudentByIdAsync_ReturnsNull_WhenNotFound()
        {
            //Arrange
            var db = CreateDb();
            //Student student = createValidStudent();

            //db.students.Add(student);
            //await db.SaveChangesAsync();

            StudentService studentService = new StudentService(db);

            //Act 
            var result = await studentService.getStudentByIdAsync(1);

            //Assert
            result.Should().BeNull();
           

        }




    }
}
