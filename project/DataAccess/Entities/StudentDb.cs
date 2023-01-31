using System.Collections.Generic;

namespace DataAccess.Entities
{
    public record StudentDb
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public GroupDb Group { get; set; }
        public int? GroupId { get; set; }

        public IList<HometaskDb> Hometasks { get; set; }
        public IList<AttendanceDb> Lectures { get; set; }
    }
}
