using System.Collections.Generic;

namespace DataAccess
{
    public record TeacherDb
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public ICollection<LectureDb> Lectures { get; set; }
    }
}