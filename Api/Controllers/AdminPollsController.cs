using Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class AdminPollsController : ApiController
{
    protected AdminPollsController(ISender sender) : base(sender)
    {
    }

    //todo : Define & Set Dtos


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreatePoll()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("All")]
    public async Task<ActionResult> GetAllPolls()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Summary/{id:int}")]
    public async Task<ActionResult> GetSummary(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpPost("Attach/{id:int}")]
    public async Task<ActionResult> AttachToPoll(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("Edit/{id:int}")]
    public async Task<ActionResult> EditPoll(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("Status/{id:int}")]
    public async Task<ActionResult> UpdatePollStatus(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }
    


}
