using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models.Models;
namespace EntityFrameWork
{
    public class Context : DbContext
    {
        private readonly IOptions< RepositoryOptions> _options;
        public Context(IOptions<RepositoryOptions> options)
        {
            _options = options;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine);
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_options.Value.DefaultConnectionString);
            optionsBuilder.UseLazyLoadingProxies();
        }
        public DbSet<Course> Courses { get; set; } 
        public DbSet<HomeTask> HomeTasks { get; set; }
        public DbSet<HomeTaskAssessment> HomeTaskAssessment { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
