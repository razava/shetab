using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.NewsApp.Commands.AddNewsCommand;
using Application.NewsApp.Commands.UpdateNewsCommand;
using Application.NewsApp.Queries.UpdateNewsCommand;
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
        var query = new GetNewsQuery(instanceId);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List<GetNewsDto>>();
        return Ok(mappedResult);
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateNews(SetNewsDto createNewsDto)
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
        if (result == null)
            return Problem();
        //return Ok();
        return CreatedAtAction("", result.Adapt<GetNewsDto>());
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditNews(int id, SetNewsDto setNewsDto)
    {
        var command = new UpdateNewsCommand(
            id,
            setNewsDto.Title,
            setNewsDto.Description,
            setNewsDto.Url,
            setNewsDto.Image,
            setNewsDto.IsDeleted);
        var result = await Sender.Send(command);
        if (result == null)
            return Problem();
        return NoContent();
    }


    //[Authorize(Roles = "Admin")]
    //[HttpDelete("{id:int}")]
    //public async Task<ActionResult> DeleteNews(int id)
    //{
    //    await Task.CompletedTask;
    //    return Ok();
    //}


}
