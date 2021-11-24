using System;
using System.Collections.Generic;

namespace DataAccess
{
    public record LectureDb
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TeacherDb Teacher { get; set; }
        public int? TeacherId { get; set; }

        public ICollection<HometaskDb> Hometasks { get; set; }
        public ICollection<AttendanceDb> Students { get; set; }
    }
}