using System;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;
using Xunit;
using WebApi.Dto;
using Services;
using Models.Models;
using FluentAssertions;
using NSubstitute;

namespace ControllerTests
{
    public class CourseControllerTest
    {
        [Fact]
        public void Edit_ReturnsViewResult_WhenCourseExists()
        {


            // Arrange
            int courseid = 16;
            var courseService =Substitute.For<CourseService>();
            courseService.GetCourseById(courseid).Returns(new Course()
            {
                Id = courseid

            }) ;
            CourseController controller = new CourseController(courseService, null);
            
            // Act
            var actual = controller.Edit(courseid);
            // Assert
            var actualViewResult = Assert.IsType<ViewResult>(actual);
            CourseDto actualmodel = Assert.IsType<CourseDto>(actualViewResult.ViewData.Model);
            CourseDto expected = new CourseDto()
            {
                Id = courseid
            };
            actualmodel.Should().BeEquivalentTo(expected);
        }
        [Fact]
        public void Edit_NotFoundResult_WhenNoCourseFound()
        {


            // Arrange
            CourseController controller = new CourseController(null, null);
            int courseid = 16;
            // Act
            var actual = controller.Edit(courseid);
            // Assert
            var actualViewResult = Assert.IsType<ViewResult>(actual);
        }
    }
}
