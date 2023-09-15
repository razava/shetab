using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Relational;

public class Category : BaseModel
{
    public int Id { get; set; }
    public int Order { get; set; }
    public int? ParentId { get; set; }
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public Category? Parent { get; set; }
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public int? ProcessId { get; set; }
    public Process? Process { get; set; }
    public string Description { get; set; } = string.Empty;
    public string AttachmentDescription { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int? ResponseDuration { get; set; }
    public CategoryType CategoryType { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<FormElement> FormElements { get; set; } = new List<FormElement>();
    public bool ObjectionAllowed { get; set; }
    public bool EditingAllowed { get; set; } = true;
    public bool HideMap { get; set; }

    [NotMapped]
    public List<int> Siblings
    {
        get
        {
            if (siblings == null)
            {
                siblings = getSiblings();
                return siblings;
            }
            else
            {
                return siblings;
            }
        }
    }
    [NotMapped]
    private List<int>? siblings = null;
    private List<int> getSiblings()
    {
        var result = new List<int>();
        var buffer = new Queue<Category>();
        buffer.Enqueue(this);
        Category currentNode;
        do
        {
            currentNode = buffer.Dequeue();
            result.Add(currentNode.Id);
            if (currentNode.Categories is null)
                continue;
            foreach (var item in currentNode.Categories)
            {
                buffer.Enqueue(item);
            }
        } while (buffer.Count > 0);
        return result;
    }
}
