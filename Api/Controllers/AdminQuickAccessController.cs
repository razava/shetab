﻿using Api.Abstractions;
using Api.Contracts;
using Application.Configurations.Queries.QuickAccesses;
using Application.QuickAccesses.Commands.AddQuickAccessCommand;
using Application.QuickAccesses.Commands.UpdateQuickAccessCommand;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class AdminQuickAccessController : ApiController
{
    protected AdminQuickAccessController(ISender sender) : base(sender)
    {
    }


    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<AdminGetQuickAccess>>> GetQuickAccesses(int instanceId, [FromQuery] QueryFilter queryFilter)
    {
        var query = new QuickAccessQuery(instanceId);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<AdminGetQuickAccess>();
        return Ok(mappedResult);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateQuickAccess(int instanceId, SetQuickAccessDto setDto)
    {
        var command = new AddQuickAccessCommand(
            instanceId,
            setDto.CategoryId,
            setDto.Title,
            setDto.MediaId,
            setDto.Order,
            false);
        var result = await Sender.Send(command);
        if (result == null)
            return Problem();
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditQuickAccesses(int id, SetQuickAccessDto setDto)
    {
        var command = new UpdateQuickAccessCommand(
            id,
            setDto.CategoryId,
            setDto.Title,
            setDto.MediaId,
            setDto.Order,
            false);
        var result = await Sender.Send(command);
        if(result == null)
            return Problem();
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteQuickAccesses(int id)
    {
        var command = new UpdateQuickAccessCommand(id, null, null, null, null, true);
        var result = await Sender.Send(command);
        if (result == null)
            return Problem();
        return Ok();
    }
    

}
