using System.Collections.Generic;

namespace BusinessLogic
{
    public record LectureAttendanceDto
    {
        public string Name { get; set; }
        public List<AttendanceReportDto> Attendance { get; set; }
    }
}