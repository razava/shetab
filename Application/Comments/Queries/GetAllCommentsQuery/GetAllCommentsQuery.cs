﻿using Application.Common.FilterModels;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational.ReportAggregate;
using MediatR;

namespace Application.Comments.Queries.GetAllCommentsQuery;

public record GetAllCommentsQuery(PagingInfo PagingInfo,
    int InstanceId,
    FilterGetCommentViolationModel? FilterModel = default!,
    bool IsSeen = false) : IRequest<PagedList<Comment>>;
