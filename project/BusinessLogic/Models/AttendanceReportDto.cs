using System;

namespace BusinessLogic
{
    public record AttendanceReportDto
    {
        public DateTime LectureDate { get; set; }
        public bool Presence { get; set; }
        public bool HometaskDone { get; set; }
    }
}