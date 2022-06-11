using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
//using ADO.NET;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Models;
using Models.Models;
using Services;
using EntityFrameWork;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Areas.Identity.Data;
using WebApi.Data;
using System.Threading.Tasks;
using System;
namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddRazorPages();
            services.AddMvc();
            services.AddScoped<CourseService>();
            services.AddScoped<StudentService>();
            services.AddScoped<HomeTaskService>();
            services.Configure<RepositoryOptions>(Configuration);
            services.AddDbContext<Context>();
            services.Add(ServiceDescriptor.Scoped(typeof(IRepository<>), typeof(UniversityRepository<>)));
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Course}/{action=Courses}");
                endpoints.MapRazorPages();

            });
        }

    }


}