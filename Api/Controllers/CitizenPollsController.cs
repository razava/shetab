using Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class CitizenPollsController : ApiController
{
    protected CitizenPollsController(ISender sender) : base(sender)
    {
    }

    //todo : Define & Set Dtos

    [Authorize(Roles = "Citizen")]
    [HttpGet("Polls")]
    public async Task<ActionResult> GetPolls(int instanceId)
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Citizen")]
    [HttpGet("Polls/{id:int}")]
    public async Task<ActionResult> GetPollById(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    [HttpPost("Polls/Answer/{id:int}")]
    public async Task<ActionResult> AnswerPoll(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }


}
