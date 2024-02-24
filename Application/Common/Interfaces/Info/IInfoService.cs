using Application.Info.Common;

namespace Application.Common.Interfaces.Info;

public interface IInfoService
{
    Task<InfoModel> GetReportsStatusPerCategory(int instanceId, int? parentCategoryId);
}
