using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
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
        var mappedResult = result.Adapt<List<ChartDto>>();
        return Ok(mappedResult);
    }


    [Authorize]
    [HttpGet("Charts/{id}")]
    public async Task<ActionResult<InfoDto>> GetChartsById(int id/*, [FromQuery] PagingInfo pagingInfo*/)//time filter?
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetInfoQuery(id, instanceId);
        var result = await Sender.Send(query);
        return Ok(result);
    }

    /*
    [Authorize]
    [HttpGet("Report")]
    public async Task<ActionResult> GetAllReports()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize]
    [HttpGet("Report/{id}")]
    public async Task<ActionResult> GetReportById()
    {
        await Task.CompletedTask;
        return Ok();
    }
    */

    [Authorize]
    [HttpGet("Locations")]
    public async Task<ActionResult<List<LocationDto>>> GetLocations()
    {
        await Task.CompletedTask;
        return Ok("Not Implemented");
    }


    //todo : is this endpoint used??
    [Authorize]
    [HttpGet("Summary")]
    public async Task<ActionResult<InfoDto>> GetSummary()
    {
        await Task.CompletedTask;
        return Ok("Not Implemented");
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
