using Api.Abstractions;
using Api.Contracts;
using Api.Dtos;
using Api.ExtensionMethods;
using Application.Comments.Commands.CreateComment;
using Application.Comments.Commands.DeleteComment;
using Application.Common.Interfaces.Persistence;
using Application.Configurations.Queries.QuickAccesses;
using Application.Reports.Commands.CreateReportByCitizen;
using Application.Reports.Commands.Like;
using Application.Reports.Commands.ReportViolation;
using Application.Reports.Common;
using Application.Reports.Queries.GetCitizenReportById;
using Application.Reports.Queries.GetNearestReports;
using Application.Reports.Queries.GetRecentReports;
using Application.Reports.Queries.GetUserReports;
using Domain.Models.Relational;
using Mapster;
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

    //todo : Define & Set Dtos for Location Get Endpoints


    [Authorize(Roles = "Citizen")]
    [HttpGet]
    public async Task<ActionResult<List<CitizenGetReportListDto>>> GetReports([FromQuery] PagingInfo pagingInfo, int instanceId)
    {
        var query = new GetRecentReportsQuery(pagingInfo, instanceId);
        var result = await Sender.Send(query);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.Meta));
        var mappedResult = result.Adapt<List<CitizenGetReportListDto>>();
        return Ok(mappedResult);
    }


    //todo : review returning Dto....................................
    //citizen can see review processes of other's reports.
    [Authorize(Roles = "Citizen")]
    [HttpGet("ReportHistory/{id:Guid}")]
    public async Task<ActionResult<List<TransitionLogDto>>> GetReportHistoryById(Guid id)
    {
        await Task.CompletedTask;//...............................
        return Ok();
    }


    [Authorize(Roles = "Citizen")]
    [HttpGet("Nearest")]
    public async Task<ActionResult<List<CitizenGetReportListDto>>> GetNearest([FromQuery]PagingInfo pagingInfo, int instanceId, [FromQuery] LocationDto locationDto)
    {
        var query = new GetNearestReportsQuery(pagingInfo, instanceId, locationDto.Longitude, locationDto.Latitude);
        var result = await Sender.Send(query);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.Meta));
        var mappedResult = result.Adapt<List<CitizenGetReportListDto>>();
        return Ok(mappedResult);
    }

    //todo : define & set dtos
    [Authorize(Roles = "Citizen")]
    [HttpGet("Locations")]
    public async Task<ActionResult> GetLocations(int instanceId/*??*/)
    {
        await Task.CompletedTask;//.......................
        return Ok();
    }

    //todo : check usage
    //todo : define & set dtos
    [Authorize(Roles = "Citizen")]
    [HttpGet("Locations/{id:Guid}")]
    public async Task<ActionResult> GetLocations(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Citizen")]
    [HttpGet("Mine")]
    public async Task<ActionResult<List<CitizenGetReportListDto>>> GetMyReports([FromQuery] PagingInfo pagingInfo, int instanceId)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        var query = new GetUserReportsQuery(pagingInfo, userId);
        var result = await Sender.Send(query);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.Meta));
        var mappedResult = result.Adapt<List<CitizenGetReportListDto>>();
        return Ok(mappedResult);
    }


    [Authorize(Roles = "Citizen")]
    [HttpGet("Mine/{id:Guid}")]
    public async Task<ActionResult<CitizenGetReportDetailsDto>> GetMyReportById(Guid id, int instanceId)
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized();

        var query = new GetCitizenReportByIdQuery(id, userId);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<CitizenGetReportDetailsDto>();
        return Ok(mappedResult);
    }


    [Authorize(Roles = "Citizen")]
    [HttpPost]
    public async Task<ActionResult> CreateReport(int instanceId, [FromForm] CitizenCreateReportDto model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (userId == null || username == null)
        {
            return Unauthorized();
        }

        var phoneNumber = username;
        var addressInfo = new AddressInfo(
            model.Address.RegionId,
            model.Address.Street,
            model.Address.Valley,
            model.Address.Detail,
            model.Address.Number,
            model.Address.PostalCode,
            model.Address.Latitude,
            model.Address.Longitude);

        var command = new CreateReportByCitizenCommand(
            instanceId,
            userId,
            phoneNumber,
            model.CategoryId,
            model.Comments,
            addressInfo,
            model.Attachments ?? new List<Guid>(),
            model.IsIdentityVisible);
        var report = await Sender.Send(command);
        
        var routeValues = new {id = report.Id, instanceId = instanceId };
        return CreatedAtAction(nameof(GetMyReportById), routeValues, report.Adapt<CitizenGetReportDetailsDto>());
        //return CreatedAtAction(nameof(GetMyReportById), routeValues, report);
        //return CreatedAtAction(nameof(GetMyReportById), report.Id, report);
        //return CreatedAtAction(nameof(GetMyReportById), report.Id, report.Adapt<CitizenGetReportDetailsDto>());
        //return CreatedAtAction(nameof(GetMyReportById), report.Id);

    }

    
    [Authorize(Roles = "Citizen")]
    [HttpGet("QuickAccesses")]
    public async Task<ActionResult<List<CitizenGetQuickAccess>>> GetQuickAccesses(int instanceId)
    {
        //there is 2 query for get QuickAccess in application layer.
        var query = new QuickAccessQuery(instanceId);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List<CitizenGetQuickAccess>>();
        return Ok(mappedResult);
    }
    

    [Authorize(Roles = "Citizen")]
    [HttpPut("Like/{id:Guid}")]
    //in old version returns int for report Likes number. 
    public async Task<ActionResult> Like(Guid id, bool isLiked)
    {
        var userId = User.GetUserId();
        if (userId == null) 
            return Unauthorized();
        var command = new LikeCommand(userId, id, isLiked);
        var result = await Sender.Send(command);
        if (!result)
            return Problem();
        return Ok();
    }

     
    [Authorize(Roles = "Citizen")]
    [HttpPost("Comment/{id:Guid}")]
    public async Task<ActionResult> CreateComment(int instanceId, Guid id, string comment)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        var command = new CreateCommentCommand(instanceId, userId, id, comment);
        var result = await Sender.Send(command);
        if (!result)
        {
            return Problem();
        }
        return Ok();
    }
    
    
    [Authorize(Roles = "Citizen")]
    [HttpGet("Comments/{id:Guid}")]
    public async Task<ActionResult<List<CitizenGetComments>>> GetComments(Guid id, [FromQuery] PagingInfo pagingInfo)
    {
        await Task.CompletedTask;//..............................
        return Ok();
    }



    [Authorize(Roles = "Citizen")]
    [HttpDelete("Comment/{commentId:Guid}")]
    public async Task<ActionResult> DeleteComment(Guid commentId)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return Unauthorized();
        var command = new DeleteCommentCommand(commentId, userId, User.GetUserRoles());
        var result = await Sender.Send(command);
        if (!result)
        {
            return Problem();
        }
        return Ok();
    }


    [Authorize(Roles = "Citizen")]
    [HttpPost("Feedback/{id:Guid}")]
    public async Task<ActionResult> Feedback(Guid id, CreateFeedbackDto createFeedbackDto)
    {
        //need userId, ReportId, token, rating..........................................
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Citizen")]
    [HttpPost("ReportViolation/{id:Guid}")]
    public async Task<ActionResult> CreateRepotrViolation(Guid id, CreateReportViolationDto createDto)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return Unauthorized();
        var command = new ReportViolationCommand(id, userId, createDto.ViolationTypeId, createDto.Description);
        var result = await Sender.Send(command);
        if(result == null)
            return Problem();
        return Ok();
    }


    [Authorize(Roles = "Citizen")]
    [HttpPost("CommentViolation/{id:Guid}")]
    public async Task<ActionResult> CreateCommentViolation(Guid id, CreateCommentViolationDto createViolationDto)
    {
        await Task.CompletedTask;//............................................
        return Ok();
    }

}
