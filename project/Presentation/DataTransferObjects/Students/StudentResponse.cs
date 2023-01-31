namespace Presentation.DataTransferObjects.Students
{
    public record StudentResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string GroupName { get; set; }
    }
}
