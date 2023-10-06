using Domain.Models.Relational.Common;

namespace Domain.Models.Relational;

public class BaseModel
{
    public int ShahrbinInstanceId { get; set; }
    public ShahrbinInstance ShahrbinInstance { get; set; } = null!;
}
