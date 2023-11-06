using Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class ViolationController : ApiController
{
    protected ViolationController(ISender sender) : base(sender)
    {
    }

    //todo : Define & Set Dtos
    //TODO: Define access policy

    //todo : this endpoint is shared with Citizen and can move to common Controller
    [Authorize(Roles = "Operator")]
    [HttpGet("Types")]
    public async Task<ActionResult> GetViolationTypes()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Operator")]
    [HttpGet]
    public async Task<ActionResult> GetViolations()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles ="Operator")]
    [HttpPut("{id:Guid}")]
    public async Task<ActionResult> GetViolations(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }


}
