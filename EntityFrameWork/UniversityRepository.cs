using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Microsoft.EntityFrameworkCore;
namespace EntityFrameWork
{
    public class UniversityRepository<T> : IRepository<T> where T : class
    {
        private readonly Context _context;
        protected DbSet<T> DbSet;

        public UniversityRepository(Context context)
        {
            _context = context;
            context.Database.EnsureCreated();
            DbSet = _context.Set<T>();
        }

        public List<T> GetAll()
        {
            return DbSet.ToList();
        }

        public T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public T Create(T entity)
        {
            var result = DbSet.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Remove(T entity)
        {
            DbSet.Remove(entity);
            _context.SaveChanges();
        }

        public void Remove(int id)
        {
            var entity = GetById(id);
            Remove(entity);
        }
    }
}
