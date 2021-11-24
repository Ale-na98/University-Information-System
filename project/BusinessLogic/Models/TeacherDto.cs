using DataAccess;
using System.Collections.Generic;

namespace BusinessLogic
{
    public record TeacherDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}