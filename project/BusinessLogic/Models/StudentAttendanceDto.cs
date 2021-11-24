using System.Collections.Generic;

namespace BusinessLogic
{
    public record StudentAttendanceDto
    {
        public string FullName { get; set; }
        public List<AttendanceReportDto> Attendance { get; set; }
    }
}