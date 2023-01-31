using System.Linq;
using DataAccess.Interfaces;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UniversityRepository<TEntity> : IUniversityRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly AppContext _context;

        public UniversityRepository(AppContext universityDbContext)
        {
            _context = universityDbContext;
            _dbSet = universityDbContext.Set<TEntity>();
        }

        public TEntity Create(TEntity entity)
        {
            var result = _dbSet.Add(entity);
            _context.SaveChanges();
            return result.Entity;
        }

        public TEntity Get(int id)
        {
            return _dbSet.Find(id);
        }

        public IList<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public TEntity Update(TEntity entity)
        {
            _context.ChangeTracker.Clear();
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return entity;
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}
