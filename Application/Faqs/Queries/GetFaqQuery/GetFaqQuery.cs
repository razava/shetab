﻿using Domain.Models.Relational;
using MediatR;

namespace Application.Faqs.Queries.GetFaqQuery;

public record GetFaqQuery(int InstanceId) : IRequest<List<Faq>>;
