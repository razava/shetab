using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Faqs.Commands.AddFaqCommand;
using Application.Faqs.Commands.UpdateFaqCommand;
using Application.Faqs.Queries.GetFaqByIdQuery;
using Application.Faqs.Queries.GetFaqQuery;
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
        var query = new GetFaqQuery(instanceId, true);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<List<GetFaqsDto>>()),
            f => Problem(f));
    }
    

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetFaqsDto>> GetFaqById(int id)
    {
        var query = new GetFaqByIdQuery(id);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<GetFaqsDto>()),
            f => Problem(f));
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateFaq(CreateFaqDto createFaqDto)
    {
        var instanceId = User.GetUserInstanceId();
        var command = new AddFaqCommand(
            instanceId,
            createFaqDto.Question,
            createFaqDto.Answer,
            createFaqDto.IsDeleted);
        var result = await Sender.Send(command);

        return result.Match(
            s => CreatedAtAction(nameof(GetFaqById), new { id = s.Id, instanceId = instanceId }, s.Adapt<GetFaqsDto>()),
            f => Problem(f));
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateFaq(int id, UpdateFaqDto updateFaqDto)
    {
        var command = new UpdateFaqCommand(
            id,
            updateFaqDto.Question,
            updateFaqDto.Answer,
            updateFaqDto.IsDeleted);
        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }


}
