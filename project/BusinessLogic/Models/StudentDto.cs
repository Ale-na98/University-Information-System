using System.Collections.Generic;

namespace BusinessLogic
{
    public record StudentDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}