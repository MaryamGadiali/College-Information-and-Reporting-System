using Azure;
using College_Information_and_Reporting_System.Data;
using College_Information_and_Reporting_System.Enums;
using College_Information_and_Reporting_System.Models.Domain;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace College_Information_and_Reporting_System.Tests
{
    public class ApiIntegrationTests : IClassFixture<TestWebApplicationFactory> //creates one factory instance to share acrooss all the tests in this class
    {

        private readonly HttpClient _httpClient;
        private readonly TestWebApplicationFactory _factory;

        public ApiIntegrationTests(TestWebApplicationFactory factory) {
            _factory = factory;
            _httpClient = factory.CreateClient();

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
            var result = await _httpClient.GetAsync($"/api/{student.studentId}");
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
            var result = await _httpClient.GetAsync($"/api/-1");
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
            var result = await _httpClient.DeleteAsync($"/api/{student.studentId}");
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
            var result = await _httpClient.DeleteAsync($"/api/-1");
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
            var result = await _httpClient.PatchAsync($"/api/{course.courseName}",jsonContent);
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
            var result = await _httpClient.PatchAsync($"/api/-1", jsonContent);
            var body = await result.Content.ReadAsStringAsync();

            //Assert
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
            body.Should().Contain("Invalid course name");

        }

    }
}
