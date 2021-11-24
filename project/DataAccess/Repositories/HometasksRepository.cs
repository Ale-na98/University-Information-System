using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    internal class HometasksRepository : UniversityRepository<HometaskDb>, IHometasksRepository
    {
        private readonly UniversityContext _context;
        private readonly DbSet<HometaskDb> _hometaskDbSet;

        public HometasksRepository(UniversityContext universityDbContext) : base(universityDbContext)
        {
            _context = universityDbContext;
            _hometaskDbSet = universityDbContext.Set<HometaskDb>();
        }
        public int Create(HometaskDb hometask)
        {
            var result = _hometaskDbSet.Add(hometask);
            _context.SaveChanges();
            return result.Entity.Id;
        }

        public double GetAverageMark(int lectureId, int studentId)
        {
            return _context.Hometasks.Where(h => h.LectureId == lectureId && h.StudentId == studentId).Average(h => h.Mark);
        }
    }
}