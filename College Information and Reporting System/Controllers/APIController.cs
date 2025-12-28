using College_Information_and_Reporting_System.Data;
using College_Information_and_Reporting_System.Models.Domain;
using College_Information_and_Reporting_System.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace College_Information_and_Reporting_System.Controllers
{

    [ApiController]
    [Route("api")]
    public class APIController : ControllerBase
    {

        private readonly AppDbContext _db;
        private readonly IStudentService _studentService;

        public APIController(AppDbContext db, IStudentService studentService) //remove db
        {
            _db = db;
            _studentService = studentService; 
        }

        [HttpGet("Hello")]
        public string Hello()
        {
            Console.WriteLine("Hello world");
            return "Hello World";
        }

        [HttpGet("Welcome")]
        public IEnumerable<string> Welcome([FromQuery] string? name = "Bob")
        {
            return _db.students.Select(s => s.studentFirstName).ToList();
            //return HtmlEncoder.Default.Encode($"Hello {name}");
        }

        [HttpGet("PathVar/{name}")]
        public string PathVar([FromRoute] string name)
        {
            return $"Hello {name}";
        }

        //Path variable example
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById([FromRoute] int id)
        {
            Console.WriteLine(id);
            Student student = await _studentService.getStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound("Invalid student ID");
            }
            return Ok(student.studentFirstName);
        }

        ////create new attendance record for student
        //[HttpPost]
        //public async Task<IActionResult> createAttendanceRecord([FromBody] Attendance attendance)
        //{

        //}

       

    }
}
