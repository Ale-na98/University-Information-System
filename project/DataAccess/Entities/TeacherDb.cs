using System.Collections.Generic;

namespace DataAccess.Entities
{
    public record TeacherDb
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public IList<LectureDb> Lectures { get; set; }
    }
}
