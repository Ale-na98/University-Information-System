namespace BusinessLogic
{
    public record LectureDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? TeacherId { get; set; }
    }
}