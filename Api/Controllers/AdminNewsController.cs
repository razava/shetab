using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.NewsApp.Commands.AddNewsCommand;
using Application.NewsApp.Commands.UpdateNewsCommand;
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
    public async Task<ActionResult<List<GetNewsDto>>> GetNews()
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetNewsQuery(instanceId, true);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s.Adapt<List<GetNewsDto>>()),
            f => Problem(f));
        //var mappedResult = result.Adapt<List<GetNewsDto>>();
        //return Ok(mappedResult);
    }


    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetNewsDto>> GetNewsById(int id)
    {
        var query = new GetNewsByIdQuery(id);
        var result = await Sender.Send(query);
        //if (result == null)
        //    return Problem();
        //var mappedResult = result.Adapt<GetNewsDto>();
        //return Ok(mappedResult);
        return result.Match(
            s => Ok(s.Adapt<GetNewsDto>()),
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
            s => CreatedAtAction(nameof(GetNewsById), new { id = s.Id, instanceId = instanceId }, s.Adapt<GetNewsDto>()),
            f => Problem(f));

        //if (result == null)
        //    return Problem();
        //var routeValues = new { id = result.Id, instanceId = instanceId };
        //return CreatedAtAction(nameof(GetNewsById), routeValues, result.Adapt<GetNewsDto>());
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
        //if (result == null)
        //    return Problem();
        //return NoContent();
    }


    //[Authorize(Roles = "Admin")]
    //[HttpDelete("{id:int}")]
    //public async Task<ActionResult> DeleteNews(int id)
    //{
    //    await Task.CompletedTask;
    //    return Ok();
    //}


}
