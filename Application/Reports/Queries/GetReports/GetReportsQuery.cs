﻿using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using MediatR;

namespace Application.Reports.Queries.GetReports;

public sealed record GetReportsQuery(
    PagingInfo PagingInfo,
    string userId,
    int instanceId) : IRequest<PagedList<Report>>;

