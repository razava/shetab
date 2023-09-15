namespace Api.Dtos
{
    public class ComplaintReferDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int DeadlineInMinutes { get; set; }
        public int ReferToId { get; set; }
        public ICollection<IFormFile> Attachments { get; set; }
    }
}
