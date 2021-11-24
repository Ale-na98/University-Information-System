using System;
using System.Collections.Generic;

namespace DataAccess
{
    public interface IAttendanceRepository : IUniversityRepository<AttendanceDb>
    {
        void Create(AttendanceDb attendance);
        AttendanceDb Get(int lectureId, int studentId, DateTime lectureDate);
        ICollection<AttendanceDb> GetLectureAttendance(string lectureName);
        ICollection<AttendanceDb> GetStudentAttendance(string studentFullName);
        int GetNumberOfAbsenteeism(int lectureId, int studentId);
    }
}