namespace BusinessLogic.Domain
{
    public record Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Group Group { get; set; }
    }
}
