namespace Api.Dtos;

public class ComplaintInsertDto
{
    public int CategoryId { get; set; }
    public string Description { get; set; } = string.Empty;
    public int InstanceId { get; set; }
    public ICollection<IFormFile> Attachments { get; set; }

}

