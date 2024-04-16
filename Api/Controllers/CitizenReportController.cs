using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Comments.Commands.CreateComment;
using Application.Comments.Commands.DeleteComment;
using Application.Common.Interfaces.Persistence;
using Application.Feedbacks.Commands.StoreFeedback;
using Application.QuickAccesses.Queries.GetCitizenQuickAccesses;
using Application.Reports.Commands.CreateReportByCitizen;
using Application.Reports.Commands.Like;
using Application.Reports.Commands.MakeObjection;
using Application.Reports.Common;
using Application.Reports.Queries.GetCitizenReportById;
using Application.Reports.Queries.GetComments;
using Application.Reports.Queries.GetNearestReports;
using Application.Reports.Queries.GetRecentReports;
using Application.Reports.Queries.GetReportById;
using Application.Reports.Queries.GetUserReports;
using Application.Violations.Commands.CommentViolation;
using Application.Violations.Commands.ReportViolation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult> GetReports(
        int instanceId,
        [FromQuery] PagingInfo pagingInfo)
    {
        var userId = User.GetUserId();
        var userRoles = User.GetUserRoles();

        var query = new GetRecentReportsQuery(pagingInfo, instanceId, userId, userRoles);

        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    //todo : review returning Dto....................................
    [Authorize(Roles = "Citizen")]
    [HttpGet("ReportHistory/{id:Guid}")]
    public async Task<ActionResult> GetReportHistoryById(Guid id)
    {
        var userId = User.GetUserId();
        var instanceId = User.GetUserInstanceId();
        var query = new GetHistoryQuery(id, userId, instanceId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = "Citizen")]
    [HttpGet("Nearest")]
    public async Task<ActionResult> GetNearest(
        int instanceId,
        [FromQuery] PagingInfo pagingInfo,
        [FromQuery] LocationDto locationDto)
    {
        var userId = User.GetUserId();
        var query = new GetNearestReportsQuery(pagingInfo, instanceId, userId, locationDto.Longitude, locationDto.Latitude);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    //todo : define & set dtos
    [Authorize(Roles = "Citizen")]
    [HttpGet("Locations")]
    public async Task<ActionResult> GetLocations()
    {
        await Task.CompletedTask;//.......................
        return Ok("Not Implemented");
    }

    //todo : check usage
    //todo : define & set dtos
    [Authorize(Roles = "Citizen")]
    [HttpGet("Locations/{id:Guid}")]
    public async Task<ActionResult> GetLocationsById(Guid id)
    {
        await Task.CompletedTask;
        return Ok("Not Implemented");
    }


    [Authorize(Roles = "Citizen")]
    [HttpGet("Mine")]
    public async Task<ActionResult> GetMyReports(
        [FromQuery] PagingInfo pagingInfo)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        var query = new GetUserReportsQuery(pagingInfo, userId, null);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s), 
            f => Problem(f));
    }


    [Authorize(Roles = "Citizen")]
    [HttpGet("Mine/{id:Guid}")]
    public async Task<ActionResult> GetMyReportById(Guid id)
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized();

        var query = new GetCitizenReportByIdQuery(id, userId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = "Citizen")]
    [HttpPost]
    public async Task<ActionResult> CreateReport(int instanceId, CitizenCreateReportDto model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (userId == null || username == null)
        {
            return Unauthorized();
        }

        var phoneNumber = username;
        var addressInfo = new AddressInfoRequest(
            model.Address.RegionId,
            model.Address.Latitude,
            model.Address.Longitude,
            model.Address.Detail);

        var command = new CreateReportByCitizenCommand(
            instanceId,
            userId,
            phoneNumber,
            model.CategoryId,
            model.Comments,
            addressInfo,
            model.Attachments ?? new List<Guid>(),
            model.IsIdentityVisible);
        var result = await Sender.Send(command);

        return result.Match(
            s => CreatedAtAction(nameof(GetMyReportById), new { id = s.Value.Id, instanceId = instanceId },  s),
            f => Problem(f));
    }

    
    [Authorize(Roles = "Citizen")]
    [HttpGet("QuickAccesses")]
    public async Task<ActionResult> GetQuickAccesses(int instanceId)
    {
        var userRoles = User.GetUserRoles();
        var query = new GetCitizenQuickAccessesQuery(instanceId, userRoles);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }
    

    [Authorize(Roles = "Citizen")]
    [HttpPut("Like/{id:Guid}")]
    public async Task<ActionResult> Like(Guid id, bool isLiked)
    {
        var userId = User.GetUserId();
        if (userId == null) 
            return Unauthorized();
        var command = new LikeCommand(userId, id, isLiked);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

     
    [Authorize(Roles = "Citizen")]
    [HttpPost("Comment/{id:Guid}")]
    public async Task<ActionResult> CreateComment(int instanceId, Guid id, CreateCommentDto commentDto)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        var command = new CreateCommentCommand(instanceId, userId, id, commentDto.Comment);
        var result = await Sender.Send(command);
        
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }
    
    
    [Authorize(Roles = "Citizen")]
    [HttpGet("Comments/{id:Guid}")]
    public async Task<ActionResult> GetComments(Guid id, [FromQuery] PagingInfo pagingInfo)
    {
        var userId = User.GetUserId();
        var query = new GetCommentsQuery(id, userId, pagingInfo);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
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

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Citizen")]
    [HttpPost("Objection/{id:Guid}")]
    public async Task<ActionResult> MakeObjection(Guid id, [FromBody]CitizenObjectReportDto objectionDto)
    {
        var userId = User.GetUserId();
        var userRoles = User.GetUserRoles();
        var command = new MakeObjectionCommand(
            userId,
            userRoles,
            id,
            objectionDto.Attachments ?? new List<Guid>(),
            objectionDto.Comments);
        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }


    [Authorize(Roles = "Citizen")]
    [HttpPost("Feedback/{id:Guid}")]
    public async Task<ActionResult> Feedback(Guid id, [FromBody] CreateFeedbackDto createFeedbackDto)
    {
        var userId = User.GetUserId();
        var command = new StoreFeedbackCommand(id, userId, null, createFeedbackDto.Rating);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [HttpPost("FeedbackUnAuthorized/{token}")]
    public async Task<ActionResult> Feedback(string token, [FromBody] CreateFeedbackDto createFeedbackDto)
    {
        var userId = User.GetUserId();
        var command = new StoreFeedbackCommand(null, null, token, createFeedbackDto.Rating);
        var result = await Sender.Send(command);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Citizen")]
    [HttpPost("ReportViolation/{id:Guid}")]
    public async Task<ActionResult> CreateRepotrViolation(Guid id, int instanceId, CreateReportViolationDto createDto)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return Unauthorized();
        var command = new ReportViolationCommand(instanceId, id, userId, createDto.ViolationTypeId, createDto.Description);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = "Citizen")]
    [HttpPost("CommentViolation/{id:Guid}")]
    public async Task<ActionResult> CreateCommentViolation(Guid id, int instanceId, CreateCommentViolationDto createViolationDto)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return Unauthorized();
        var command = new CommentViolationCommand(instanceId, id, userId, createViolationDto.ViolationTypeId, createViolationDto.Description);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

}
