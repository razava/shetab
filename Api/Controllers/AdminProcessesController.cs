using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Processes.Commands.AddProcessCommand;
using Application.Processes.Commands.UpdateProcessCommand;
using Application.Processes.Queries.GetExecutiveActorsQuery;
using Application.Processes.Queries.GetProcessByIdQuery;
using Application.Processes.Queries.GetProcessesQuery;
using Mapster;
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
    public async Task<ActionResult<List<GetProcessListDto>>> GetProcesses([FromQuery]QueryFilter queryFilter)
    {
        var query = new GetProcessesQuery();
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List<GetProcessListDto>>();
        return Ok(mappedResult);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateProcesses(SetProcessDto setProcessDto)
    {
        var InstanceId = User.GetUserInstanceId();
        var command = new AddProcessCommand(
            InstanceId,
            setProcessDto.Code,
            setProcessDto.Title,
            setProcessDto.ActorIds);
        var result = await Sender.Send(command);
        if (!result)
            return Problem();
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetProcessDto>> GetProcessById(int id)
    {
        var query = new GetProcessByIdQuery(id);
        var result = await Sender.Send(query);
        if(result == null) return Problem();
        var mappedResult = result.Adapt<GetProcessDto>();
        return Ok(mappedResult);
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditProcesses(int id, SetProcessDto setProcessDto)
    {
        var command = new UpdateProcessCommand(
            id,
            setProcessDto.Code,
            setProcessDto.Title,
            setProcessDto.ActorIds);
        var result = await Sender.Send(command);
        if (!result)
            return Problem();
        return Ok();
    }

    //TODO: Define access policies
    [Authorize]
    [HttpGet("Executives")]
    public async Task<ActionResult<List<GetExecutiveListDto>>> GetExecutives()
    {
        var query = new GetExecutiveActorsQuery();
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List<GetExecutiveListDto>>();
        return Ok(mappedResult);
    }
    


}
