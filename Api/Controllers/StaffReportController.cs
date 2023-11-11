using Api.Abstractions;
using Api.Contracts;
using Api.Dtos;
using Api.Services.Authentication;
using Application.Common.Interfaces.Persistence;
using Application.Reports.Commands.AcceptByOperator;
using Application.Reports.Commands.CreateReportByOperator;
using Application.Reports.Commands.UpdateByOperator;
using Application.Reports.Common;
using Application.Reports.Queries.GetPossibleTransitions;
using Application.Reports.Queries.GetReports;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
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
    protected StaffReportController(ISender sender) : base(sender)
    {
    }

    //........................
    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetTasks()
    {
        await Task.CompletedTask;
        return Ok();
    }

    //........................
    [Authorize]
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult> GetTaskById(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
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
        //todo : fix this :
        //return result; 
        return Ok();    
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

        return Ok();
    }


    [Authorize(Roles = "Inspector")]
    [HttpPost("Review/{id:Guid}")]
    public async Task<ActionResult> Review(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //TODO: Define access policy
    [Authorize]
    [HttpGet("PossibleSources")]
    public async Task<ActionResult> GetPossibleSources()
    {
        await Task.CompletedTask;
        return Ok();
    }


    //TODO: Define access policy
    [Authorize]
    [HttpPost("MessageToCitizen/{id:Guid}")]
    public async Task<ActionResult> MessageToCitizen(Guid id)
    {
        await Task.CompletedTask;
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
            model.Address.RegionId!.Value,
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

        return Ok(report.Id);
    }

    //todo : shoudn't have id for  input and route?
    [Authorize(Roles = "Operator")]
    [HttpPut]
    public async Task<ActionResult<Guid>> UpdateReportByOperator([FromForm] UpdateReportDto model)
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
                model.Address.RegionId!.Value,
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
            model.Id,
            operatorId,
            model.CategoryId,
            model.Comments,
            addressInfo,
            model.Attachments,
            model.Visibility == Visibility.EveryOne);
        await Sender.Send(command);

        return NoContent();
    }


    //todo : shoudn't have id for  input param and route?
    [Authorize(Roles = "Operator")]
    [HttpPut("Accept")]
    public async Task<ActionResult<Guid>> AcceptReportByOperator([FromForm] UpdateReportDto model)
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
                model.Address.RegionId!.Value,
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
            model.Id,
            operatorId,
            model.CategoryId,
            model.Comments,
            addressInfo,
            model.Attachments,
            model.Visibility == Visibility.EveryOne);
        await Sender.Send(command);

        return NoContent();
    }



    //todo : Define & Set Input Dtos

    [Authorize(Roles = "Operator")]
    [HttpGet("Comments")]
    public async Task<ActionResult> GetComments([FromQuery] PagingInfo pagingInfo)
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Operator")]
    [HttpPut("Comment/{commentId:Guid}")]
    public async Task<ActionResult> PutComment(Guid commentId, UpdateCommentDto updateCommentDto)
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Operator")]
    [HttpDelete("Comment/{id:Guid}")]
    public async Task<ActionResult> DeleteComment(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }


    //todo: is this used??
    [Authorize(Roles = "Operator")]
    [HttpPut("Satisfaction/{id:Guid}")]   //id : reportId
    public async Task<ActionResult> PutSatisfaction(Guid id, PutSatisfactionDto putSatisfactionDto)
    {
        await Task.CompletedTask;
        return Ok();
    }


    //todo : is this used? and is access policy correct?
    [Authorize(Roles = "Operator")]
    [HttpPost("Feedback/SendNow")]
    public async Task<ActionResult> SendNowFeedback()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Operator")]
    [HttpGet("Violations")]
    public async Task<ActionResult> GetViolations()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Operator")]
    [HttpPut("Violation/{id:Guid}")]
    public async Task<ActionResult> GetViolations(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }




    //??
    [Authorize]
    [HttpGet("ReportsunkownEndpoint")]
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


}
