﻿using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Faqs.Commands.AddFaqCommand;
using Application.Faqs.Commands.UpdateFaqCommand;
using Application.Faqs.Queries.GetFaqQuery;
using Application.NewsApp.Queries.UpdateNewsCommand;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]

public class StaffFaqController : ApiController
{
    public StaffFaqController(ISender sender) : base(sender)
    {
    }


    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<GetFaqsDto>>> GetFaqs()
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetFaqQuery(instanceId);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List<GetFaqsDto>>();
        return Ok(mappedResult);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateFaq(SetFaqDto createFaqDto)
    {
        var instanceId = User.GetUserInstanceId();
        var command = new AddFaqCommand(
            instanceId,
            createFaqDto.Question,
            createFaqDto.Answer,
            createFaqDto.IsDeleted);
        var result = await Sender.Send(command);
        if (result == null)
            return Problem();
        return CreatedAtAction("", result.Adapt<GetFaqsDto>());
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateFaq(int id, SetFaqDto updateFaqDto)
    {
        var command = new UpdateFaqCommand(
            id,
            updateFaqDto.Question,
            updateFaqDto.Answer,
            updateFaqDto.IsDeleted);
        var result = await Sender.Send(command);
        if (result == null) return Problem();
        return NoContent();
    }


}
