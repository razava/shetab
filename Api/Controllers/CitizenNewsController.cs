using Api.Abstractions;
using Api.Contracts;
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


    [Authorize(Roles = "Citizen")]
    [HttpGet("News")]
    public async Task<ActionResult<List<GetNewsDto>>> GetNews()
    {
        await Task.CompletedTask;
        return Ok();
    }
}
