using Domain.Models.Relational.Common;

namespace Domain.Models.Relational;

public class QuickAccess : BaseModel
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int Order { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public Media Media { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTime Created { get; set; }
}
