using Domain.Models.Relational.Common;
using Domain.Primitives;

namespace Domain.Models.Relational.ReportAggregate;

public class FormElement : Entity
{
    public FormElement(Guid id) : base(id) { }
    
    public string ElementType { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public int Order { get; private set; }
    public string Meta { get; private set; } = string.Empty;

    public static FormElement Create(string elementType, string name, string title, int order, string meta)
    {
        return new FormElement(Guid.NewGuid())
        {
            ElementType = elementType,
            Name = name,
            Title = title,
            Order = order,
            Meta = meta
        };
    }
}

public class Form : Entity
{
    private Form(Guid id): base(id)
    {
    }

    public static Form Create(string title, List<FormElement> elements)
    {
        return new Form(Guid.NewGuid())
        {
            Title = title,
            Elements = elements
        };
    }
    public string Title { get; private set; } = string.Empty;
    public DateTime Created { get; private set; }
    public List<FormElement> Elements { get; private set; } = new List<FormElement>();
    public List<Category> Categories { get; private set; } = new List<Category>();
}