using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.AdministrativeDivisions.Queries.GetProvince;
using Application.Categories.Queries.GetCategory;
using Application.Configurations.Queries.ShahrbinInstanceManagement;
using Application.Configurations.Queries.ViolationTypes;
using Application.Users.Queries.GetRegions;
using Domain.Models.Relational.Common;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId:int}/[controller]")]
[ApiController]
public class StaffCommonController : ApiController
{
    public StaffCommonController(ISender sender) : base(sender)
    {
    }

    //todo : define access policies

    [Authorize]
    [HttpGet("ShahrbinInstances")]
    public async Task<IActionResult> GetInstances()
    {
        var query = new ShahrbinInstancesQuery();
        var result = await Sender.Send(query);
        return Ok(result);
    }

    //todo : Define Access policy
    [Authorize]
    [HttpGet("MyShahrbinInstance")]
    public async Task<ActionResult<List<ShahrbinInstance>>> GetMyShahrbinInstance()
    {
        var instanceId = User.GetUserInstanceId();

        var query = new ShahrbinInstancesQuery();
        var result = await Sender.Send(query);
        var instanceResult = result.SingleOrDefault(result => result.Id == instanceId);

        if (instanceResult == null)
            return NotFound();
        return Ok(instanceResult);
    }



    [Authorize]
    [HttpGet("Categories")]
    public async Task<ActionResult<CategoryGetDto>> GetCategory(int? instanceId)
    {
        if (instanceId is null)
            return BadRequest();
        var query = new GetCategoryQuery(instanceId.Value);
        var result = await Sender.Send(query);
        result.ForEach(x => x.Categories = result.Where(c => c.ParentId == x.Id).ToList());
        var root = result.Where(r => r.ParentId == null).Single();

        return root.Adapt<CategoryGetDto>();
    }


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
    [HttpGet("Regions/{id}")]
    public async Task<ActionResult<List<GetRegionDto>>> GetRegionsByCityId(int id)
    {
        var query = new GetRegionQuery(id);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List<GetRegionDto>>();
        return Ok(mappedResult);
    }


    [Authorize]
    [HttpGet("Roles")]
    public async Task<ActionResult<List<GetRolesDto>>> GetRoles()
    {
        await Task.CompletedTask;//.......................
        return Ok();
    }


    [Authorize]
    [HttpGet("Educations")]
    public ActionResult<List<EducationDto>> GetEducations()
    {
        var result = new List<EducationDto>();
        foreach (var item in Enum.GetValues(typeof(Education)).Cast<Education>())
        {
            result.Add(new EducationDto((int)item, item.GetDescription()!));
        }
        return Ok(result);
    }


    //todo : define access policeis
    [Authorize]
    [HttpGet("Executives")]  //will use in reportFilters and ChartFilters, should be filter for current user
    public async Task<ActionResult<List<GetExecutiveDto>>> GetExecutives()
    {
        await Task.CompletedTask;//..............
        return Ok();
    }

    [Authorize]
    [HttpGet("UserRegions/{id}")]
    public async Task<ActionResult<List<IsInRegionDto>>> GetUserRegions(string id)
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetUserRegionsQuery(instanceId, id);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List<IsInRegionDto>>();
        return Ok(mappedResult);
    }

}

