﻿using Application.Reports.Common;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Commands.UpdateByOperator;

public sealed record UpdateByOperatorCommand(
    Guid reportId,
    string operatorId,
    int? CategoryId,
    string? Comments,
    AddressInfoRequest? Address,
    List<Guid>? Attachments,
    bool? IsPublic = true) : IRequest<Result<GetReportByIdResponse>>;

