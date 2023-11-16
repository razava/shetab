using Api.Abstractions;
using Api.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class AdminProcessesController : ApiController
{
    public AdminProcessesController(ISender sender) : base(sender)
    {
    }


    //this endpoint is duplicate with GetProcesses in AdminCategoryController
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<GetProcessListDto>>> GetProcesses(QueryFilter queryFilter)
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateProcesses(SetProcessDto setProcessDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetProcessDto>> GetProcessById(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditProcesses(int id, SetProcessDto setProcessDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //TODO: Define access policies
    [Authorize]
    [HttpGet("Executives")]
    public async Task<ActionResult<List<GetExecutiveListDto>>> GetExecutives()
    {
        await Task.CompletedTask;
        return Ok();
    }
    


}
