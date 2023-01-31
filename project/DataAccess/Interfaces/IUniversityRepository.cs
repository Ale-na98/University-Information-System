using System.Collections.Generic;

namespace DataAccess.Interfaces
{
    public interface IUniversityRepository<T> where T : class
    {
        T Create(T entity);
        T Get(int id);
        IList<T> GetAll();
        T Update(T entity);
        void Delete(int id);
    }
}
