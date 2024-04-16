using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Polls.Commands.AnswerPoll;
using Application.Polls.Queries.GetPolls;
using Application.Polls.Queries.GetPollsById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CitizenPollsController : ApiController
{
    public CitizenPollsController(ISender sender) : base(sender)
    {
    }

    //todo : Define & Set Dtos

    [Authorize(Roles = "Citizen")]
    [HttpGet("Polls/{instanceId:int}")]
    public async Task<ActionResult> GetPolls(int instanceId)
    {
        var userId = User.GetUserId();
        var query = new GetPollsQuery(instanceId, userId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetPollById(int id)
    {
        var userId = User.GetUserId();
        var query = new GetPollsByIdQuery(id, userId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    
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
            s => Ok(s),
            f => Problem(f));
    }


}
