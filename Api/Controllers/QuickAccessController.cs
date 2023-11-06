using Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class QuickAccessController : ApiController
{
    protected QuickAccessController(ISender sender) : base(sender)
    {
    }

    //todo : Define & Set Dtos

    //TODO: Define access policy
    //todo : this endpoint is shared with Citizen and can move to common Controller
    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetQuickAccesses()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateQuickAccess()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditQuickAccesses(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteQuickAccesses(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }
    

}
