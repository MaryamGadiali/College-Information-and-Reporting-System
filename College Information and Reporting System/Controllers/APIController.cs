using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace College_Information_and_Reporting_System.Controllers
{

    [ApiController]
    [Route("api")]
    public class APIController : ControllerBase
    {
        [HttpGet("Hello")]
        public string Hello()
        {
            Console.WriteLine("Hello world");
            return "Hello World";
        }

        [HttpGet("Welcome")]
        public string Welcome([FromQuery]  string? name="Bob")
        {
            return HtmlEncoder.Default.Encode($"Hello {name}");
        }

        [HttpGet("PathVar/{name}")]
        public string PathVar([FromRoute] string name)
        {
            return $"Hello {name}";
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById([FromRoute] int id)
        {
            return Ok($"id={id}");
        }

       

    }
}
