using Api.Abstractions;
using Application.Categories.Queries.GetCategory;
using Application.Categories.Queries.GetCategoryById;
using Domain.Models.Relational;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId:int}/[controller]")]
[ApiController]
public class CommonController : ApiController
{
    public CommonController(ISender sender) : base(sender)
    {
    }

    [HttpGet("Categories")]
    public async Task<ActionResult<List<Category>>> GetCategory(int? instanceId)
    {
        if (instanceId is null)
            return BadRequest();
        var query = new GetCategoryQuery(instanceId.Value);
        var result = await Sender.Send(query);
        return result;
    }

    [HttpGet("Categories/{id}")]
    public async Task<Category> GetCategory(int id)
    {
        var query = new GetCategoryByIdQuery(id);
        var result = await Sender.Send(query);
        return result;
    }

}
