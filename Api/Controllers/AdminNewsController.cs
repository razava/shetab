using Api.Abstractions;
using Api.Contracts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class AdminNewsController : ApiController
{
    protected AdminNewsController(ISender sender) : base(sender)
    {
    }


    //TODO: Define access policy
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<GetNewsDto>>> GetNews()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateNew(CreateNewsDto createNewsDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditNews(int id, UpdateNewsDto updateNewsDto)
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteNews(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }


}
