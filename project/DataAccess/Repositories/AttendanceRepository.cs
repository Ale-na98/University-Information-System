using System;
using System.Linq;
using DataAccess.Entities;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    internal class AttendanceRepository : UniversityRepository<AttendanceDb>, IAttendanceRepository
    {
        private readonly DbSet<AttendanceDb> _attendanceDbSet;

        public AttendanceRepository(AppContext universityDbContext) : base(universityDbContext)
        {
            _attendanceDbSet = universityDbContext.Set<AttendanceDb>();
        }

        public AttendanceDb GetWithLecture(int id)
        {
            return _attendanceDbSet
                .Include(attendance => attendance.Lecture)
                .FirstOrDefault(attendance => attendance.Id == id);
        }

        public Page<AttendanceDb> GetByFilter(int? groupId, int? studentId, int? lectureId,
            DateTime? lectureDateFrom, DateTime? lectureDateTo, PageParams parameters)
        {
            IQueryable<AttendanceDb> selectedAttendance = _attendanceDbSet.Include(attendance => attendance.Lecture);

            if (groupId != null)
                selectedAttendance = selectedAttendance.Where(attendance => attendance.Student.Group.Id == groupId);
            if (studentId != null)
                selectedAttendance = selectedAttendance.Where(attendance => attendance.Student.Id == studentId);

            if (lectureId != null)
                selectedAttendance = selectedAttendance.Where(attendance => attendance.Lecture.Id == lectureId);
            if (lectureDateFrom != null)
                selectedAttendance = selectedAttendance.Where(attendance => attendance.LectureDate >= lectureDateFrom);
            if (lectureDateTo != null)
                selectedAttendance = selectedAttendance.Where(attendance => attendance.LectureDate <= lectureDateTo);

            var page = new Page<AttendanceDb>
            {
                Data = selectedAttendance.OrderBy(attendance => attendance.Id)
                .Skip((parameters.CurrentPage - 1) * parameters.PageSize)
                .Take(parameters.PageSize).ToList(),

                TotalPages = GetTotalPages(selectedAttendance, parameters.PageSize)
            };

            return page;
        }

        public int GetNumberOfAbsenteeism(int lectureId, int studentId)
        {
            return _attendanceDbSet
                .Where(a => a.LectureId == lectureId && a.StudentId == studentId)
                .Count(a => a.Presence == false);
        }

        private int GetTotalPages(IQueryable<AttendanceDb> selectedAttendance, int pageSize)
        {
            var count = selectedAttendance.Count();
            return (int)Math.Ceiling(decimal.Divide(count, pageSize));
        }
    }
}
