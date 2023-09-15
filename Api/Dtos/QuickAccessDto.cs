namespace Api.Dtos;

public class QuickAccessDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Order { get; set; }
    public int CategoryId { get; set; }
    public CategoryDto Category { get; set; }
    public IFormFile Image { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime Created { get; set; }
}
