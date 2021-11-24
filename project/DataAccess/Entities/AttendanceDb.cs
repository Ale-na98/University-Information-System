using System;

namespace DataAccess
{
    public record AttendanceDb
    {
        public int LectureId { get; set; }
        public LectureDb Lecture { get; set; }
        public int StudentId { get; set; }
        public StudentDb Student { get; set; }
        public DateTime LectureDate { get; set; }
        public bool Presence { get; set; }
        public bool HometaskDone { get; set; }
    }
}