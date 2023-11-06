using Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class NewsController : ApiController
{
    protected NewsController(ISender sender) : base(sender)
    {
    }

    //todo : Define & Set Dtos

    //TODO: Define access policy
    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetNews()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateNew()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditNews(int id)
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
