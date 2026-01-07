using College_Information_and_Reporting_System.Data;
using College_Information_and_Reporting_System.Models.ViewModels;
using College_Information_and_Reporting_System.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("Home")]
        [HttpGet]
        public IActionResult Index()
        {
            var model = new IndexViewModel
            {
                Students = _studentService.getStudents(),
                StudentRisks = _studentService.getStudentRisks()
            };

            return View(model);
        }

        //'/students/@student.studentId/attendance'"

        [Route("students/{studentId}/attendance")]
        public async Task<IActionResult> StudentAttendance([FromRoute] string studentId)
        {
            var attendanceRecords = await _studentService.getAttendanceForStudentId(studentId);
            Console.WriteLine(attendanceRecords);
            //handle if records is null
            return View(attendanceRecords);

        }
    }
}
