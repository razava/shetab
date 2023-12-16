using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Polls.Commands.AddPollCommand;
using Application.Polls.Commands.UpdatePollCommand;
using Application.Polls.Common;
using Application.Polls.Queries.GetPollResultQuery;
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

    //todo : Define & Set Dtos


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
        if (result == null)
            return Problem();
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("All")]
    public async Task<ActionResult<List<GetPollsDto>>> GetAllPolls()
    {
        var userId = User.GetUserId();
        var instanceId = User.GetUserInstanceId();
        var query = new GetPollsQuery(instanceId, userId, true);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List<GetPollsDto>>();
        return Ok(mappedResult);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Summary/{id:int}")]
    public async Task<ActionResult<PollResultDto>> GetSummary(int id)
    {
        var query = new GetPollResultQuery(id);
        var result = await Sender.Send(query);
        if (result == null) return Problem();
        var mappedResult = result.Adapt<PollResultDto>();
        return Ok(mappedResult);
    }


    //[Authorize(Roles = "Admin")]
    //[HttpPost("Attach/{id:int}")]
    //public async Task<ActionResult> AttachToPoll(int id)
    //{
    //    await Task.CompletedTask;
    //    return Ok();
    //}

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
            updateDto.PollState);
        var result = await Sender.Send(command);
        if (result == null)
            return Problem();
        return NoContent();
    }


    //[Authorize(Roles = "Admin")]
    //[HttpPut("Status/{id:int}")]
    //public async Task<ActionResult> UpdatePollStatus(int id)
    //{
    //    await Task.CompletedTask;
    //    return Ok();
    //}
    


}
