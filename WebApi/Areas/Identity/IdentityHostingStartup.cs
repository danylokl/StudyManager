using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Areas.Identity.Data;
using WebApi.Authorization;
using WebApi.Data;

[assembly: HostingStartup(typeof(WebApi.Areas.Identity.IdentityHostingStartup))]
namespace WebApi.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
         
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<UserContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("UserContextConnection")));
              
                services.ConfigureApplicationCookie(p =>
                {
                    p.Cookie.Name = "University";
                });
                services.AddDefaultIdentity<WebApiUser>(options => options.SignIn.RequireConfirmedAccount = false).AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<UserContext>(); 
                services.AddScoped<IAuthorizationHandler, SameStudentRequirementAuthorizationHandler>();
                services.AddAuthorization(options =>
                {
                    options.AddPolicy("UserAccessPolicy", builder =>
                    {
                        builder.Requirements.Add(new SameStudentRequirement());
                    });
                });
            });
          
          
        }
   
        private async Task CreateAdminUser(
           UserManager<WebApiUser> userManager,
           RoleManager<IdentityRole> roleManager)
        {
            WebApiUser webapiuser = new WebApiUser
            {
                Email = "Admin@test.com",
                UserName = "Admin@test.com"
            };
            var userRes = await userManager.CreateAsync(webapiuser, "SuperAdmin#1234");
            var rol = await roleManager.CreateAsync(new IdentityRole("Admin"));
            var res = await userManager.AddToRoleAsync(webapiuser, "Admin");
            var isInRole = await userManager.IsInRoleAsync(webapiuser, "Admin");
            var roles = await userManager.GetRolesAsync(webapiuser);
            var allRoles = await roleManager.Roles.ToListAsync();
            var x = userManager.Options.SignIn.RequireConfirmedAccount;
        }
    }
}