using Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]

public class TaskController : ApiController
{
    protected TaskController(ISender sender) : base(sender)
    {
    }


    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetTasks()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize]
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult> GetTaskById(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //TODO: Define access policy
    [Authorize(Roles = "Operator")]
    [HttpGet("PossibleTransitions/{id:Guid}")]
    public async Task<ActionResult> GetPossibleTransitions(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //TODO: Define access policy
    [Authorize]
    [HttpPost("MakeTransition/{id:Guid}")]
    public async Task<ActionResult> MakeTransition(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //TODO: Define access policy
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




}
