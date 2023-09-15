namespace Api.Dtos;

public class ComplaintCategoryGetDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public int? ParentId { get; set; }
    public List<ComplaintCategoryGetDto> Children { get; set; } = new List<ComplaintCategoryGetDto>();
}
