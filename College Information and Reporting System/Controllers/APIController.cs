using College_Information_and_Reporting_System.Data;
using College_Information_and_Reporting_System.Enums;
using College_Information_and_Reporting_System.Models.Domain;
using College_Information_and_Reporting_System.Models.DTOs;
using College_Information_and_Reporting_System.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Encodings.Web;

namespace College_Information_and_Reporting_System.Controllers
{

    [ApiController]
    [Route("api")]
    public class APIController : ControllerBase
    {

        private readonly IStudentService _studentService;

        public APIController(IStudentService studentService) 
        {
            _studentService = studentService; 
        }


        [SwaggerOperation(
            Summary = "Get student by ID",
            Description = "Returns a student if the ID exists."
        )]
        [SwaggerResponse(200, "Student found", typeof(Student))]
        [SwaggerResponse(404, "Student not found")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById([FromRoute] int id)
        {
            Console.WriteLine(id);
            Student student = await _studentService.getStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound("Invalid student ID");
            }
            return Ok(student);
        }

        //CREATE new attendance record for student
        [HttpPost]
        public async Task<IActionResult> createAttendanceRecord([FromBody] AttendanceCreateDTO attendanceRecord)
        {
            //check if enum is valid
            var newAttendanceStatus= _studentService.isAttendanceStatusCheck(attendanceRecord.attendanceStatus);
            if (newAttendanceStatus==null){
                return BadRequest("Please enter a valid status");
            }
          
                Attendance attendance = new Attendance
                {
                    attendanceTime = attendanceRecord.attendanceTime,
                    attendanceStatus = (AttendanceStatus)newAttendanceStatus,
                    student = await _studentService.getStudentByIdAsync(attendanceRecord.studentId),
                    course = await _studentService.getCourseByIdAsync(attendanceRecord.courseId)
                };


            //error handling
            if (attendance.student == null)
            {
                return BadRequest("Please enter a valid student ID");
            }
            else if (attendance.course == null)
            {
                return BadRequest("Please enter a valid course ID");
            }
            else if (attendance.attendanceStatus == null)
            {
                return BadRequest("Please enter a valid status");
            }
            else if (!await _studentService.isStudentCourseMatchAsync(attendance.student.studentId, attendance.course.courseId))
            {
                return BadRequest("Student is not enrolled in this course");
            }

            _studentService.addAttendanceRecord(attendance);

            return Ok(attendance);
        }

        ////UPDATE course name
        [HttpPatch("{oldCourseName}")]
        public async Task<IActionResult> updateCourseName([FromRoute] string oldCourseName, [FromBody] string newCourseName)
        {
            Console.WriteLine(oldCourseName);
            Course course= await _studentService.getCourseByNameAsync(oldCourseName);

            if (course == null)
            {
                return NotFound("Invalid course name");
            }

            course.courseName = newCourseName;
            await _studentService.SaveChangesAsync();
            return Ok(course);
        }

        //delete student
        [HttpDelete("{id}")]
        public async Task<IActionResult> deleteStudentById([FromRoute] int id)
        {
            Student student = await _studentService.getStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound("Invalid student ID");
            }
            await _studentService.deleteStudentAsync(student);
            return Ok("Successfully deleted");
        }

    }
}
