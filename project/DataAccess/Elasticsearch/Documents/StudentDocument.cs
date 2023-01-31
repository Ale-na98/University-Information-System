namespace DataAccess.Elasticsearch.Documents
{
    public record StudentDocument
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string GroupName { get; set; }
    }
}
