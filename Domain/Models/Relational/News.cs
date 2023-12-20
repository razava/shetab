using Domain.Models.Relational.Common;

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

    private News()
    {
        
    }

    public static News Create(int InstanceId, string title, string description, string url, Media? image, bool isDeleted)
    {
        var news = new News()
        {
            ShahrbinInstanceId = InstanceId,
            Title = title,
            Description = description,
            Url = url,
            Image = image,
            IsDeleted = isDeleted,
            Created = DateTime.UtcNow
        };
        return news;
    }

    public void Update(string? title, string? description, string? url, Media? image, bool? isDeleted)
    {
        Title = title ?? Title;
        Description = description ?? Description;
        Url = url ?? Url;
        Image = image ?? Image;
        IsDeleted = isDeleted ?? IsDeleted;
    }
}
