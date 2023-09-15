namespace Api.Dtos
{
    public class ComplaintFinishDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public ICollection<IFormFile> Attachments { get; set; }
    }
}
