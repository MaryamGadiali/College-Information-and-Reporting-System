using College_Information_and_Reporting_System.Data;
using College_Information_and_Reporting_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace College_Information_and_Reporting_System.Controllers
{
    public class WebAppController : Controller
    {

        private readonly IStudentService _studentService;

        public WebAppController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [Route("Home")]
        [HttpGet]
        public IActionResult Index()
        {
            var students = _studentService.getStudentDetails();
            return View(students);
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}


    }
}
