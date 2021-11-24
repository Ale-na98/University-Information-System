using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    internal class UniversityRepository<TEntity> : IUniversityRepository<TEntity> where TEntity : class
    {
        private readonly UniversityContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public UniversityRepository(UniversityContext universityDbContext)
        {
            _context = universityDbContext;
            _dbSet = universityDbContext.Set<TEntity>();
        }

        public TEntity Get(int id)
        {
            return _dbSet.Find(id);
        }

        public ICollection<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public void Update(TEntity entity)
        {
            _context.ChangeTracker.Clear();
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}
