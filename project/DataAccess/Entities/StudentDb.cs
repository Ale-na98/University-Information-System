using System.Collections.Generic;

namespace DataAccess
{
    public record StudentDb
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<HometaskDb> Hometasks { get; set; }
        public ICollection<AttendanceDb> Lectures { get; set; }
    }
}