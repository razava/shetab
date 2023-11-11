using Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class ManagementController : ApiController
{
    protected ManagementController(ISender sender) : base(sender)
    {
    }

    [Authorize]
    [HttpPost("Feedback/SendNow")]
    public async Task<ActionResult> SendNowFeedback()
    {
        await Task.CompletedTask;
        return Ok();
    }



}
