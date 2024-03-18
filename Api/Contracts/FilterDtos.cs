using System.ComponentModel.DataAnnotations;

namespace Api.Contracts;


public record QueryFilter(
    [MaxLength(64)]
    string? Query);


public record TimeFilter(
    DateTime SentFromDate,
    DateTime? SentToDate);



public record FilterGetCommentViolation(
    DateTime? SentFromDate,
    DateTime? SentToDate,
    List<int>? CategoryIds,
    [MinLength(3)] [MaxLength(16)]
    string? Query);


