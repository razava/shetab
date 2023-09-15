namespace Api.Dtos
{
    public class ComplaintFeedbackDto
    {
        public Guid Id { get; set; }
        public bool IsObjection { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public ICollection<IFormFile> Attachments { get; set; }
    }
}
