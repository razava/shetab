using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.AdministrativeDivisions.Queries.GetProvince;
using Application.Categories.Queries.GetCategory;
using Application.Categories.Queries.GetCategoryById;
using Application.Configurations.Queries.ShahrbinInstanceManagement;
using Application.Configurations.Queries.ViolationTypes;
using Application.Users.Queries.GetRegions;
using Domain.Models.Relational;
using Domain.Models.Relational.Common;
using Domain.Models.Relational.ReportAggregate;
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
    public async Task<ActionResult<List<ShahrbinInstance>>> MyGetShahrbinInstance()
    {
        var instanceId = User.GetUserInstanceId();

        var query = new ShahrbinInstancesQuery();
        var result = await Sender.Send(query);
        var instanceResult = result.SingleOrDefault(result => result.Id == instanceId);
        //ShahrbinInstance instanceResult;
        //foreach (var instance in result)
        //{
        //    if (instance.Id == instanceId)
        //    {
        //        instanceResult = instance;
        //        break;
        //    }
        //}

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
        var root = result.Where(r => r.ParentId == null).Single();

        return root.Adapt<CategoryGetDto>();
    }

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

    /*
    [Authorize]
    [HttpGet("RegionsByName")]
    public async Task<ActionResult<List<GetRegionByName>>> GetRegionsByName()
    {
        await Task.CompletedTask;
        return Ok();
    }*/
    

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
    public ActionResult<List<EducationDto>> GetEducations()
    {
        var result = new List<EducationDto>();
        foreach (var item in Enum.GetValues(typeof(Education)).Cast<Education>())
        {
            result.Add(new EducationDto((int)item, item.GetDescription()!));
        }
        return Ok(result);
    }


    [Authorize]
    [HttpGet("TaradodReason")]
    public async Task<ActionResult<List<TaradodReason>>> GetTaradodReason()
    {
        await Task.CompletedTask;
        return Ok();
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

//public class CategoryGetDto
//{
//    public int ShahrbinInstanceId { get; set; }
//    public int Id { get; set; }
//    public int Order { get; set; }
//    public string Code { get; set; } = null!;
//    public string Title { get; set; } = null!;
//    public ICollection<CategoryGetDto> Categories { get; set; } = new List<CategoryGetDto>();
//    public int? ProcessId { get; set; }
//    public string Description { get; set; } = string.Empty;
//    public string AttachmentDescription { get; set; } = string.Empty;
//    public int Duration { get; set; }
//    public int? ResponseDuration { get; set; }
//    public CategoryType CategoryType { get; set; }
//    public bool IsDeleted { get; set; }
//    public ICollection<FormElement> FormElements { get; set; } = new List<FormElement>();
//    public bool HideMap { get; set; }
//}