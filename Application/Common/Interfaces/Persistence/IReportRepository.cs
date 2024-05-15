using Application.Info.Queries.GetInfo;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ProcessAggregate;
using Domain.Models.Relational.ReportAggregate;
using System.Linq.Expressions;

namespace Application.Common.Interfaces.Persistence;

public interface IReportRepository : IGenericRepository<Report>
{
    public Task<Report?> GetByIDAsync(Guid id, bool trackChanges = true);
    public Task<T?> GetByIdSelective<T>(
        Guid id, Expression<Func<Report, bool>> filter, Expression<Func<Report, T>> selector);
    public Task<PagedList<T>> GetCitizenReports<T>(
        string userId, int? instanceId, Expression<Func<Report, T>> selector, PagingInfo pagingInfo);
    public Task<PagedList<T>> GetRecentReports<T>(
        List<string> roles, Expression<Func<Report,
            bool>> filter,
        Expression<Func<Report, T>> selector,
        PagingInfo pagingInfo);

    public Task<PagedList<T>> GetReports<T>(
        int instanceId,
        string userId,
        List<string> roles,
        string? fromRoleId,
        Expression<Func<Report, T>> selector,
        ReportFilters reportFilters,
        PagingInfo pagingInfo);

    public Task<PagedList<T>> GetReportComments<T>(
        Guid reportId, Expression<Func<Comment, T>> selector, PagingInfo pagingInfo);
    public Task<List<HistoryResponse>> GetReportHistory(Guid id);
    public Task<PagedList<T>> GetNearest<T>(
        int instanceId,
        double longitude,
        double latitude,
        Expression<Func<Report, T>> selector,
        PagingInfo pagingInfo);

    public Task<List<PossibleSourceResponse>> GetPossibleSources(string userId, List<string> roleNames);
    public Task<List<PossibleTransitionResponse>> GetPossibleTransition(Guid reportId, string userId);
    public Task<int> GetInstanceId(Guid reportId);
}



public class HistoryResponse
{
    public Guid Id { get; set; }
    public DateTime DateTime { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public List<Media> Attachments { get; set; } = new List<Media>();
    public ProcessReason Reason { get; set; } = default!;
    public ActorType ActorType { get; set; }
    public string ActorIdentifier { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public ActorResponse Actor { get; set; } = default!;   //todo : Handle from Application Layer
}

public class ActorResponse
{
    //todo : is this correct that set ? for fix warnings here?
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Title { get; set; }
    public string DisplayName { get { return Title + ((FirstName + LastName).Length > 0 ? " (" + FirstName + " " + LastName + ")" : ""); } }
}

//....
public sealed record PossibleSourceResponse(string RoleId, string RoleName, string RoleTitle, bool CanRefer = true);

public class PossibleTransitionResponse
{
    public string StageTitle { get; set; } = string.Empty;
    public int TransitionId { get; set; }
    public IEnumerable<ProcessReasonResponse> ReasonList { get; set; } = new List<ProcessReasonResponse>();
    public IEnumerable<ActorWithName> Actors { get; set; } = new List<ActorWithName>();
    public bool CanSendMessageToCitizen { get; set; }
    public TransitionType TransitionType { get; set; }
}


public class ActorWithName
{
    public int Id { get; set; }
    public string Identifier { get; set; } = null!;
    public ActorType Type { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string DisplayName
    {
        get { return Title + ((FirstName + LastName).Length > 0 ? " (" + FirstName + " " + LastName + ")" : ""); }
    }
    public string Organization { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}

public record ProcessReasonResponse(int Id, string Title, string Description, ReasonMeaning ReasonMeaning);


