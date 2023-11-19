using Api.Abstractions;
using Api.Contracts;
using Api.Dtos;
using Api.ExtensionMethods;
using Api.Services.Authentication;
using Application.Comments.Commands.DeleteComment;
using Application.Comments.Commands.ReplyComment;
using Application.Comments.Commands.UpdateComment;
using Application.Common.Interfaces.Persistence;
using Application.Reports.Commands.AcceptByOperator;
using Application.Reports.Commands.CreateReportByOperator;
using Application.Reports.Commands.MessageToCitizen;
using Application.Reports.Commands.UpdateByOperator;
using Application.Reports.Common;
using Application.Reports.Queries.GetAllReports;
using Application.Reports.Queries.GetPossibleTransitions;
using Application.Reports.Queries.GetReportById;
using Application.Reports.Queries.GetReports;
using Application.Users.Queries.GetUserById;
using Application.Workspaces.Queries.GetPossibleSources;
using DocumentFormat.OpenXml.Office2019.Word.Cid;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

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
    public async Task<ActionResult<List<StaffGetReportListDto>>> GetTasks(int instanceId, [FromQuery]PagingInfo pagingInfo, [FromQuery]FilterGetReports filterGetReports)
    {
        //have FilterGetReports.........................................

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized();

        var query = new GetReportsQuery(pagingInfo, userId, instanceId);
        var result = await Sender.Send(query);
        Response.AddPaginationHeaders(result.Meta);
        var mappedResult = result.Adapt<List<StaffGetReportListDto>>();
        return Ok(mappedResult);
    }


    [Authorize]
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<StaffGetReportDetailsDto>> GetTaskById(Guid id, int instanceId)
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized();
        var query = new GetReportByIdQuery(id, userId, instanceId);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<StaffGetReportDetailsDto>();
        return Ok(mappedResult);
    }

    
    [Authorize]
    [HttpGet("AllReports")]
    public async Task<ActionResult<List<StaffGetReportListDto>>> GetAllReports([FromQuery] PagingInfo pagingInfo, [FromQuery] FilterGetAllReports filterGetAllReports, int instanceId)
    {
        //have FilterGetAllReports (diffrent from FilterGetReports)

        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized();
        var userRoles = User.GetUserRoles();
            var query = new GetAllReportsQuery(pagingInfo, instanceId, userId, userRoles);
        var result = await Sender.Send(query);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.Meta));
        var mappedResult = result.Adapt<List<StaffGetReportListDto>>();
        return Ok(mappedResult);
    }



    //TODO: Define access policy : other staff?
    [Authorize(Roles = "Operator")]
    [HttpGet("PossibleTransitions/{id:Guid}")]
    public async Task<ActionResult<List<GetPossibleTransitionDto>>> GetPossibleTransitions(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized();

        //TODO: Get this from token
        var instanceId = 1;

        var query = new GetPossibleTransitionsQuery(id, userId, instanceId);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List<GetPossibleTransitionDto>>();
        return Ok(mappedResult);    
    }


    //TODO: Define access policy
    [Authorize]
    [HttpPost("MakeTransition/{id:Guid}")]
    public async Task<ActionResult> MakeTransition(Guid id, MakeTransitionDto model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized();

        MakeTransitionCommand command = new(
            id,
            model.TransitionId,
            model.ReasonId,
            model.Attachments,
            model.Comment,
            userId,
            model.ActorIds,
            User.IsInRole("Executive"),
            User.IsInRole("Contractor"));
        var result = await Sender.Send(command);
        //need to handle result?
        return Ok();
    }


    [Authorize(Roles = "Inspector")]
    [HttpPost("Review/{id:Guid}")]
    public async Task<ActionResult> Review(Guid id, MoveToStageDto moveToStageDto)
    {
        await Task.CompletedTask;//.........................................................
        return Ok();
    }


    //TODO: Define access policy
    [Authorize]
    [HttpGet("PossibleSources")]
    public async Task<ActionResult<List<GetPossibleSourceDto>>> GetPossibleSources()
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized();
        // query needs userId & user roles......................................................
        //var query = new GetPossibleSourcesQuery(userId);
        await Task.CompletedTask;
        return Ok();
    }


    //TODO: Define access policy
    [Authorize]
    [HttpPost("MessageToCitizen/{id:Guid}")]
    public async Task<ActionResult> MessageToCitizen(Guid id, MessageToCitizenDto messageToCitizenDto)
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized();
        var userRoles = User.GetUserRoles();

        var command = new MessageToCitizenCommand(
            id,
            userId,
            userRoles,
            messageToCitizenDto.Attachments,
            messageToCitizenDto.Comment,
            messageToCitizenDto.IsPublic,
            messageToCitizenDto.Message);

        var result = await Sender.Send(command);
        if (result == null)
            return Problem();
        return Ok();
    }



    [Authorize(Roles = "Operator")]
    [HttpPost("RegisterByOperator")]
    public async Task<ActionResult<Guid>> CreateReportByOperator([FromForm] OperatorCreateReportDto model)
    {
        var instanceIdStr = User.FindFirstValue(AppClaimTypes.InstanceId);
        if (instanceIdStr == null)
        {
            return BadRequest();
        }
        var instanceId = int.Parse(instanceIdStr);
        if (instanceId <= 0)
        {
            return BadRequest();
        }
        var operatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (operatorId == null)
        {
            return Unauthorized();
        }

        var addressInfo = new AddressInfo(
            model.Address.RegionId,
            model.Address.Street,
            model.Address.Valley,
            model.Address.Detail,
            model.Address.Number,
            model.Address.PostalCode,
            model.Address.Latitude!.Value,
            model.Address.Longitude!.Value);

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


        var report = await Sender.Send(command);
        //todo : handle result
        return Ok(report.Id);
    }

    [Authorize(Roles = "Operator")]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateReportByOperator(Guid id, [FromForm] UpdateReportDto model)
    {
        var instanceIdStr = User.FindFirstValue(AppClaimTypes.InstanceId);
        if (instanceIdStr == null)
        {
            return BadRequest();
        }
        var instanceId = int.Parse(instanceIdStr);
        if (instanceId <= 0)
        {
            return BadRequest();
        }
        var operatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (operatorId == null)
        {
            return Unauthorized();
        }

        AddressInfo? addressInfo = null;
        if (model.Address != null)
        {
            addressInfo = new AddressInfo(
                model.Address.RegionId,
                model.Address.Street,
                model.Address.Valley,
                model.Address.Detail,
                model.Address.Number,
                model.Address.PostalCode,
                model.Address.Latitude!.Value,
                model.Address.Longitude!.Value);

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
        await Sender.Send(command);

        return NoContent();
    }


    [Authorize(Roles = "Operator")]
    [HttpPut("Accept/{id:Guid}")]
    public async Task<ActionResult> AcceptReportByOperator(Guid id, [FromForm] UpdateReportDto model)
    {
        var instanceIdStr = User.FindFirstValue(AppClaimTypes.InstanceId);
        if (instanceIdStr == null)
        {
            return BadRequest();
        }
        var instanceId = int.Parse(instanceIdStr);
        if (instanceId <= 0)
        {
            return BadRequest();
        }
        var operatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (operatorId == null)
        {
            return Unauthorized();
        }

        AddressInfo? addressInfo = null;
        if (model.Address != null)
        {
            addressInfo = new AddressInfo(
                model.Address.RegionId,
                model.Address.Street,
                model.Address.Valley,
                model.Address.Detail,
                model.Address.Number,
                model.Address.PostalCode,
                model.Address.Latitude!.Value,
                model.Address.Longitude!.Value);

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
        await Sender.Send(command);

        return NoContent();
    }



    [Authorize(Roles = "Operator")]
    [HttpGet("Comments")]
    public async Task<ActionResult<List<GetCommentsDto>>> GetComments([FromQuery] PagingInfo pagingInfo, [FromQuery] FilterGetCommentViolation filter)
    {
        //have FilterGetComments
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Operator")]
    [HttpPost("ReplyComment/{commentId:Guid}")]
    public async Task<ActionResult> ReplyComment(Guid commentId, ReplyCommentDto replyCommentDto)
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized();
        var command = new ReplyCommentCommand(userId, commentId, replyCommentDto.Comment);
        var result = await Sender.Send(command);
        if (!result)
            return Problem();
        return Ok();
    }

    [Authorize(Roles = "Operator")]
    [HttpPut("Comment/{commentId:Guid}")]
    public async Task<ActionResult> PutComment(Guid commentId, UpdateCommentDto updateCommentDto)
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized();
        var command = new UpdateCommentCommand(userId, commentId, updateCommentDto.Comment);
        var result = await Sender.Send(command);
        if (!result)
            return Problem();
        return Ok();
    }


    [Authorize(Roles = "Operator")]
    [HttpDelete("Comment/{id:Guid}")]
    public async Task<ActionResult> DeleteComment(Guid id)
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized();
        var command = new DeleteCommentCommand(id, userId, User.GetUserRoles());
        var result = await Sender.Send(command);
        if (!result)
        {
            return Problem();
        }
        return Ok();
    }


    //todo: is this used??
    [Authorize(Roles = "Operator")]
    [HttpPut("Satisfaction/{id:Guid}")]   //id : reportId
    public async Task<ActionResult> PutSatisfaction(Guid id, PutSatisfactionDto putSatisfactionDto)
    {
        await Task.CompletedTask;//....................................
        return Ok();
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
        return Ok();
    }

    //todo : define Access Policy
    [Authorize]
    [HttpGet("Citizen/{id}")]
    public async Task<ActionResult<GetCitizenDto>> GetCitizenById(string id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await Sender.Send(query);
        if(result == null)
            return NotFound();
        var mappedResult = result.Adapt<GetCitizenDto>();
        return Ok(mappedResult);
    }


    //todo : define Access Policy
    [Authorize]
    [HttpGet("ReportHistory/{id:Guid}")]
    public async Task<ActionResult<List<TransitionLogDto>>> GetReportHistory(Guid id)
    {
        await Task.CompletedTask;//.............................................
        return Ok();
    }

    /*
    //??
    [Authorize]
    [HttpGet("ReportsUnkownEndpoint")]
    public async Task<ActionResult<List<Report>>> GetReports([FromQuery] PagingInfo pagingInfo, int instanceId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized();

        var query = new GetReportsQuery(pagingInfo, userId, instanceId);
        var result = await Sender.Send(query);
        Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(result.Meta));
        return Ok(result.ToList());
    }
    */

}
