using System;

namespace DataAccess
{
    public record HometaskDb
    {
        public int Id { get; set; }
        public DateTime HometaskDate { get; set; }
        public byte Mark { get; set; }

        public StudentDb Student { get; set; }
        public int StudentId { get; set; }

        public LectureDb Lecture { get; set; }
        public int LectureId { get; set; }
    }
}