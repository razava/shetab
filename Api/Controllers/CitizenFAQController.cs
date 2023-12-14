using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Faqs.Queries.GetFaqQuery;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[Route("api/{instanceId}/[controller]")]
[ApiController]
public class CitizenFAQController : ApiController
{
    protected CitizenFAQController(ISender sender) : base(sender)
    {
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet]
    public async Task<ActionResult<List<GetFaqsDto>>> GetFaqs()
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetFaqQuery(instanceId);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List<GetFaqsDto>>();
        return Ok(mappedResult);
    }


}
