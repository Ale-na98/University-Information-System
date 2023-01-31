using System.Collections.Generic;

namespace DataAccess.Entities
{
    public record LectureDb
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public TeacherDb Teacher { get; set; }
        public int? TeacherId { get; set; }

        public IList<HometaskDb> Hometasks { get; set; }
        public IList<AttendanceDb> Students { get; set; }
        public IList<ScheduleDb> Schedule { get; set; }
    }
}
