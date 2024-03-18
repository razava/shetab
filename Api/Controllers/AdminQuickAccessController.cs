using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Common.FilterModels;
using Application.QuickAccesses.Commands.AddQuickAccess;
using Application.QuickAccesses.Commands.UpdateQuickAccess;
using Application.QuickAccesses.Queries.GetQuickAccessById;
using Application.QuickAccesses.Queries.GetQuickAccesses;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class AdminQuickAccessController : ApiController
{
    public AdminQuickAccessController(ISender sender) : base(sender)
    {
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult> GetQuickAccesses(int instanceId, [FromQuery] QueryFilter queryFilter)
    {
        var query = new GetQuickAccessesQuery(instanceId, true, queryFilter.Adapt<QueryFilterModel>());
        var result = await Sender.Send(query);
        
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetQuickAccessById(int id)
    {
        var query = new GetQuickAccessByIdQuery(id);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateQuickAccess(int instanceId, [FromForm] CreateQuickAccessDto setDto)
    {
        var command = new AddQuickAccessCommand(
            instanceId,
            setDto.CategoryId,
            setDto.Title,
            setDto.Image,
            setDto.Order,
            false);
        var result = await Sender.Send(command);

        return result.Match(
            s => CreatedAtAction(nameof(GetQuickAccessById), new { id = s.Value.Id, instanceId = instanceId }, s),
            f => Problem(f));
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditQuickAccesses(int id, [FromForm] UpdateQuickAccessDto setDto)
    {
        var command = new UpdateQuickAccessCommand(
            id,
            setDto.CategoryId,
            setDto.Title,
            setDto.Image,
            setDto.Order,
            setDto.IsDeleted);
        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }
}
