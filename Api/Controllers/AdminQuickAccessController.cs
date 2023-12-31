﻿using Api.Abstractions;
using Api.Contracts;
using Application.Common.FilterModels;
using Application.QuickAccesses.Commands.AddQuickAccessCommand;
using Application.QuickAccesses.Commands.UpdateQuickAccessCommand;
using Application.QuickAccesses.Queries.GetQuickAccessByIdQuery;
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
    public async Task<ActionResult<List<AdminGetQuickAccess>>> GetQuickAccesses(int instanceId, [FromQuery] QueryFilter queryFilter)
    {
        var query = new GetQuickAccessesQuery(instanceId, queryFilter.Adapt<QueryFilterModel>(), true);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List<AdminGetQuickAccess>>();
        return Ok(mappedResult);
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<AdminGetQuickAccess>> GetQuickAccessById(int id)
    {
        var query = new GetQuickAccessByIdQuery(id);
        var result = await Sender.Send(query);
        if (result == null)
            return Problem();
        var mappedResult = result.Adapt<AdminGetQuickAccess>();
        return Ok(mappedResult);
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
        if (result == null)
            return Problem();
        //return Created("",result.Id);
        var routeValues = new { id = result.Id, instanceId = instanceId };
        return CreatedAtAction(nameof(GetQuickAccessById), routeValues, result.Adapt<AdminGetQuickAccess>());
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
        if(result == null)
            return Problem();
        return NoContent();
    }


    //[Authorize(Roles = "Admin")]
    //[HttpDelete("{id:int}")]
    //public async Task<ActionResult> DeleteQuickAccesses(int id)
    //{
    //    var command = new UpdateQuickAccessCommand(id, null, null, null, null, true);
    //    var result = await Sender.Send(command);
    //    if (result == null)
    //        return Problem();
    //    return NoContent();
    //}
    

}
