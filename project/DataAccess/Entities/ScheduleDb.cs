namespace DataAccess.Entities
{
    public record ScheduleDb
    {
        public int LectureId { get; set; }
        public LectureDb Lecture { get; set; }

        public int GroupId { get; set; }
        public GroupDb Group { get; set; }
    }
}
