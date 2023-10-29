﻿using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetCitizenReportById;

public sealed record GetCitizenReportByIdQuery(
    Guid id) : IRequest<Report>;

