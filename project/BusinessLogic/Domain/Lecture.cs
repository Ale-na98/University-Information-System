namespace BusinessLogic.Domain
{
    public record Lecture
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? TeacherId { get; set; }
    }
}
