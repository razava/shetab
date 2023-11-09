using Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class AdminProcessesController : ApiController
{
    protected AdminProcessesController(ISender sender) : base(sender)
    {
    }

    //todo : Define & Set Dtos


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult> GetProcesses()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateProcesses()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetProcesse(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditProcesses(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //TODO: Define access policies
    [Authorize]
    [HttpGet("Executives")]
    public async Task<ActionResult> GetExecutives()
    {
        await Task.CompletedTask;
        return Ok();
    }
    


}
