using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.Communications.Commands;
using Application.Messages.Queries.GetMessageCount;
using Application.Messages.Queries.GetMessages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class MessagesController : ApiController
{
    public MessagesController(ISender sender) : base(sender)
    {
    }

    //todo : Define & Set Dtos
    //TODO: Define access policy

    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetMessages([FromQuery] PagingInfo pagingInfo)
    {
        var userId = User.GetUserId();
        var query = new GetMessagesQuery(pagingInfo, userId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("Count")]
    public async Task<ActionResult> GetMessagesCount([FromQuery] TimeFilter timeFilter)
    {
        var userId = User.GetUserId();
        var query = new GetMessageCountQuery(timeFilter.SentFromDate, userId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize]
    [HttpPost("ConnectionId")]
    public async Task<ActionResult> AddConnectionId([FromBody] string connectionId)
    {
        var userId = User.GetUserId();
        var command = new AddSignalRConnectionIdCommand(userId, connectionId);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

}
