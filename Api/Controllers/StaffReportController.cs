using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Comments.Commands.DeleteComment;
using Application.Comments.Commands.ReplyComment;
using Application.Comments.Commands.UpdateComment;
using Application.Comments.Queries.GetAllCommentsQuery;
using Application.Common.FilterModels;
using Application.Common.Interfaces.Persistence;
using Application.Reports.Commands.AcceptByOperator;
using Application.Reports.Commands.CreateReportByOperator;
using Application.Reports.Commands.InspectorTransition;
using Application.Reports.Commands.MakeTransition;
using Application.Reports.Commands.MessageToCitizen;
using Application.Reports.Commands.UpdateByOperator;
using Application.Reports.Common;
using Application.Reports.Queries.GetAllReports;
using Application.Reports.Queries.GetComments;
using Application.Reports.Queries.GetPossibleTransitions;
using Application.Reports.Queries.GetReportById;
using Application.Reports.Queries.GetReports;
using Application.Users.Queries.GetUserById;
using Application.Workspaces.Queries.GetPossibleSources;
using Domain.Models.Relational.Common;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]

public class StaffReportController : ApiController
{
    public StaffReportController(ISender sender) : base(sender)
    {
    }

    //todo : Define Access Policies

    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<GetReportsResponse>>> GetTasks( string? fromRoleId, [FromQuery]PagingInfo pagingInfo, [FromQuery]FilterGetReports filterGetReports)
    {
        var userId = User.GetUserId();
        var instanceId = User.GetUserInstanceId();
        var roles = User.GetUserRoles();
        var mappedFilter = filterGetReports.Adapt<FilterGetReportsModel>();
        var query = new GetReportsQuery(pagingInfo, userId, roles, fromRoleId, instanceId, mappedFilter);
        var result = await Sender.Send(query);
        
        if(result.IsFailed)
            return Problem(result.ToResult());
        Response.AddPaginationHeaders(result.Value.Meta);
        return Ok(result.Value);
    }


    [Authorize]
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<GetReportByIdResponse>> GetReportById(Guid id, int instanceId)
    {
        var userId = User.GetUserId();
        var query = new GetReportByIdQuery(id, userId, instanceId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    
    [Authorize]
    [HttpGet("AllReports")]
    public async Task<ActionResult<List<GetReportsResponse>>> GetAllReports(
        [FromQuery] PagingInfo pagingInfo, 
        [FromQuery] FilterGetAllReports filterGetAllReports)
    {
        var userId = User.GetUserId();
        var userRoles = User.GetUserRoles();
        var instanceId = User.GetUserInstanceId();
        var mappedFilter = filterGetAllReports.Adapt<FilterGetAllReportsModel>();
        var query = new GetAllReportsQuery(pagingInfo, instanceId, userId, userRoles, mappedFilter);
        var result = await Sender.Send(query);

        if (result.IsFailed)
            return Problem(result.ToResult());
        Response.AddPaginationHeaders(result.Value.Meta);
        return Ok(result.Value);
    }



    //TODO: Define access policy
    [Authorize]
    [HttpGet("PossibleTransitions/{id:Guid}")]
    public async Task<ActionResult<List<GetPossibleTransitionDto>>> GetPossibleTransitions(Guid id)
    {
        var userId = User.GetUserId();
        var instanceId = User.GetUserInstanceId();
        
        var query = new GetPossibleTransitionsQuery(id, userId, instanceId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<List<GetPossibleTransitionDto>>()),
            f => Problem(f));
    }


    //TODO: Define access policy
    [Authorize]
    [HttpPost("MakeTransition/{id:Guid}")]
    public async Task<ActionResult> MakeTransition(Guid id, MakeTransitionDto model)
    {
        var userId = User.GetUserId();

        MakeTransitionCommand command = new(
            id,
            model.TransitionId,
            model.ReasonId,
            model.Attachments,
            model.Comment,
            userId,
            model.ToActorId,
            User.IsInRole("Executive"),
            User.IsInRole("Contractor"));
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(),
            f => Problem(f));
    }


