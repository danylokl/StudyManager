using Microsoft.AspNetCore.Authorization;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Authorization
{
    public class SameStudentRequirementAuthorizationHandler : AuthorizationHandler<SameStudentRequirement, Student>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            SameStudentRequirement requirement,
            Student student)
        {
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
            }

            if (context.User.Identity?.Name == student.Email)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
