using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    internal class TeachersRepository : UniversityRepository<TeacherDb>, ITeachersRepository
    {
        private readonly UniversityContext _context;
        private readonly DbSet<TeacherDb> _teacherDbSet;

        public TeachersRepository(UniversityContext universityDbContext) : base(universityDbContext)
        {
            _context = universityDbContext;
            _teacherDbSet = universityDbContext.Set<TeacherDb>();
        }
        public int Create(TeacherDb teacher)
        {
            var result = _teacherDbSet.Add(teacher);
            _context.SaveChanges();
            return result.Entity.Id;
        }
    }
}