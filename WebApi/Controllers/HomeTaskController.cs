using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Services;
using WebApi.Dto;
using Microsoft.AspNetCore.Routing;
using Models.Models;
using System;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    public class HomeTask : Controller
    {
        private readonly HomeTaskService _hometaskService;
        private readonly StudentService _studentService;
        public HomeTask(HomeTaskService hometaskService, StudentService studentService)
        {
            _hometaskService = hometaskService;
            _studentService = studentService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(int courseId)
        {
            ViewBag.Action = "Create";
            ViewBag.CourseId = courseId;
            var model = new HomeTaskDto();
            return View("Save", model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(int courseId, [FromForm] HomeTaskDto value)
        {
       
            var routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("id", courseId);

            _hometaskService.CreateHomeTask(ToModel(value));
            return RedirectToAction("Edit", "Course", routeValueDictionary);
            
        }

        [HttpGet]
       
        public IActionResult Edit(int id)
        {
           Models.Models.HomeTask homeTask = _hometaskService.GetHomeTaskById(id);
            if (homeTask == null)
                return NotFound();
            ViewBag.Action = "Edit";

            return View(HomeTaskDto.FromModel( homeTask));
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(HomeTaskDto homeTaskParameter)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Action = "Edit";

                return View(homeTaskParameter);
            }

            var homeTask = _hometaskService.GetHomeTaskById(homeTaskParameter.Id);

            var routeValueDictionary = new RouteValueDictionary();
            _hometaskService.UpdateHomeTask(ToModel(homeTaskParameter));
            routeValueDictionary.Add("id", homeTask.Course.Id);
            return RedirectToAction("Edit", "Course", routeValueDictionary);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int homeTaskId, int courseId)
        {
            _hometaskService.DeleteHomeTask(homeTaskId);

            var routeValueDictionary = new RouteValueDictionary();
            routeValueDictionary.Add("id", courseId);
            return RedirectToAction("Edit", "Course", routeValueDictionary);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Evaluate(int id)
        {
            var homeTask = _hometaskService.GetHomeTaskById(id);

            if (homeTask == null)
            {
                return NotFound();
            }

            HomeTaskAssessmentModel assessmentViewModel =
                new HomeTaskAssessmentModel
                {
                    Date = homeTask.Date,
                    Description = homeTask.Description,
                    Title = homeTask.Title,
                    HomeTaskStudents = new List<HomeTaskStudentViewModel>(),
                    HomeTaskId = homeTask.Id
                };

            if (homeTask.HomeTaskAssessments.Any())
            {
                foreach (var homeTaskHomeTaskAssessment in homeTask.HomeTaskAssessments)
                {
                    assessmentViewModel.HomeTaskStudents.Add(new HomeTaskStudentViewModel()
                    {

                        StudentFullName = homeTaskHomeTaskAssessment.Student.Name,
                        StudentId = homeTaskHomeTaskAssessment.Student.Id,
                        IsComplete = homeTaskHomeTaskAssessment.IsComplete,
                        HomeTaskAssessmentId = homeTaskHomeTaskAssessment.Id
                    });
                }

            }
            else
            {
                foreach (var student in homeTask.Course.Students)
                {
                    assessmentViewModel.HomeTaskStudents.Add(new HomeTaskStudentViewModel() { StudentFullName = student.Name, StudentId = student.Id });
                }
            }

            return View(assessmentViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult SaveEvaluation(HomeTaskAssessmentModel model)
        {
            var homeTask = _hometaskService.GetHomeTaskById(model.HomeTaskId);

            if (homeTask == null)
            {
                return NotFound();
            }

            foreach (var homeTaskStudent in model.HomeTaskStudents)
            {
                var target = homeTask.HomeTaskAssessments.Find(p => p.Id == homeTaskStudent.HomeTaskAssessmentId);
                if (target != null)
                {
                    target.Date = DateTime.Now;
                    target.IsComplete = homeTaskStudent.IsComplete;
                }
                else
                {
                    var student = _studentService.GetStudentById(homeTaskStudent.StudentId);
                    homeTask.HomeTaskAssessments.Add(new HomeTaskAssessment
                    {
                        HomeTask = homeTask,
                        IsComplete = homeTaskStudent.IsComplete,
                        Student = student,
                        Date = DateTime.Now

                    });
                }
                _hometaskService.UpdateHomeTask(homeTask);
            }
            return RedirectToAction("Courses", "Course");
        }
        public static Models.Models.HomeTask ToModel(HomeTaskDto homeTask)
        {
            return new Models.Models.HomeTask()
            {
                CourseId = homeTask.CourseId,
                Id = homeTask.Id,
                Number = homeTask.Number,
                Date = homeTask.Date,
                Description = homeTask.Description,
                Title = homeTask.Title
            };
        }
    }
}
