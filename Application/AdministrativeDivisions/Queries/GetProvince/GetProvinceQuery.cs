﻿using Domain.Models.Relational.Common;
using MediatR;

namespace Application.AdministrativeDivisions.Queries.GetProvince;

public sealed record GetProvinceQuery() : IRequest<List<Province>>;

