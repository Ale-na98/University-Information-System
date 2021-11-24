using System;

namespace BusinessLogic
{
    public record AttendanceDto
    {
        public int LectureId { get; set; }
        public int StudentId { get; set; }

        public DateTime LectureDate { get; set; }
        public bool Presence { get; set; }
        public bool HometaskDone { get; set; }
    }
}