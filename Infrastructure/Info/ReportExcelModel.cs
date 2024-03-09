using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using System.Linq.Expressions;

namespace Infrastructure.Info;

/*
 ws.Cell(rowNum, j++).Value = i;
                ws.Cell(rowNum, j++).Value = report.Citizen.PhoneNumber;
                ws.Cell(rowNum, j++).Value = report.Citizen.FirstName;
                ws.Cell(rowNum, j++).Value = report.Citizen.LastName;
                ws.Cell(rowNum, j++).Value = report.Registrant?.FirstName;
                ws.Cell(rowNum, j++).Value = report.Registrant?.LastName;
                ws.Cell(rowNum, j++).Value = $"{report.Executive?.Title} ({report.Executive?.FirstName} {report.Executive?.LastName})";
                ws.Cell(rowNum, j++).Value = report.Category.Parent?.Title;
                ws.Cell(rowNum, j++).Value = report.Category.Title;
                ws.Cell(rowNum, j++).Value = report.Address.Detail;
                ws.Cell(rowNum, j++).Value = report.TrackingNumber;
                ws.Cell(rowNum, j++).Value = report.Sent.GregorianToPersian();
                ws.Cell(rowNum, j++).Value = (report.Finished != null) ? report.Finished.Value.GregorianToPersian() : "";
                ws.Cell(rowNum, j++).Value = report.LastStatus;
                ws.Cell(rowNum, j++).Value = (report.LastReason != null) ? report.LastReason.Title : "";
                ws.Cell(rowNum, j++).Value = report.IsFeedbacked ? "بله" : "خیر";
                ws.Cell(rowNum, j++).Value = report.IsObjectioned ? "بله" : "خیر";
                ws.Cell(rowNum, j++).Value = report.Comments;
                var transitions = "";
                foreach (var tl in report.TransitionLogs)
                {
                    transitions += tl.DateTime.GregorianToPersian();
                    transitions += ":";
                    transitions += tl.Message;
                    transitions += "-";
                    transitions += tl.Comment;
                    transitions += "\r\n";
                }
                ws.Cell(rowNum, j++).Value = transitions;
 */
public record ReportExcelModel(
    Guid Id,
    string TrackingNumber,
    string Category,
    string Status,
    Priority Priority,
    PersonExcelModel Citizen,
    PersonExcelModel Registrant,
    PersonExcelModel CurrentActor,
    PersonExcelModel Executive,
    PersonExcelModel Contractor,
    string Comments,
    string Address,
    DateTime Sent,
    DateTime? Finished,
    DateTime? Responsed,
    string LastReason,
    bool IsFeedbacked,
    bool IsObjectioned,
    int? Rating
    )
{

    public static Expression<Func<Report, ReportExcelModel>> GetSelector()
    {
        Expression<Func<Report, ReportExcelModel>> selector
            = r => new ReportExcelModel(
                r.Id,
                r.TrackingNumber,
                r.Category.Title,
                r.LastStatus,
                r.Priority,
                new PersonExcelModel(r.Citizen.FirstName, r.Citizen.LastName, "", r.Citizen.PhoneNumber),
                new PersonExcelModel(r.Registrant.FirstName, r.Registrant.LastName, r.Registrant.Title, ""),
                new PersonExcelModel("", "", "", ""),
                new PersonExcelModel(r.Executive.FirstName, r.Executive.LastName, r.Executive.Title, ""),
                new PersonExcelModel(r.Contractor.FirstName, r.Contractor.LastName, r.Contractor.Title, ""),
                r.Comments,
                r.Address.Detail,
                r.Sent,
                r.Finished,
                r.Responsed,
                r.LastReason.Title,
                r.IsFeedbacked,
                r.IsObjectioned,
                r.Rating);
        return selector;
    }

}

public record PersonExcelModel(
    string FirstName,
    string LastName,
    string Title,
    string PhoneNumber);