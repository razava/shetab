﻿using Application.Info.Common;

namespace Application.Info.Queries.GetInfoQuery;

public record GetInfoQuery(
    int Code,
    int InstanceId,
    string UserId,
    string? Parameter)
    : IRequest<Result<InfoModel>>;

