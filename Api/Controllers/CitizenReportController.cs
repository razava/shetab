using Api.Abstractions;
using Api.Contracts;
using Api.Dtos;
using Application.Common.Interfaces.Persistence;
using Application.Reports.Commands.CreateReportByCitizen;
using Application.Reports.Common;
using Application.Reports.Queries.GetRecentReports;
using Domain.Models.Relational;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class CitizenReportController : ApiController
{
    public CitizenReportController(ISender sender) : base(sender)
    {
    }

    //todo : Define & Set Dtos


    [Authorize(Roles = "Citizen")]
    [HttpGet]
    public async Task<ActionResult<Report>> GetReports([FromQuery] PagingInfo pagingInfo, int instanceId)
    {
        var query = new GetRecentReportsQuery(pagingInfo, instanceId);
        var result = await Sender.Send(query);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.Meta));
        return Ok();
    }

    //todo : remove ,cause i think that not used.
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
    public async Task<ActionResult<Report>> GetLocations(int instanceId/*??*/)
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
    public async Task<ActionResult<Report>> GetMyReports([FromQuery] PagingInfo pagingInfo, int instanceId)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet("Mine/{id:Guid}")]
    public async Task<ActionResult<Report>> GetMyReportById(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    [HttpPost]
    public async Task<ActionResult<Report>> CreateReport(int instanceId, [FromForm] CreateReportDto model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (userId == null || username == null)
        {
            return Unauthorized();
        }

        var phoneNumber = username;
        var addressInfo = new AddressInfo(
            model.Address.RegionId!.Value,
            model.Address.Street,
            model.Address.Valley,
            model.Address.Detail,
            model.Address.Number,
            model.Address.PostalCode,
            model.Address.Latitude!.Value,
            model.Address.Longitude!.Value);

        var command = new CreateReportByCitizenCommand(
            instanceId,
            userId,
            phoneNumber,
            model.CategoryId,
            model.Comments,
            addressInfo,
            model.Attachments,
            model.IsIdentityVisible);
        var report = await Sender.Send(command);

        //TODO: Fix this ::::> is this okay?
        return CreatedAtAction(nameof(GetMyReportById), report.Id, report);
    }


    //this could be in Common controller
    [Authorize(Roles = "Citizen")]
    [HttpGet("QuickAccesses")]
    public async Task<ActionResult> GetQuickAccesses()
    {
        await Task.CompletedTask;
        return Ok();
    }
    

    [Authorize(Roles = "Citizen")]
    // Post changed to Put and get isLiked param for doing Like & UnLike operation in same endpoint like old version.
    //in old version returns int for report Likes number. 
    [HttpPut("Like/{id:Guid}")]  
    public async Task<ActionResult> Like(Guid id, bool isLiked)
    {
        await Task.CompletedTask;
        return Ok();
    }

     
    [Authorize(Roles = "Citizen")]
    [HttpPost("Comment/{id:Guid}")]
    public async Task<ActionResult> CreateComment(int instanceId, Guid id, string comment)
    {
        await Task.CompletedTask;
        return Ok();
    }
    
    [Authorize(Roles = "Citizen")]
    [HttpGet("Comments/{id:Guid}")]
    public async Task<ActionResult> GetComments(Guid id, [FromQuery] PagingInfo pagingInfo)
    {
        await Task.CompletedTask;
        return Ok();
    }
    
    [Authorize(Roles = "Citizen")]
    [HttpDelete("Comment/{commentId:Guid}")]
    public async Task<ActionResult> DeleteComment(Guid commentId)
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Citizen")]
    [HttpPost("Feedback/{id:Guid}")]
    public async Task<ActionResult> Feedback(Guid id, CreateFeedbackDto createFeedbackDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //todo : post/feedbackFromApp ??


    //todo : define & set dto
    [Authorize(Roles = "Citizen")]
    [HttpPost("Violation")]
    public async Task<ActionResult> RepotrViolation()
    {
        await Task.CompletedTask;
        return Ok();
    }



}
