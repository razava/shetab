using System.ComponentModel.DataAnnotations.Schema;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ProcessAggregate;
using Domain.Models.Relational.ReportAggregate;

namespace Domain.Models.Relational;

public class Category : BaseModel
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
    public int? ParentId { get; set; }
    public Category? Parent { get; set; }
    public int? ProcessId { get; set; }
    public Process? Process { get; set; }
    public ICollection<Category> Categories { get; set; } = new List<Category>();
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

    //TODO: Make constructor private
    //private Category()
    //{
        
    //}

    public static Category Create(
        int instanceId,
        string code,
        string title,
        string description,
        int order,
        int parentId,
        int duration,
        int responseDuration,
        int? processId = null,
        bool isDeleted = false,
        bool objectionAllowed = true,
        bool edittingAllowed = true,
        bool hideMap = false,
        string attachmentDescription = "",
        List<FormElement>? formElements = null)
    {
        var category = new Category()
        {
            ShahrbinInstanceId = instanceId,
            Code = code,
            Title = title,
            Description = description,
            Order = order,
            ParentId = parentId,
            Duration = duration,
            ResponseDuration = responseDuration,
            ProcessId = processId,
            IsDeleted = isDeleted,
            ObjectionAllowed = objectionAllowed,
            EditingAllowed = edittingAllowed,
            HideMap = hideMap,
            AttachmentDescription = attachmentDescription,
            FormElements = formElements ?? new List<FormElement>()
        };

        return category;
    }

    public void Update(
        string? code = null,
        string? title = null,
        string? description = null,
        int? order = null,
        int? parentId = null,
        int? duration = null,
        int? responseDuration = null,
        int? processId = null,
        bool? isDeleted = null,
        bool? objectionAllowed = null,
        bool? edittingAllowed = null,
        bool? hideMap = null,
        string? attachmentDescription = null,
        List<FormElement>? formElements = null)
    {
        Code = code ?? Code;
        Title = title ?? Title;
        Description = description ?? Description;
        Order = order ?? Order;
        ParentId = parentId ?? ParentId;
        Duration = duration ?? Duration;
        ResponseDuration = responseDuration ?? ResponseDuration;
        ProcessId = processId ?? ProcessId;
        IsDeleted = isDeleted ?? IsDeleted;
        ObjectionAllowed = objectionAllowed ?? ObjectionAllowed;
        EditingAllowed = edittingAllowed ?? EditingAllowed;
        HideMap = hideMap ?? HideMap;
        AttachmentDescription = attachmentDescription ?? AttachmentDescription;
        FormElements = formElements ?? new List<FormElement>();
    }
}
