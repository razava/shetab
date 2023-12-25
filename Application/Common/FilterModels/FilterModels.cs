using Domain.Models.Relational.Common;
using System.ComponentModel.DataAnnotations;

namespace Application.Common.FilterModels;

public record QueryFilterModel(
    string? Query);


public record TimeFilterModel(
    DateTime? SentFromDate,
    DateTime? SentToDate);


public record FilterGetReportsModel(
    DateTime? SentFromDate,
    DateTime? SentToDate,
    List<ReportState>? CurrentStates,
    string? Query);


public record FilterGetAllReportsModel(
    DateTime? SentFromDate,
    DateTime? SentToDate,
    List<string>? RoleNames,
    List<int>? CategoryIds,
    List<int>? RegionIds,
    List<ReportState>? CurrentStates,
    bool? HasSatisfaction,  //?
    int? MinSatisfaction,   //?
    int? MaxSatisfaction,  //?
    [MaxLength(64)]
    string? Query);


public record FilterGetCommentViolationModel(
    DateTime? SentFromDate,
    DateTime? SentToDate,
    List<int>? CategoryIds,
    string? Query);


public record FilterGetUsersModel(
    DateTime? SentFromDate,
    DateTime? SentToDate,
    List<string>? RoleNames,
    List<int>? RegionIds,
    string? Query);









