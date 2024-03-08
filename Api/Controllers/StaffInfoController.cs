using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Info;
using Application.Info.Queries.GetInfoQuery;
using Application.Info.Queries.GetListChartQuery;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class StaffInfoController : ApiController
{
    public StaffInfoController(ISender sender) : base(sender)
    {
    }


    [Authorize]
    [HttpGet("ListChart")]
    public async Task<ActionResult<List<ChartDto>>> GetListChart()
    {
        var userRoles = User.GetUserRoles();
        var instanceId = User.GetUserInstanceId();
        var query = new GetListChartQuery(instanceId, userRoles);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<List<ChartDto>>()),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("Charts/{code}")]
    public async Task<ActionResult<InfoDto>> GetChartsById(
        int code,
        string? parameter,
        [FromQuery] List<double>? geometry,
        [FromQuery] List<ReportsToInclude>? reportsToInclude,
        [FromQuery] ReportFilters reportFilters)
    {
        var instanceId = User.GetUserInstanceId();
        var userId = User.GetUserId();
        var userRoles = User.GetUserRoles();
        List<GeoPoint>? geoPoints = null;
        if(geometry is not null)
        {
            if(geometry.Count%2 == 0)
            {
                geoPoints = new List<GeoPoint>();
                for(var i=0; i<geometry.Count; i += 2)
                {
                    geoPoints.Add(new GeoPoint(geometry[i], geometry[i+1]));
                }
            }
        }
        var query = new GetInfoQuery(code, instanceId, userId, userRoles, parameter, geoPoints, reportsToInclude, reportFilters);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    //............................................
    //todo : is this endpoint used??
    [Authorize]
    [HttpGet("Excel")]
    public async Task<ActionResult> GetExcel()
    {
        await Task.CompletedTask;
        return Ok("Not Implemented");
    }

}
