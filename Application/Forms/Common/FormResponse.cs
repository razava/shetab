using Domain.Models.Relational.ReportAggregate;

namespace Application.Forms.Common;

public record FormResponse(Guid Id, string Title, IEnumerable<FormElementResponse> Elements)
{
    public static FormResponse? FromForm(Form? form)
    {
        if (form is null)
            return null;
        return new FormResponse(
                form.Id,
                form.Title,
                form.Elements.Select(fe => new FormElementResponse(
                    fe.ElementType,
                    fe.Name,
                    fe.Title,
                    fe.Order,
                    fe.Meta)));
    }
};
