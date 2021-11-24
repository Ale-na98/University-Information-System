using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    internal class AttendanceRepository : UniversityRepository<AttendanceDb>, IAttendanceRepository
    {
        private readonly UniversityContext _context;
        private readonly DbSet<AttendanceDb> _attendanceDbSet;

        public AttendanceRepository(UniversityContext universityDbContext) : base(universityDbContext)
        {
            _context = universityDbContext;
            _attendanceDbSet = universityDbContext.Set<AttendanceDb>();
        }

        public void Create(AttendanceDb attendance)
        {
            _attendanceDbSet.Add(attendance);
            _context.SaveChanges();
        }

        public AttendanceDb Get(int lectureId, int studentId, DateTime lectureDate)
        {
            return _attendanceDbSet.Find(lectureId, studentId, lectureDate);
        }

        public ICollection<AttendanceDb> GetLectureAttendance(string lectureName)
        {
            return _attendanceDbSet.Where(a => a.Lecture.Name == lectureName).Include(a => a.Student).ToList();
        }

        public ICollection<AttendanceDb> GetStudentAttendance(string studentFullName)
        {
            return _attendanceDbSet.Where(a => a.Student.FullName == studentFullName).Include(a => a.Lecture).ToList();
        }

        public int GetNumberOfAbsenteeism(int lectureId, int studentId)
        {
            return _attendanceDbSet.Where(a => a.LectureId == lectureId && a.StudentId == studentId).Count(a => a.Presence == false);
        }
    }
}