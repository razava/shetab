﻿using Api.Abstractions;
using Api.Contracts;
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
        await Task.CompletedTask;
        return Ok("Not Implemented");
    }


    [Authorize]
    [HttpGet("Charts/{id}")]
    public async Task<ActionResult<InfoDto>> GetChartsById(int id/*, [FromQuery] PagingInfo pagingInfo*/)//time filter?
    {
        await Task.CompletedTask;
        return Ok("Not Implemented");
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