    [Authorize(Roles = "Inspector")]
    [HttpPost("Review/{id:Guid}")]
    public async Task<ActionResult> Review(Guid id, InspectorTransitionDto dto) 
    {
        var userId = User.GetUserId();
        var command = new InspectorTransitionCommand(
            id,
            dto.IsAccepted,
            dto.Attachments,
            dto.Comment,
            userId,
            dto.ToActorId,
            dto.StageId,
            dto.Visibility);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(),
            f => Problem(f));
    }


    //TODO: Define access policy
    [Authorize]
    [HttpGet("PossibleSources")]
    public async Task<ActionResult<List<GetPossibleSourceDto>>> GetPossibleSources()
    {
        var userId = User.GetUserId();
        var userRoles = User.GetUserRoles();
        var query = new GetPossibleSourcesQuery(userId, userRoles);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<List<GetPossibleSourceDto>>()),
            f => Problem(f));
    }


    //TODO: Define access policy
    [Authorize]
    [HttpPost("MessageToCitizen/{id:Guid}")]
    public async Task<ActionResult> MessageToCitizen(Guid id, MessageToCitizenDto messageToCitizenDto)
    {
        var userId = User.GetUserId();
        var userRoles = User.GetUserRoles();

        var command = new MessageToCitizenCommand(
            id,
            userId,
            userRoles,
            messageToCitizenDto.Attachments,
            messageToCitizenDto.Comment);

        var result = await Sender.Send(command);

        return result.Match(
           s => Ok(),
           f => Problem(f));
    }


    [Authorize(Roles = "Operator")]
    [HttpPost("RegisterByOperator")]
    public async Task<ActionResult> CreateReportByOperator(OperatorCreateReportDto model)
    {
        var instanceId = User.GetUserInstanceId();
        var operatorId = User.GetUserId();

        var addressInfo = new AddressInfoRequest(
            model.Address.RegionId,
            model.Address.Latitude,
            model.Address.Longitude,
            model.Address.Detail);

        var command = new CreateReportByOperatorCommand(
            instanceId,
            operatorId,
            model.PhoneNumber,
            model.FirstName,
            model.LastName,
            model.CategoryId,
            model.Comments,
            addressInfo,
            model.Attachments,
            model.IsIdentityVisible,
            model.Visibility == Visibility.EveryOne);

        var result = await Sender.Send(command);

        return result.Match(
            s => CreatedAtAction(nameof(GetReportById), new { id = s.Id, instanceId = instanceId }, s),
            f => Problem(f));
    }


    [Authorize(Roles = "Operator")]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateReportByOperator(Guid id, UpdateReportDto model)
    {
        var instanceId = User.GetUserInstanceId();
        var operatorId = User.GetUserId();

        AddressInfoRequest? addressInfo = null;
        if (model.Address != null)
        {
            addressInfo = new AddressInfoRequest(
            model.Address.RegionId,
            model.Address.Latitude,
            model.Address.Longitude,
            model.Address.Detail);

        }
        //TODO: Visibility should be considerd
        var command = new UpdateByOperatorCommand(
            id,
            operatorId,
            model.CategoryId,
            model.Comments,
            addressInfo,
            model.Attachments,
            model.Visibility == Visibility.EveryOne);
        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }


    [Authorize(Roles = "Operator")]
    [HttpPut("Accept/{id:Guid}")]
    public async Task<ActionResult> AcceptReportByOperator(Guid id, UpdateReportDto model)
    {
        var instanceId = User.GetUserInstanceId();
        var operatorId = User.GetUserId();

        AddressInfoRequest? addressInfo = null;
        if (model.Address != null)
        {
            addressInfo = new AddressInfoRequest(
            model.Address.RegionId,
            model.Address.Latitude,
            model.Address.Longitude,
            model.Address.Detail);

        }
        //TODO: Visibility should be considerd
        var command = new AcceptByOperatorCommand(
            id,
            operatorId,
            model.CategoryId,
            model.Comments,
            addressInfo,
            model.Attachments,
            model.Visibility == Visibility.EveryOne);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(id),
            f => Problem(f));
    }



    [Authorize(Roles = "Operator")]
    [HttpGet("Comments")]
    public async Task<ActionResult<List<GetCommentsDto>>> GetComments([FromQuery] PagingInfo pagingInfo, [FromQuery] FilterGetCommentViolation filter)
    {
        //have FilterGetComments
        var instanceId = User.GetUserInstanceId();
        var mappedFilter = filter.Adapt<FilterGetCommentViolationModel>();
        var query = new GetAllCommentsQuery(pagingInfo, instanceId, mappedFilter);
        var result = await Sender.Send(query);

        if(result.IsFailed)
            return Problem(result.ToResult());
        Response.AddPaginationHeaders(result.Value.Meta);
        return Ok(result.Value.Adapt<List<GetCommentsDto>>());
    }


    [Authorize(Roles = "Operator")]
    [HttpGet("ReportComments/{ReportId:Guid}")]
    public async Task<ActionResult<List<GetReportComments>>> GetReportComments(Guid ReportId, [FromQuery] PagingInfo pagingInfo)
    {
        var userId = User.GetUserId();
        var query = new GetCommentsQuery(ReportId, pagingInfo);
        var result = await Sender.Send(query);

        if(result.IsFailed)
            return Problem(result.ToResult());
    
        Response.AddPaginationHeaders(result.Value.Meta);
        var mappedResult = new List<GetReportComments>();
        foreach (var item in result.Value)
        {
            var mapppedItem = item.Adapt<GetReportComments>();
            mapppedItem.CanDelete = item.UserId == userId;
            mappedResult.Add(mapppedItem);
        }
        return Ok(mappedResult);
    }
                                                

    [Authorize(Roles = "Operator")]
    [HttpPost("ReplyComment/{commentId:Guid}")]
    public async Task<ActionResult> ReplyComment(Guid commentId, ReplyCommentDto replyCommentDto)
    {
        var userId = User.GetUserId();
        var command = new ReplyCommentCommand(userId, commentId, replyCommentDto.Comment);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(),
            f => Problem(f));
    }


    [Authorize(Roles = "Operator")]
    [HttpPut("Comment/{commentId:Guid}")]
    public async Task<ActionResult> PutComment(Guid commentId, UpdateCommentDto updateCommentDto)
    {
        var userId = User.GetUserId();
        var command = new UpdateCommentCommand(userId, commentId, updateCommentDto.Comment);
        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }


    [Authorize(Roles = "Operator")]
    [HttpDelete("Comment/{id:Guid}")]
    public async Task<ActionResult> DeleteComment(Guid id)
    {
        var userId = User.GetUserId();
        var command = new DeleteCommentCommand(id, userId, User.GetUserRoles());
        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }


    //todo: is this used??
    [Authorize(Roles = "Operator")]
    [HttpPut("Satisfaction/{id:Guid}")]   //id : reportId
    public async Task<ActionResult> PutSatisfaction(Guid id, PutSatisfactionDto putSatisfactionDto)
    {
        await Task.CompletedTask;//....................................
        return NoContent();
    }

    [Authorize(Roles = "Operator")]
    [HttpGet("Violations")]
    public async Task<ActionResult<List<GetViolationsDto>>> GetViolations([FromQuery] PagingInfo pagingInfo, [FromQuery] FilterGetCommentViolation filter)
    {
        await Task.CompletedTask;//.......................................
        return Ok();
    }


    [Authorize(Roles = "Operator")]
    [HttpPut("Violation/{id:Guid}")]
    public async Task<ActionResult> PutViolation(Guid id, ViolationPutDto violationPutDto)
    {
        await Task.CompletedTask;//.......................................
        return NoContent();
    }

    //todo : define Access Policy
    [Authorize]
    [HttpGet("Citizen/{id}")]
    public async Task<ActionResult<GetCitizenDto>> GetCitizenById(string id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<GetCitizenDto>()),
            f => Problem(f));
    }


    //todo : define Access Policy
    [Authorize]
    [HttpGet("ReportHistory/{id:Guid}")]
    public async Task<ActionResult<List<TransitionLogDto>>> GetReportHistory(Guid id)
    {
        var userId = User.GetUserId();
        var instanceId = User.GetUserInstanceId();
        var query = new GetHistoryQuery(id, userId, instanceId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<List<TransitionLogDto>>()),
            f => Problem(f));
    }

    

}
