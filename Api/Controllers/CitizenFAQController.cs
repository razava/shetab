using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Faqs.Queries.GetFaq;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[Route("api/{instanceId}/[controller]")]
[ApiController]
public class CitizenFAQController : ApiController
{
    public CitizenFAQController(ISender sender) : base(sender)
    {
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet]
    public async Task<ActionResult<List<GetFaqsDto>>> GetFaqs(int instanceId)
    {
        var query = new GetFaqQuery(instanceId);
        var result = await Sender.Send(query);
        
        return result.Match(
            s => Ok(s.Adapt<List<GetFaqsDto>>()),
            f => Problem(f));
    }


}
