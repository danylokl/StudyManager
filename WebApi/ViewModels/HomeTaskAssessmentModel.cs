using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Dto
{
    public class HomeTaskAssessmentModel
    {
        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<HomeTaskStudentViewModel> HomeTaskStudents { get; set; } = new List<HomeTaskStudentViewModel>();

        public int HomeTaskId { get; set; }
    }

    public class HomeTaskStudentViewModel
    {
        public int StudentId { get; set; }

        public string StudentFullName { get; set; }

        public bool IsComplete { get; set; }

        public int HomeTaskAssessmentId { get; set; }
    }
}
