using System;

namespace DataAccess.Entities
{
    public record AttendanceDb
    {
        public int Id { get; set; }

        public int LectureId { get; set; }
        public LectureDb Lecture { get; set; }

        public int StudentId { get; set; }
        public StudentDb Student { get; set; }

        public DateTime LectureDate { get; set; }
        public bool Presence { get; set; }
        public bool HometaskDone { get; set; }
    }
}
