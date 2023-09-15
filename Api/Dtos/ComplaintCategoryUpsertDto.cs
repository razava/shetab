namespace Api.Dtos
{
    public class ComplaintCategoryUpsertDto
    {
        public string Title { get; set; } = null!;
        public int? ParentId { get; set; }

    }
}
