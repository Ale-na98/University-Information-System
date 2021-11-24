using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    internal class StudentsRepository : UniversityRepository<StudentDb>, IStudentsRepository
    {
        private readonly UniversityContext _context;
        private readonly DbSet<StudentDb> _studentDbSet;

        public StudentsRepository(UniversityContext universityDbContext) : base(universityDbContext)
        {
            _context = universityDbContext;
            _studentDbSet = universityDbContext.Set<StudentDb>();
        }
        public int Create(StudentDb student)
        {
            var result = _studentDbSet.Add(student);
            _context.SaveChanges();
            return result.Entity.Id;
        }
    }
}
