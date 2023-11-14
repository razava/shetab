using Api.Abstractions;
using Api.Contracts;
using Application.Categories.Queries.GetCategory;
using Application.Categories.Queries.GetCategoryById;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ReportAggregate;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId:int}/[controller]")]
[ApiController]
public class CitizenCommonController : ApiController
{
    protected CitizenCommonController(ISender sender) : base(sender)
    {
    }


    //........... review dto.................
    [HttpGet("Categories")]
    public async Task<ActionResult<List<CategoryGetDto>>> GetCategory(int? instanceId)
    {
        if (instanceId is null)
            return BadRequest();
        var query = new GetCategoryQuery(instanceId.Value);
        var result = await Sender.Send(query);

        return result.Adapt<List<CategoryGetDto>>();
    }

    //..........................is needed for citizen?........
    //........... review dto.................
    [HttpGet("Categories/{id}")]
    public async Task<CategoryGetDto> GetCategory(int id)
    {
        var query = new GetCategoryByIdQuery(id);
        var result = await Sender.Send(query);
        return result.Adapt<CategoryGetDto>();
    }


    [Authorize]
    [HttpGet("ViolationTypes")]
    public async Task<ActionResult<List<ViolationTypeDto>>> GetViolationTypes()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [HttpGet("RegionsByName")]
    public async Task<ActionResult<List<GetRegionByName>>> GetRegionsByName()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet("Educations")]
    public async Task<ActionResult<List<EducationDto>>> GetEducations()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet("TaradodReason")]
    public async Task<ActionResult<List<TaradodReason>>> GetTaradodReason()
    {
        await Task.CompletedTask;
        return Ok();
    }


}




