using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Polls.Commands.AddPollCommand;
using Application.Polls.Commands.UpdatePollCommand;
using Application.Polls.Common;
using Application.Polls.Queries.GetPollResultQuery;
using Application.Polls.Queries.GetPollsByIdQuery;
using Application.Polls.Queries.GetPollsQuery;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class AdminPollsController : ApiController
{
    public AdminPollsController(ISender sender) : base(sender)
    {
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreatePoll(PollCreateDto createDto)
    {
        var userId = User.GetUserId();
        var instanceId = User.GetUserInstanceId();
        var command = new AddPollCommand(
            instanceId,
            userId,
            createDto.Title,
            createDto.PollType,
            createDto.Question,
            createDto.Choices.Adapt<List<PollChoiceRequest>>(),
            createDto.IsActive);
        var result = await Sender.Send(command);

        return result.Match(
            s => CreatedAtAction(nameof(GetPollById), new { id = s.Id, instanceId = instanceId }, s.Adapt<GetPollsDto>()),
            f => Problem(f));
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("All")]
    public async Task<ActionResult<List<GetPollsDto>>> GetAllPolls()
    {
        var userId = User.GetUserId();
        var instanceId = User.GetUserInstanceId();
        var query = new GetPollsQuery(instanceId, userId, true);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<List<GetPollsDto>>()),
            f => Problem(f));
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetPollsDto>> GetPollById(int id)
    {
        var userId = User.GetUserId();
        var query = new GetPollsByIdQuery(id, userId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<GetPollsDto>()),
            f => Problem(f));
    }



    [Authorize(Roles = "Admin")]
    [HttpGet("Summary/{id:int}")]
    public async Task<ActionResult> GetSummary(int id)
    {
        var query = new GetPollResultQuery(id);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    
    [Authorize(Roles = "Admin")]
    [HttpPut("Edit/{id:int}")]
    public async Task<ActionResult> EditPoll(int id, PollUpdateDto updateDto)
    {
        var command = new UpdatePollCommand(
            id,
            updateDto.Title,
            updateDto.PollType,
            updateDto.Question,
            updateDto.Choices.Adapt<List<PollChoiceRequest>>(),
            updateDto.PollState,
            updateDto.isDeleted);
        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }


    
}
