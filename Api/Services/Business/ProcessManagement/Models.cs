using Domain.Models.Relational;

namespace Api.Services.Business.ProcessManagement;

public record ReportCreateModel(
    int CategoryId,
    string Comments,
    AddressInfo Address,
    ICollection<Guid> Attachments,
    bool IsIdentityVisible = true,
    bool IsPublic = true);

public record ReportUpdateModel(
    int? CategoryId,
    string? Comments,
    AddressInfo? Address,
    ICollection<Guid>? Attachments,
    bool? IsPublic = true);

public record ReportAcceptModel(
    int? CategoryId,
    string? Comments,
    AddressInfo? Address,
    ICollection<Guid>? Attachments,
    bool? IsPublic = true);


public record AddressInfo(
    int RegionId,
    string Street,
    string Valley,
    string Detail,
    string Number,
    string PostalCode,
    double Latitude,
    double Longitude,
    double? Elevation = null);

public record ReplyCitizenModel(
    string ActorIdentifier,
    ActorType ActorType,
    List<Guid> Attachments,
    string Comment,
    bool IsPublic,
    string Message);
