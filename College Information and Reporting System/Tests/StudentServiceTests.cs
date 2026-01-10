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
          
                studentFirstName = "Mary",
                studentLastName = "Smith",
                studentGender = Gender.Female,
                studentDateOfBirth = new DateOnly(2000, 1, 1),
                studentStartDate = new DateOnly(2020, 9, 1),
                status = StudentStatus.Active,
                createdAt = DateTime.UtcNow
            };
        }

        private Course createValidCourse()
        {
            return new Course
            {
                courseName = "Maths",
                budget = 2000,
                isActive = true,
                startYear = new DateOnly(2020, 9, 1)
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
        public async Task isStudentCourseMatch_ReturnsTrue_WhenMatches()
        {
            //Arrange
            var db = CreateDb();
            

            Course course = createValidCourse();
            Student student = createValidStudent();
            student.courses.Add(course);

            db.students.Add(student);
            await db.SaveChangesAsync();

            StudentService studentService = new StudentService(db);

            //Act 
            var result = await studentService.isStudentCourseMatchAsync(student.studentId,course.courseId);

            //Assert
            result.Should().BeTrue();

        }

        [Fact]
        public async Task isStudentCourseMatch_ReturnsFalse_WhenNoMatches()
        {
            //Arrange
            var db = CreateDb();
            StudentService studentService = new StudentService(db);


            //Act 
            var result = await studentService.isStudentCourseMatchAsync(0,-1);

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
            var result = await studentService.getStudentByIdAsync(student.studentId);

            //Assert
            result.Should().NotBeNull();
            result!.studentFirstName.Should().Be("Mary");

        }

        [Fact]
        public async Task getStudentByIdAsync_ReturnsNull_WhenNotFound()
        {
            //Arrange
            var db = CreateDb();
            StudentService studentService = new StudentService(db);

            //Act 
            var result = await studentService.getStudentByIdAsync(-1);

            //Assert
            result.Should().BeNull();
           

        }




    }
}
