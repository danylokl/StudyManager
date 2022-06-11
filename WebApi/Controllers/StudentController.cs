using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Services;
using WebApi.Dto;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
   
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;
        private readonly IAuthorizationService _authorizationService;
        public StudentController(StudentService studentService, IAuthorizationService authorizationService)
        {
            _studentService = studentService;
            _authorizationService = authorizationService;
        }
        [HttpGet]
        public IActionResult Students()
        {
            IEnumerable<StudentDto> model = (_studentService.GetAllStudents().Select(student => StudentDto.FromModel(student)));
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var student = _studentService.GetStudentById(id);
            var result = await _authorizationService.AuthorizeAsync(User, student, "UserAccessPolicy");
            if (result.Succeeded)
            {
                var model = StudentDto.FromModel(student);
                ViewBag.Action = "Edit";
                return View("Save", model);
            }
            return Forbid();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Action = "Create";
            return View("Save", new StudentDto());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([FromForm] StudentDto value)
        {
            ViewBag.Action = "Create";
            var insertresult = _studentService.CreateStudent(value.ToModel());
            if (insertresult.HasErrors)
            {
                return BadRequest(insertresult.Errors);
            }
            return RedirectToAction("Students");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] StudentDto value)
        {
            if (!ModelState.IsValid)
            {
                return View("Save", value);
            }
            var result = _studentService.UpdateStudent(value.ToModel());
            var resultauthorization = await _authorizationService.AuthorizeAsync(User, value.ToModel(), "UserAccessPolicy");
            if (resultauthorization.Succeeded)
            {
                if (result.HasErrors)
                {
                    _studentService.UpdateStudent(value.ToModel());
                    return BadRequest(result.Errors);
                }
                ViewBag.Action = "Edit";
                return RedirectToAction("Students");
            }
            return Forbid();
            
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _studentService.DeleteStudent(id);
            return Accepted();
        }
        
    }
}
