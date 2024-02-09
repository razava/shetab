using Domain.Models.Relational.ReportAggregate;

namespace Application.Forms.Common;

public record FormElementModel(
    string ElementType,
    string Name,
    string Title,
    int Order,
    string Meta)
{
    public FormElement GetFormElement()
    {
        return FormElement.Create(ElementType, Name, Title, Order, Meta);
    }
}