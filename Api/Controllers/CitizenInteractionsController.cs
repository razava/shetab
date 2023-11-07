using Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class CitizenInteractionsController : ApiController
{
    protected CitizenInteractionsController(ISender sender) : base(sender)
    {
    }

    //todo : Define & Set Dtos
    //TODO: Define access policy

    [Authorize]
    [HttpGet("Polls")]
    public async Task<ActionResult> GetPolls()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize]
    [HttpGet("Polls/{id:int}")]
    public async Task<ActionResult> GetPollById(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize]
    [HttpPost("Polls/Answer/{id:int}")]
    public async Task<ActionResult> AnswerPoll(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize]
    [HttpPost("Violation")]
    public async Task<ActionResult> RepotrViolation(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //this could be in Common controller
    [Authorize]
    [HttpGet("News")]
    public async Task<ActionResult> GetNews()
    {
        await Task.CompletedTask;
        return Ok();
    }

    //this could be in Common controller
    [Authorize]
    [HttpGet("ViolationTypes")]
    public async Task<ActionResult> GetViolationTypes()
    {
        await Task.CompletedTask;
        return Ok();
    }


}
