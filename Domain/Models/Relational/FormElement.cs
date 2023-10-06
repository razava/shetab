using Domain.Models.Relational.Common;

namespace Domain.Models.Relational.ReportAggregate;

public class FormElement
{
    public int Id { get; set; }
    public FormElementType FormElementType { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Hint { get; set; } = string.Empty;
    public string Default { get; set; } = string.Empty;
    public int DefaultId { get; set; }
    public string Url { get; set; } = string.Empty;
    public int Order { get; set; }
    public bool IsEditable { get; set; }
    public bool IsEnabled { get; set; } = true;
    public bool IsRequired { get; set; } = true;
    public int MaxLength { get; set; }
}
