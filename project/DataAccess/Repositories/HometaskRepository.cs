using System.Linq;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    internal class HometaskRepository : UniversityRepository<HometaskDb>, IHometaskRepository
    {
        private readonly DbSet<HometaskDb> _hometaskDbSet;

        public HometaskRepository(AppContext universityDbContext) : base(universityDbContext)
        {
            _hometaskDbSet = universityDbContext.Set<HometaskDb>();
        }

        public double GetAverageMark(int lectureId, int studentId)
        {
            return _hometaskDbSet.Where(h => h.LectureId == lectureId && h.StudentId == studentId).Average(h => h.Mark);
        }
    }
}
