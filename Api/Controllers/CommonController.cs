using Api.Abstractions;
using Application.Categories.Queries.GetCategory;
using Application.Categories.Queries.GetCategoryById;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ReportAggregate;
using Mapster;
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
    public async Task<ActionResult<List<CategoryGetDto>>> GetCategory(int? instanceId)
    {
        if (instanceId is null)
            return BadRequest();
        var query = new GetCategoryQuery(instanceId.Value);
        var result = await Sender.Send(query);

        return result.Adapt<List<CategoryGetDto>>();
    }

    [HttpGet("Categories/{id}")]
    public async Task<CategoryGetDto> GetCategory(int id)
    {
        var query = new GetCategoryByIdQuery(id);
        var result = await Sender.Send(query);
        return result.Adapt<CategoryGetDto>();
    }

}

public class CategoryGetDto
{
    public int ShahrbinInstanceId { get; set; }
    public int Id { get; set; }
    public int Order { get; set; }
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public ICollection<CategoryGetDto> Categories { get; set; } = new List<CategoryGetDto>();
    public int? ProcessId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string AttachmentDescription { get; set; } = string.Empty;
    public int Duration { get; set; }
    public int? ResponseDuration { get; set; }
    public CategoryType CategoryType { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<FormElement> FormElements { get; set; } = new List<FormElement>();
    public bool HideMap { get; set; }
}