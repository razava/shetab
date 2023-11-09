using Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class CitizenNewsController : ApiController
{
    protected CitizenNewsController(ISender sender) : base(sender)
    {
    }

    //todo : Define & Set Dtos

    [Authorize(Roles = "Citizen")]
    [HttpGet("News")]
    public async Task<ActionResult> GetNews()
    {
        await Task.CompletedTask;
        return Ok();
    }
}
