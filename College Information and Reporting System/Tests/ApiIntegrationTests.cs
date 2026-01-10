using Azure;
using College_Information_and_Reporting_System.Data;
using College_Information_and_Reporting_System.Enums;
using College_Information_and_Reporting_System.Models.Domain;
using College_Information_and_Reporting_System.Models.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace College_Information_and_Reporting_System.Tests
{
    public class ApiIntegrationTests : IClassFixture<TestWebApplicationFactory> //creates one factory instance to share acrooss all the tests in this class
    {

        private readonly HttpClient _httpClient;
        private readonly TestWebApplicationFactory _factory;
        private readonly ITestOutputHelper _logging;

        public ApiIntegrationTests(TestWebApplicationFactory factory, ITestOutputHelper logging) {
            _factory = factory;
            _httpClient = factory.CreateClient();
            _logging = logging;


        }

        private Student createValidStudent()
        {
            return new Student
            {
                //studentId = 1,
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
               courseName="Maths",
               budget=2000,
               isActive=true,
               startYear= new DateOnly(2020, 9, 1)
            };
        }

        private AttendanceCreateDTO createValidAttendanceDTO(Student student, Course course)
        {
            return new AttendanceCreateDTO
            {
                attendanceStatus = "Present",
                studentId = student.studentId,
                courseId = course.courseId,
                attendanceTime =  DateTime.Now
            };
        }


        [Fact]
        public async Task GetStudentById_ReturnsStudent_WhenExists()
        {

            //Arrange
            Student student = createValidStudent();

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.students.Add(student);
                await db.SaveChangesAsync();
            }



            //Act
            var result = await _httpClient.GetAsync($"/api/student/{student.studentId}");
            var body = await result.Content.ReadAsStringAsync();

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            body.Should().Contain("Mary");

        }


        [Fact]
        public async Task GetStudentById_ReturnsNotFound_WhenNotExists()
        {

            //Arrange

            //Act
            var result = await _httpClient.GetAsync($"/api/student/-1");
            var body = await result.Content.ReadAsStringAsync();

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            body.Should().Contain("Invalid student ID");

        }






        [Fact]
        public async Task deleteStudentById_ReturnsOK_WhenDeleted()
        {

            //Arrange
            Student student = createValidStudent();

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.students.Add(student);
                await db.SaveChangesAsync();
            }

            //Act
            var result = await _httpClient.DeleteAsync($"/api/student/{student.studentId}");
            var body = await result.Content.ReadAsStringAsync();

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            body.Should().Contain("Successfully deleted");

        }


        [Fact]
        public async Task deleteStudentById_ReturnsNotFound_WhenNotExists()
        {

            //Arrange
            
            //Act
            var result = await _httpClient.DeleteAsync($"/api/student/-1");
            var body = await result.Content.ReadAsStringAsync();

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            body.Should().Contain("Invalid student ID");

        }



        [Fact]
        public async Task updateCourseName_ReturnsCourse_WhenValid()
        {

            //Arrange
            Course course = createValidCourse();

            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.courses.Add(course);
                await db.SaveChangesAsync();
            }

            JsonContent jsonContent = JsonContent.Create("Mathematics");

            //Act
            var result = await _httpClient.PatchAsync($"/api/course/{course.courseName}",jsonContent);
            var body = await result.Content.ReadAsStringAsync();

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            body.Should().Contain("Mathematics");

        }


        [Fact]
        public async Task updateCourseName_ReturnsNotFound_WhenNotExists()
        {

            //Arrange
            JsonContent jsonContent = JsonContent.Create("Mathematics");

            //Act
            var result = await _httpClient.PatchAsync($"/api/course/-1", jsonContent);
            var body = await result.Content.ReadAsStringAsync();

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            body.Should().Contain("Invalid course name");

        }






        [Fact]
        public async Task createAttendanceRecord_ReturnsAttendance_WhenValid()
        {

            //Arrange
            Student student = createValidStudent();
            Course course = createValidCourse();


            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.courses.Add(course);
                student.courses.Add(course);
                db.students.Add(student);

                _logging.WriteLine(student.courses.FirstOrDefault().courseId.ToString());
                _logging.WriteLine(course.courseId.ToString());
                await db.SaveChangesAsync();
            }

            AttendanceCreateDTO attendanceRecord = createValidAttendanceDTO(student, course);

            JsonContent jsonContent = JsonContent.Create(attendanceRecord);

            //Act
            var result = await _httpClient.PostAsync($"/api/attendance/", jsonContent);
            var body = await result.Content.ReadAsStringAsync();

            _logging.WriteLine($"Status: {result.StatusCode}");
            _logging.WriteLine(body);

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
            body.Should().Contain(student.studentId.ToString());
            body.Should().Contain(course.courseId.ToString());

        }

    }
}
