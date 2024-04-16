using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Common.FilterModels;
using Application.Processes.Commands.AddProcess;
using Application.Processes.Commands.DeleteProcess;
using Application.Processes.Commands.UpdateProcess;
using Application.Processes.Queries.GetExecutiveActors;
using Application.Processes.Queries.GetProcessById;
using Application.Processes.Queries.GetProcesses;
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


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult> GetProcesses([FromQuery]QueryFilter queryFilter)
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetProcessesQuery(instanceId, queryFilter.Adapt<QueryFilterModel>());
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateProcesses(CreateProcessDto setProcessDto)
    {
        var InstanceId = User.GetUserInstanceId();
        var command = new AddProcessCommand(
            InstanceId,
            setProcessDto.Code,
            setProcessDto.Title,
            setProcessDto.ActorIds);
        var result = await Sender.Send(command);

        return result.Match(
            s => CreatedAtAction(nameof(GetProcessById), new { id = s.Value.Id, instanceId = InstanceId }, s),
            f => Problem(f));
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetProcessById(int id)
    {
        var query = new GetProcessByIdQuery(id);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditProcesses(int id, UpdateProcessDto setProcessDto)
    {
        var command = new UpdateProcessCommand(
            id,
            setProcessDto.Code,
            setProcessDto.Title,
            setProcessDto.ActorIds);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    //TODO: Define access policies
    [Authorize]
    [HttpGet("Executives")]
    public async Task<ActionResult> GetExecutives()
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetExecutiveActorsQuery(instanceId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProcess(int id)
    {
        var command = new DeleteProcessCommand(id);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

}
