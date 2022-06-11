using System;
using System.Collections.Generic;
using System.Linq;
using Models.Models;
using System.ComponentModel.DataAnnotations;
namespace WebApi.Dto
{
    public class CourseDto
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        public int Id { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/2/2004", "3/4/3004",
            ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [Range(typeof(DateTime), "1/2/2004", "3/4/3004",
            ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Pass credit")]
        [Range(1,100)]
        public int PassCredits { get; set; }

        public virtual List<StudentDto> Students { get; set; } = new List<StudentDto>();

        public virtual List<HomeTaskDto> HomeTasks { get; set; } = new List<HomeTaskDto>();

        public Course ToModel()
        {
            return new Course()
            {
                Name = Name,
                Id = Id,
                StartDate = StartDate,
                EndDate = EndDate,
                PassCredits = PassCredits,
                Students = Students.Select(p => new Student()
                {
                    Id = p.Id,
                    BirthDate = p.BirthDate,
                    Email = p.Email,
                    GitHubLink = p.GitHubLink,
                    Name = p.Name,
                    PhoneNumber = p.PhoneNumber,
                    Notes = p.Notes
                }).ToList(),
                HomeTasks = HomeTasks.Select(p => new HomeTask()
                {
                    Date = p.Date,
                    Id = p.Id,
                    Description = p.Description,
                    Number = p.Number,
                    CourseId = p.CourseId,
                    Title = p.Title
                }).ToList()
            };
        }

        public static CourseDto FromModel(Course course)
        {
            return new CourseDto()
            {
                Id = course.Id,
                Name = course.Name,
                StartDate = course.StartDate,
                PassCredits = course.PassCredits,
                EndDate = course.EndDate,
                HomeTasks = course.HomeTasks.Select(p => new HomeTaskDto()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Number = p.Number,
                    CourseId = p.CourseId,
                    Title = p.Title,
                    Date = p.Date
                }).ToList()
            };
        }
    }

}