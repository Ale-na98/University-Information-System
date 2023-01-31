using System;
using DataAccess.Entities;

namespace DataAccess.Interfaces
{
    public interface IAttendanceRepository : IUniversityRepository<AttendanceDb>
    {
        public AttendanceDb GetWithLecture(int id);
        Page<AttendanceDb> GetByFilter(int? groupId, int? studentId, int? lectureId,
            DateTime? lectureDateFrom, DateTime? lectureDateTo, PageParams parameters);
        int GetNumberOfAbsenteeism(int lectureId, int studentId);
    }
}
