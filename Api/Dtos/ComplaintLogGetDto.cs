using Domain.Models.Relational.Common;

namespace Api.Dtos;

public class ComplaintLogGetDto
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public ApplicationUserDto Actor { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ICollection<Media> Medias { get; set; } = new List<Media>();
}
