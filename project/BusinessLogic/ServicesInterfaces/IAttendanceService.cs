using System;
using System.Collections.Generic;

namespace BusinessLogic
{
    public interface IAttendanceService
    {
        void SetPresence(AttendanceDto attendanceDto);
        AttendanceDto Get(int lectureId, int studentId, DateTime lectureDate);
        IReadOnlyCollection<StudentAttendanceDto> CreateLectureAttendanceReport(string lectureName);
        IReadOnlyCollection<LectureAttendanceDto> CreateStudentAttendanceReport(string studentFullName);
    }
}