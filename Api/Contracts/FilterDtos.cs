using Domain.Models.Relational.Common;
using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;


public record QueryFilter(
    [MaxLength(64)]
    string? Query);


public record TimeFilter(
    DateTime? SentFromDate,
    DateTime? SentToDate);



public record FilterGetReports(
    DateTime? SentFromDate,
    DateTime? SentToDate,
    List<ReportState>? CurrentStates,
    //[MaxLength(64)] this query is for searching TrackingNumbers
    [MinLength(3)] [MaxLength(16)]
    string? Query);


public record FilterGetAllReports(
    DateTime? SentFromDate,
    DateTime? SentToDate,
    //todo : annotation for strings?
    List<string>? RoleNames,
    List<int>? CategoryIds,
    List<int>? RegionIds,
    List<ReportState>? CurrentStates,
    bool? HasSatisfaction,  //?........todo : report model haven't satisfaction field
    int? MinSatisfaction,   //?...........
    int? MaxSatisfaction,  //?.............
    [MaxLength(64)]
    string? Query);



public record FilterGetCommentViolation(
    DateTime? SentFromDate,
    DateTime? SentToDate,
    List<int>? CategoryIds,
    [MaxLength(64)]
    string? Query);


public record FilterGetUsers(
    DateTime? SentFromDate,
    DateTime? SentToDate,
    List<string>? RoleNames,
    List<int>? RegionIds,
    [MaxLength(64)]
    string? Query);





