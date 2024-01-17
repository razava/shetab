using Application.Common.Interfaces.Persistence;
using Application.Info.Common;
using DocumentFormat.OpenXml.InkML;
using Domain.Models.Relational;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Application.Info.Queries.GetInfoQuery;

internal class GetInfoQueryHandler : IRequestHandler<GetInfoQuery, InfoModel>
{
    private readonly IUnitOfWork _unitOfWork;
    public GetInfoQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<InfoModel> Handle(GetInfoQuery request, CancellationToken cancellationToken)
    {
        var code = request.Code % 100000;
        InfoModel result = new InfoModel();

        switch (code)
        {
            case 1:
                result = await GetUsersStatistics(request.InstanceId);
                break;
            case 2:
                result = await GetReportsStatistics(request.InstanceId);
                break;
            default:
                break;
        }



        return result;
    }



    private async Task<InfoModel> GetUsersStatistics(int instanceId)
    {
        var result = new InfoModel();
        var userContext = _unitOfWork.DbContext.Set<ApplicationUser>().AsNoTracking();

        var totalPersonel = await userContext.Where(e =>
        e.ShahrbinInstanceId != null && e.ShahrbinInstanceId == instanceId).LongCountAsync();

        var totalUsers = await userContext.Where(u => u.ShahrbinInstanceId == null).LongCountAsync();

        //var now = DateTime.UtcNow.Date;
        var now = DateTime.UtcNow.Date;
        //???????????????????????????????? citizen or staff :  ???
        var lastDay = await userContext.Where(p => p.VerificationSent >= now.Subtract(new TimeSpan(1, 0, 0, 0)))
            .LongCountAsync();

        var lastWeek = await userContext.Where(p => p.VerificationSent >= now.Subtract(new TimeSpan(7, 0, 0, 0)))
            .LongCountAsync();

        var lastMonth = await userContext.Where(p => p.VerificationSent >= now.Subtract(new TimeSpan(30, 0, 0, 0)))
            .LongCountAsync();

        result.Add(new InfoSingleton(
            totalPersonel.ToString(),
            "کل پرسنل",
            null));

        result.Add(new InfoSingleton(
            totalUsers.ToString(),
            "کل شهروندان",
            null));

        result.Add(new InfoSingleton(
            lastDay.ToString(),
            "روز گذشته",
            null));

        result.Add(new InfoSingleton(
            lastWeek.ToString(),
            "هفته گذشته",
            null));

        result.Add(new InfoSingleton(
            lastMonth.ToString(),
            "ماه گذشته",
            null));

        return result;

    }
     

    private async Task<InfoModel> GetReportsStatistics(int instanceId)
    {
        var result = new InfoModel();
        var query = _unitOfWork.DbContext.Set<Report>().AsNoTracking().Where(r => r.ShahrbinInstanceId == instanceId);

        var totalReports = await query.LongCountAsync();

        var now = DateTime.UtcNow;
        var lastDay = await query.Where(p => p.Sent >= now.Subtract(new TimeSpan(1, 0, 0, 0))).LongCountAsync();
        var lastWeek = await query.Where(p => p.Sent >= now.Subtract(new TimeSpan(7, 0, 0, 0))).LongCountAsync();
        var lastMonth = await query.Where(p => p.Sent >= now.Subtract(new TimeSpan(30, 0, 0, 0))).LongCountAsync();

        result.Add(new InfoSingleton(
            totalReports.ToString(),
            "کل درخواست ها",
            null));

        result.Add(new InfoSingleton(
            lastDay.ToString(),
            "روز گذشته",
            null));

        result.Add(new InfoSingleton(
            lastWeek.ToString(),
            "هفته گذشته",
            null));

        result.Add(new InfoSingleton(
            lastMonth.ToString(),
            "ماه گذشته",
            null));

        return result;
    }

    private async Task<InfoModel> GetTimeStatistics(int instanceId)
    {
        return new InfoModel();
    }



}
