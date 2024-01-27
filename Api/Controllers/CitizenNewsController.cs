using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.NewsApp.Queries.GetNews;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class CitizenNewsController : ApiController
{
    public CitizenNewsController(ISender sender) : base(sender)
    {
    }


    [Authorize(Roles = "Citizen")]
    [HttpGet("News")]
    public async Task<ActionResult<List<GetNewsDto>>> GetNews(int instanceId)
    {
        var query = new GetNewsQuery(instanceId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<List<GetNewsDto>>()),
            f => Problem(f));
    }



}
