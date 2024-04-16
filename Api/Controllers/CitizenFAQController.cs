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
    [HttpGet("/{instanceId:int}")]
    public async Task<ActionResult> GetFaqs(int instanceId)
    {
        var query = new GetFaqQuery(instanceId);
        var result = await Sender.Send(query);
        
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


}
