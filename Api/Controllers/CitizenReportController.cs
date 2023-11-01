using Api.Abstractions;
using Api.Dtos;
using Application.Common.Interfaces.Persistence;
using Application.Reports.Queries.GetRecentReports;
using Domain.Models.Relational;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class CitizenReportController : ApiController
{
    public CitizenReportController(ISender sender) : base(sender)
    {
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet]
    public async Task<ActionResult<Report>> GetReports([FromQuery] PagingInfo pagingInfo, int instanceId)
    {
        var query = new GetRecentReportsQuery(pagingInfo, instanceId);
        var result = await Sender.Send(query);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.Meta));
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<Report>> GetReportById(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet("Nearest")]
    public async Task<ActionResult<Report>> GetNearest([FromQuery]PagingInfo pagingInfo, int instanceId)
    {
        var query = new GetRecentReportsQuery(pagingInfo, instanceId);
        var result = await Sender.Send(query);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.Meta));
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet("Locations")]
    public async Task<ActionResult<Report>> GetLocations()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet("Locations/{id:Guid}")]
    public async Task<ActionResult<Report>> GetLocations(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet("Mine")]
    public async Task<ActionResult<Report>> GetMyReports()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet("Mine/{id:Guid}")]
    public async Task<ActionResult<Report>> GetMyReports(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    [HttpPost]
    public async Task<ActionResult<Report>> CreateReport(CreateReportDto model)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    // Post changed to Put and get isLiked param for doing Like & UnLike operation in same endpoint
    [HttpPut("Like/{id:Guid}")]  
    public async Task<ActionResult> Like(Guid id, bool isLiked)
    {
        await Task.CompletedTask;
        return Ok();
    }

     
    [Authorize(Roles = "Citizen")]
    [HttpPost("Comment/{id:Guid}")]
    public async Task<ActionResult> CreateComment(Guid id, string comment)
    {
        await Task.CompletedTask;
        return Ok();
    }
    
    [Authorize(Roles = "Citizen")]
    [HttpGet("Comment/{id:Guid}")]
    public async Task<ActionResult> GetComments(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }
    
    [Authorize(Roles = "Citizen")]
    [HttpDelete("Comment/{commentId:Guid}")]
    public async Task<IActionResult> DeleteComment(Guid commentId)
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Citizen")]
    [HttpPost("Feedback/{id:Guid}")]
    public async Task<ActionResult> Feedback(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //todo : post/feedbackFromApp ??



}
