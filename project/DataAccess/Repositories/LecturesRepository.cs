using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    internal class LecturesRepository : UniversityRepository<LectureDb>, ILecturesRepository
    {
        private readonly UniversityContext _context;
        private readonly DbSet<LectureDb> _lectureDbSet;

        public LecturesRepository(UniversityContext universityDbContext) : base(universityDbContext)
        {
            _context = universityDbContext;
            _lectureDbSet = universityDbContext.Set<LectureDb>();
        }
        public int Create(LectureDb lecture)
        {
            var result = _lectureDbSet.Add(lecture);
            _context.SaveChanges();
            return result.Entity.Id;
        }
    }
}