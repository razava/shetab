﻿using Domain.Models.MyYazd;

namespace Application.Common.Interfaces.MyYazd;

public interface IMyYazdService
{
    Task<Result<MyYazdUserInfo>> GetUserInfo(string code);
}
