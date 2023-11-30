using Api.Abstractions;
using Api.Contracts;
using Application.AdministrativeDivisions.Queries.GetProvince;
using Application.Categories.Queries.GetCategory;
using Application.Categories.Queries.GetCategoryById;
using Application.Configurations.Queries.ShahrbinInstanceManagement;
using Application.Configurations.Queries.ViolationTypes;
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
    public CitizenCommonController(ISender sender) : base(sender)
    {
    }


    //todo : define access policies


    [Authorize]
    [HttpGet("Categories")]
    public async Task<ActionResult<List<CategoryGetDto>>> GetCategories(int? instanceId)
    {
        if (instanceId is null)
            return BadRequest();
        var query = new GetCategoryQuery(instanceId.Value);
        var result = await Sender.Send(query);

        return result.Adapt<List<CategoryGetDto>>();
    }

    //..........................is needed for citizen?........
    //........... review dto.................
    //[HttpGet("Categories/{id}")]
    //public async Task<CategoryGetDto> GetCategory(int id)
    //{
    //    var query = new GetCategoryByIdQuery(id);
    //    var result = await Sender.Send(query);
    //    return result.Adapt<CategoryGetDto>();
    //}


    [Authorize]
    [HttpGet("ViolationTypes")]
    public async Task<ActionResult<List<ViolationTypeDto>>> GetViolationTypes()
    {
        var query = new ViolationTypesQuery();
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List<ViolationTypeDto>>();
        return Ok(mappedResult);
    }

    [Authorize]
    [HttpGet("ShahrbinInstances")]
    public async Task<ActionResult<List<ShahrbinInstance>>> GetShahrbinInstances()
    {
        var query = new ShahrbinInstancesQuery();
        var result = await Sender.Send(query);
        return Ok(result);
    }


    [Authorize]
    [HttpGet("RegionsByName")]
    public async Task<ActionResult<List<GetRegionByName>>> GetRegionsByName()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize]
    [HttpGet("Regions/{id}")]
    public async Task<ActionResult<List<GetRegionDto>>> GetRegionsByCityId(int id)
    {
        var query = new GetRegionQuery(id);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List<GetRegionDto>>();
        return Ok(mappedResult);
    }


    [Authorize]
    [HttpGet("Educations")]
    public async Task<ActionResult<List<EducationDto>>> GetEducations()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize]
    [HttpGet("TaradodReason")]
    public async Task<ActionResult<List<TaradodReason>>> GetTaradodReason()
    {
        await Task.CompletedTask;
        return Ok();
    }


}




