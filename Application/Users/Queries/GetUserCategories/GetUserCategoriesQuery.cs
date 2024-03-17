﻿namespace Application.Users.Queries.GetUserCategories;

public record GetUserCategoriesQuery(string UserId) : IRequest<Result<List<int>>>;

