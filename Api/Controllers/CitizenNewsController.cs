using Api.Abstractions;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.NewsApp.Queries.GetNews;
using Application.NewsApp.Queries.GetNewsById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CitizenNewsController : ApiController
{
    public CitizenNewsController(ISender sender) : base(sender)
    {
    }


    [Authorize(Roles = "Citizen")]
    [HttpGet("News/{instanceId:int}")]
    public async Task<ActionResult> GetNews(int instanceId, [FromQuery] PagingInfo pagingInfo)
    {
        var query = new GetNewsQuery(pagingInfo, instanceId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetNewsById(int id)
    {
        var query = new GetNewsByIdQuery(id);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


}
