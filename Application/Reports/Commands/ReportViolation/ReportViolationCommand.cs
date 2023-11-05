﻿using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Commands.ReportViolation;

public sealed record ReportViolationCommand(
    Guid ReportId,
    string UserId,
    int ViolationTypeId,
    string Description) : IRequest<Violation>;

