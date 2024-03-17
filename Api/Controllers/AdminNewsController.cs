using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.NewsApp.Commands.AddNews;
using Application.NewsApp.Commands.UpdateNews;
using Application.NewsApp.Queries.GetNews;
using Application.NewsApp.Queries.GetNewsById;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class AdminNewsController : ApiController
{
    public AdminNewsController(ISender sender) : base(sender)
    {
    }


    //TODO: Define access policy
    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetNews(PagingInfo pagingInfo)
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetNewsQuery(pagingInfo, instanceId, true);
        var result = await Sender.Send(query);

        Response.AddPaginationHeaders(result.Value.Meta);

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


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateNews([FromForm] CreateNewsDto createNewsDto)
    {
        var instanceId = User.GetUserInstanceId();
        var command = new AddNewsCommand(
            instanceId,
            createNewsDto.Title,
            createNewsDto.Description,
            createNewsDto.Url,
            createNewsDto.Image,
            createNewsDto.IsDeleted);
        var result = await Sender.Send(command);

        return result.Match(
            s => CreatedAtAction(nameof(GetNewsById), new { id = s.Value.Id, instanceId = instanceId }, s),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditNews(int id, [FromForm] UpdateNewsDto setNewsDto)
    {
        var command = new UpdateNewsCommand(
            id,
            setNewsDto.Title,
            setNewsDto.Description,
            setNewsDto.Url,
            setNewsDto.Image,
            setNewsDto.IsDeleted);
        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }


}
