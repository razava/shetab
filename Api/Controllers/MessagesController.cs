using Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class MessagesController : ApiController
{
    protected MessagesController(ISender sender) : base(sender)
    {
    }

    //todo : Define & Set Dtos
    //TODO: Define access policy

    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetMessages()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize]
    [HttpGet("Count")]
    public async Task<ActionResult> GetMessagesCount()
    {
        await Task.CompletedTask;
        return Ok();
    }
    


}
