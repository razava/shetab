using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Comments.Commands.DeleteComment;
using Application.Comments.Commands.ReplyComment;
using Application.Comments.Commands.UpdateComment;
using Application.Comments.Queries.GetAllCommentsQuery;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Info.Queries.GetInfo;
using Application.ReportNotes.Commands.AddReportNote;
using Application.ReportNotes.Commands.DeleteReportNote;
using Application.ReportNotes.Commands.UpdateReportNote;
using Application.ReportNotes.Queries.GetAllReportNotes;
using Application.ReportNotes.Queries.GetReportNotes;
using Application.Reports.Commands.AcceptByOperator;
using Application.Reports.Commands.CreateReportByOperator;
using Application.Reports.Commands.MakeObjection;
using Application.Reports.Commands.MakeTransition;
using Application.Reports.Commands.MessageToCitizen;
using Application.Reports.Commands.UpdateByOperator;
using Application.Reports.Common;
using Application.Reports.Queries.GetComments;
using Application.Reports.Queries.GetPossibleTransitions;
using Application.Reports.Queries.GetReportById;
using Application.Reports.Queries.GetReports;
using Application.Reports.Queries.GetUserReports;
using Application.Satisfactions.Commands.UpsertSatisfaction;
using Application.Satisfactions.Queries.GetSatisfaction;
using Application.Users.Queries.GetUserById;
using Application.Violations.Commands.CheckCommentViolation;
using Application.Violations.Commands.CheckReportViolation;
using Application.Violations.Queries.GetCommentViolations;
using Application.Violations.Queries.GetCommentViolationsList;
using Application.Violations.Queries.GetReportViolations;
using Application.Violations.Queries.GetReportViolationsList;
using Application.Workspaces.Queries.GetPossibleSources;
using Domain.Models.Relational.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class StaffReportController : ApiController
{
    public StaffReportController(ISender sender) : base(sender)
    {
    }

    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetTasks(
        string? fromRoleId,
        [FromQuery] PagingInfo pagingInfo,
        [FromQuery] ReportFilters reportFilters)
    {
        var userId = User.GetUserId();
        var instanceId = User.GetUserInstanceId();
        var roles = User.GetUserRoles();
        var query = new GetReportsQuery(pagingInfo, userId, roles, fromRoleId, instanceId, reportFilters);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize]
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult> GetReportById(Guid id)
    {
        var instanceId = User.GetUserInstanceId();
        var userId = User.GetUserId();
        var query = new GetReportByIdQuery(id, userId, instanceId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    //TODO: Define access policy
    [Authorize]
    [HttpGet("PossibleTransitions/{id:Guid}")]
    public async Task<ActionResult> GetPossibleTransitions(Guid id)
    {
        var userId = User.GetUserId();
        var instanceId = User.GetUserInstanceId();
        
        var query = new GetPossibleTransitionsQuery(id, userId, instanceId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
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
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Inspector")]
    [HttpPost("Objection/{id:Guid}")]
    public async Task<ActionResult> MakeObjection(Guid id, [FromBody] CitizenObjectReportDto objectionDto)
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
            s => Ok(s),
            f => Problem(f));
    }

    //TODO: Define access policy
    [Authorize]
    [HttpGet("PossibleSources")]
    public async Task<ActionResult> GetPossibleSources()
    {
        var userId = User.GetUserId();
        var userRoles = User.GetUserRoles();
        var query = new GetPossibleSourcesQuery(userId, userRoles);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
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
           s => Ok(s),
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
            s => CreatedAtAction(nameof(GetReportById), new { id = s.Value.Id }, s),
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
            model.Priority,
            model.Visibility);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
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
            model.Priority,
            model.Visibility);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }



    [Authorize(Roles = "Operator")]
    [HttpGet("Comments")]
    public async Task<ActionResult> GetComments([FromQuery] PagingInfo pagingInfo, [FromQuery] CommentFilters filter)
    {
        //have FilterGetComments
        var instanceId = User.GetUserInstanceId();
        var query = new GetAllCommentsQuery(pagingInfo, instanceId, filter);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("ReportComments/{ReportId:Guid}")]
    public async Task<ActionResult> GetReportComments(Guid ReportId, [FromQuery] PagingInfo pagingInfo)
    {
        var userId = User.GetUserId();
        var query = new GetCommentsQuery(ReportId, userId, pagingInfo);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }
                                                

    [Authorize(Roles = "Operator")]
    [HttpPost("ReplyComment/{commentId:Guid}")]
    public async Task<ActionResult> ReplyComment(Guid commentId, ReplyCommentDto replyCommentDto)
    {
        var userId = User.GetUserId();
        var command = new ReplyCommentCommand(userId, commentId, replyCommentDto.Comment);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
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
            s => Ok(s),
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
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = "Operator")]
    [HttpPost("Satisfaction/{reportId:Guid}")]
    public async Task<ActionResult> UpsertSatisfaction(Guid reportId, UpsertSatisfactionDto satisfactionDto)
    {
        var instanceId = User.GetUserInstanceId();
        var userId = User.GetUserId();
        var command = new UpsertSatisfactionCommand(
            reportId,
            userId,
            satisfactionDto.Comments,
            satisfactionDto.Rating);

        var result = await Sender.Send(command);
        return result.Match(
            s => CreatedAtAction(nameof(GetSatisfaction), new { reportId = reportId }, s),
            f => Problem(f));
    }

    [Authorize]
    [HttpGet("Satisfaction/{reportId:Guid}")]
    public async Task<ActionResult> GetSatisfaction(Guid reportId)
    {
        var query = new GetSatisfactionQuery(reportId);

        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Operator")]
    [HttpGet("ReportViolations")]
    public async Task<ActionResult> GetReportViolationsList([FromQuery] PagingInfo pagingInfo)
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetReportViolationsListQuery(instanceId, pagingInfo);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Operator")]
    [HttpGet("ReportViolations/{id:guid}")]
    public async Task<ActionResult> GetReportViolations(Guid id, [FromQuery] PagingInfo pagingInfo)
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetReportViolationsQuery(id, pagingInfo);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = RoleNames.Operator)]
    [HttpPut("ReportViolations/{id:Guid}")]
    public async Task<ActionResult> CheckReportViolation(Guid id, CheckReportViolationDto checkReportViolationDto)
    {
        var userId = User.GetUserId();
        var query = new CheckReportViolationCommand(
            id,
            userId,
            checkReportViolationDto.Action,
            checkReportViolationDto.Comments,
            checkReportViolationDto.Attachments);

        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = "Operator")]
    [HttpGet("CommentViolations")]
    public async Task<ActionResult> GetCommentViolationsList([FromQuery] PagingInfo pagingInfo)
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetCommentViolationsListQuery(instanceId, pagingInfo);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Operator")]
    [HttpGet("CommentViolations/{id:guid}")]
    public async Task<ActionResult> GetCommentViolations(Guid id, [FromQuery] PagingInfo pagingInfo)
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetCommentViolationsQuery(id, pagingInfo);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = RoleNames.Operator)]
    [HttpPut("CommentViolations/{id:Guid}")]
    public async Task<ActionResult> CheckCommentViolation(Guid id, CheckCommentViolationDto checkCommentViolationDto)
    {
        var userId = User.GetUserId();
        var query = new CheckCommentViolationCommand(
            id,
            userId,
            checkCommentViolationDto.Action,
            checkCommentViolationDto.Text);

        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    //todo : define Access Policy
    [Authorize]
    [HttpGet("Citizen/{id}")]
    public async Task<ActionResult> GetCitizenById(string id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = RoleNames.Operator)]
    [HttpGet("UserReports/{id}")]
    public async Task<ActionResult> GetUserReportsById([FromQuery] PagingInfo pagingInfo, string id)
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetUserReportsQuery(pagingInfo, id, instanceId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    //todo : define Access Policy
    [Authorize]
    [HttpGet("ReportHistory/{id:Guid}")]
    public async Task<ActionResult> GetReportHistory(Guid id)
    {
        var userId = User.GetUserId();
        var instanceId = User.GetUserInstanceId();
        var query = new GetHistoryQuery(id, userId, instanceId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("Notes/{reportId:guid}")]
    public async Task<ActionResult> GetReportNotes(Guid reportId)
    {
        var userId = User.GetUserId();
        var query = new GetReportNotesQuery(userId, reportId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("Notes")]
    public async Task<ActionResult> GetUserReportNotes(
        [FromQuery] PagingInfo pagingInfo)
    {
        var userId = User.GetUserId();
        var query = new GetAllReportNotesQuery(pagingInfo, userId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize]
    [HttpPost("Notes/{reportId:guid}")]
    public async Task<ActionResult> CreateUserReportNotes
        (Guid reportId, CreateReportNoteDto reportNoteDto)
    {
        var instanceId = User.GetUserInstanceId();
        var userId = User.GetUserId();
        var command = new AddReportNoteCommand(userId, reportId, reportNoteDto.Text);
        var result = await Sender.Send(command);

        return result.Match(
            s => CreatedAtAction(nameof(GetReportNotes), new { reportId }, s),
            f => Problem(f));
    }

    [Authorize]
    [HttpPut("Notes/{noteId:guid}")]
    public async Task<ActionResult> UpdateUserReportNotes
        (Guid noteId, CreateReportNoteDto reportNoteDto)
    {
        var userId = User.GetUserId();
        var command = new UpdateReportNoteCommand(noteId, userId, reportNoteDto.Text);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize]
    [HttpDelete("Notes/{noteId:guid}")]
    public async Task<ActionResult> DeleteUserReportNotes
        (Guid noteId)
    {
        var userId = User.GetUserId();
        var command = new DeleteReportNoteCommand(noteId, userId);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }
}

public record CreateReportNoteDto(string Text);
public record UpsertSatisfactionDto(string Comments, int Rating);

public record CheckReportViolationDto(
    ViolationCheckResult Action,
    string? Comments,
    List<Guid>? Attachments);

public record CheckCommentViolationDto(
    ViolationCheckResult Action,
    string? Text);
