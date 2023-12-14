using Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class ManagementController : ApiController
{
    public ManagementController(ISender sender) : base(sender)
    {
    }

    [Authorize(Roles = "PowerUser")]
    [HttpPost("Feedback/SendNow")]
    public async Task<ActionResult> SendNowFeedback()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "PowerUser")]
    [HttpGet("DeleteReport/{trackingNumbers}")]
    public async Task<ActionResult> DeleteReport(string trackingNumbers)
    {
        await Task.CompletedTask;//.................
        return Ok();
    }


}
