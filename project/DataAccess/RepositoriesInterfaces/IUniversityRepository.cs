using System.Collections.Generic;

namespace DataAccess
{
    public interface IUniversityRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        ICollection<TEntity> GetAll();
        void Update(TEntity entity);
        void Delete(int id);
    }
}