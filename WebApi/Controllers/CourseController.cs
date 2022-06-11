using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Services;
using WebApi.Dto;
using Models.Models;
using Microsoft.AspNetCore.Authorization;
namespace WebApi.Controllers
{
  
        public class CourseController : Controller
    {
        private readonly CourseService _courseService;
        private readonly StudentService _studentService;
        public CourseController(CourseService courseService, StudentService studentService)
        {
            _courseService = courseService;
            _studentService = studentService;
        }
        [HttpGet]
        public ViewResult Courses()
        {
            IEnumerable<CourseDto> model = (_courseService.GetAllCourses().Select(course => CourseDto.FromModel(course)));
            return View(model);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var course = _courseService.GetCourseById(id);

            if (course == null)
            {
                return NotFound();
            }
            var viewmodel = CourseDto.FromModel(course);
            ViewBag.Action = "Edit";
            return View("Save",viewmodel);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Action = "Create";
            return View("Save", new CourseDto());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromForm] CourseDto value)
        {
            ViewBag.Action = "Create";
            var insertresult = _courseService.CreateCourse(value.ToModel());
            if (insertresult.HasErrors)
            {
                return BadRequest(insertresult.Errors);
            }
            return RedirectToAction("Courses");
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, [FromForm] CourseDto value)
        {
            ViewBag.Action = "Edit";
            if (!ModelState.IsValid)
            {
                return View("Save", value);
            }
            var updateResult = _courseService.UpdateCourse(value.ToModel());
            if (updateResult.HasErrors)
            {
                return View("Save", value);
            }
            
            return RedirectToAction("Courses");
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignStudent (int id)
        {
            var allstudents = _studentService.GetAllStudents();
            var course = _courseService.GetCourseById(id);
            if (course == null)
            {
                return BadRequest();
            }
            var model = new CourseStudentAssignmentModel();
            model.Id = id;
            model.EndDate = course.EndDate;
            model.Name = course.Name;
            model.StartDate = course.StartDate;
            model.PassCredits = course.PassCredits;
            model.Students = new List<AssignmentStudentViewModel>();

            foreach (var student in allstudents)
            {
                bool isAssigned = course.Students.Any(p => p.Id == student.Id);
                model.Students.Add(new AssignmentStudentViewModel() { StudentId = student.Id, StudentFullName = student.Name, IsAssigned = isAssigned });
            }
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignStudent(CourseStudentAssignmentModel model)
        {
            _courseService.SetStudentsToCourse(model.Id, model.Students.Where(st => st.IsAssigned).Select(student => student.StudentId));
            return RedirectToAction("Courses");
        }
        [HttpGet("[Action]/{id}")]
        public IActionResult Delete(int id)
        {
            _courseService.DeleteCourse(id);
            return RedirectToAction("Courses");
        }
        
    }
}
