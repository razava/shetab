using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Polls.Commands.AnswerPollCommand;
using Application.Polls.Queries.GetPollsByIdQuery;
using Application.Polls.Queries.GetPollsQuery;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class CitizenPollsController : ApiController
{
    public CitizenPollsController(ISender sender) : base(sender)
    {
    }

    //todo : Define & Set Dtos

    [Authorize(Roles = "Citizen")]
    [HttpGet("Polls")]
    public async Task<ActionResult<List<GetPollsDto>>> GetPolls(int instanceId)
    {
        var userId = User.GetUserId();
        var query = new GetPollsQuery(instanceId, userId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<List<GetPollsDto>>()),
            f => Problem(f));
        //var mappedResult = result.Adapt<List<GetPollsDto>>();
        //return Ok(mappedResult);
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetPollsDto>> GetPollById(int id)
    {
        var userId = User.GetUserId();
        var query = new GetPollsByIdQuery(id, userId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<GetPollsDto>()),
            f => Problem(f));
        //if (result == null)
        //    return Problem();
        //var mappedResult = result.Adapt<GetPollsDto>();
        //return Ok(mappedResult);
    }

    //[Authorize(Roles = "Citizen")]
    //[HttpGet("Polls/{id:int}")]
    //public async Task<ActionResult> GetPollById(int id)
    //{
    //    await Task.CompletedTask;
    //    return Ok();
    //}

    [Authorize(Roles = "Citizen")]
    [HttpPost("Polls/Answer/{id:int}")]
    public async Task<ActionResult> AnswerPoll(int id, AnswerToPollDto answerDto)
    {
        var userId = User.GetUserId();
        var commad = new AnswerPollCommand(
            userId,
            id,
            answerDto.Text,
            answerDto.ChoicesIds);
        var result = await Sender.Send(commad);

        return result.Match(
            s => Ok(),
            f => Problem(f));
    }


}
