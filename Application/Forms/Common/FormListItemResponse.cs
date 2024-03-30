using Domain.Models.Relational.ReportAggregate;
using System.Linq.Expressions;

namespace Application.Forms.Common;

public record FormListItemResponse(Guid Id, string Title)
{
    public static Expression<Func<Form, FormListItemResponse>> GetSelector()
    {
        Expression<Func<Form, FormListItemResponse>> selector =
        form => new FormListItemResponse(
            form.Id,
            form.Title);

        return selector;
    }
    
}


