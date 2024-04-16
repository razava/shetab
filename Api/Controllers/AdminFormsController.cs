using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Forms.Commands.AddForm;
using Application.Forms.Commands.DeleteForm;
using Application.Forms.Commands.UpdateForm;
using Application.Forms.Queries.GetForm;
using Application.Forms.Queries.GetFormById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminFormsController : ApiController
{
    public AdminFormsController(ISender sender) : base(sender)
    {
    }

    //TODO: Define access policy
    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetForms()
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetFormQuery(instanceId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetFormById(Guid id)
    {
        var query = new GetFormByIdQuery(id);
        var result = await Sender.Send(query);
        
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateForm(CreateFormDto createFormDto)
    {
        var instanceId = User.GetUserInstanceId();
        var command = new AddFormCommand(
            instanceId,
            createFormDto.Title,
            createFormDto.Elements);
        var result = await Sender.Send(command);

        return result.Match(
            s => CreatedAtAction(nameof(GetFormById), new { id = s.Value.Id }, s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> EditNews(Guid id, UpdateFormDto updateFormDto)
    {
        var command = new UpdateFormCommand(
            id,
            updateFormDto.Title,
            updateFormDto.Elements);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteNews(Guid id)
    {
        var command = new DeleteFormCommand(id);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }
}
