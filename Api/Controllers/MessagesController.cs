using Api.Abstractions;
using Api.Contracts;
using Application.Common.Interfaces.Persistence;
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
    public async Task<ActionResult<List<GetMessageDto>>> GetMessages([FromQuery] PagingInfo pagingInfo)
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize]
    [HttpGet("Count")]
    public async Task<ActionResult<GetMessageCountDto>> GetMessagesCount([FromQuery] TimeFilter timeFilter)
    {
        await Task.CompletedTask;
        return Ok();
    }
    


}
