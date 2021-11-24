using System;

namespace BusinessLogic
{
    public record HometaskDto
    {
        public int Id { get; set; }
        public DateTime HometaskDate { get; set; }
        public int? Mark { get; set; }

        public int StudentId { get; set; }
        public int LectureId { get; set; }
    }
}