using System;

namespace BusinessLogic.Domain
{
    public record Attendance
    {
        public int Id { get; set; }
        public Lecture Lecture { get; set; }
        public int StudentId { get; set; }

        public DateTime LectureDate { get; set; }
        public bool Presence { get; set; }
        public bool HometaskDone { get; set; }
    }
}
