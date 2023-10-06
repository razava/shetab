using Domain.Models.Relational.Common;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models.Relational;

public class News : BaseModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Url { get; set; } = null!;
    public Media? Image { get; set; }
    public DateTime Created { get; set; }
    public bool IsDeleted { get; set; }
}
