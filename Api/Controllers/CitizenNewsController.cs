using Api.Abstractions;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.NewsApp.Queries.GetNews;
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
    public async Task<ActionResult> GetNews([FromQuery] PagingInfo pagingInfo, int instanceId)
    {
        var query = new GetNewsQuery(pagingInfo, instanceId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }



}
