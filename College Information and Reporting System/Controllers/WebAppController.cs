using College_Information_and_Reporting_System.Data;
using College_Information_and_Reporting_System.Models.ViewModels;
using College_Information_and_Reporting_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace College_Information_and_Reporting_System.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
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
            //var students = _studentService.getStudentDetails();
            var model = new IndexViewModel
            {
                Students = _studentService.getStudents(),
                StudentRisks = _studentService.getStudentRisks()
            };

            return View(model);
        }

      

    }
}
