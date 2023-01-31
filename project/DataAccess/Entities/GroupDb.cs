using System.Collections.Generic;

namespace DataAccess.Entities
{
    public record GroupDb
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IList<StudentDb> Students { get; set; }
        public IList<ScheduleDb> Schedule { get; set; }
    }
}
