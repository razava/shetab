using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Categories.Queries.GetCategory;
using Application.Configurations.Queries.ShahrbinInstanceManagement;
using Application.Configurations.Queries.ViolationTypes;
using Domain.Models.Relational.Common;
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
    public async Task<ActionResult<CategoryGetDto>> GetCategories(int? instanceId)
    {
        if (instanceId is null)
            return BadRequest();
        var query = new GetCategoryQuery(instanceId.Value);
        var result = await Sender.Send(query);

        //TODO: This can be improved
        result.ForEach(c => c.Categories = result.Where(p => p.ParentId == c.Id).ToList());

        return result.Where(c => c.ParentId == null).Single().Adapt<CategoryGetDto>();
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

    //[Authorize]
    [HttpGet("ShahrbinInstances")]
    public async Task<ActionResult<List<ShahrbinInstance>>> GetShahrbinInstances()
    {
        var query = new ShahrbinInstancesQuery();
        var result = await Sender.Send(query);
        return Ok(result);
    }




    //[Authorize]
    //[HttpGet("Regions/{id}")]
    //public async Task<ActionResult<List<GetRegionDto>>> GetRegionsByCityId(int id)
    //{
    //    var query = new GetRegionQuery(id);
    //    var result = await Sender.Send(query);
    //    var mappedResult = result.Adapt<List<GetRegionDto>>();
    //    return Ok(mappedResult);
    //}


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



    /*
    [Authorize]
    [HttpGet("Enumeration/{name}")]
    public async Task<ActionResult<List<GetEnum>>> GetEnumeration(string name)
    {
        //object tempList = new List<Enum>();

        //switch (name)
        //{
        //    case "Education":
        //        tempList = Enum.GetValues(typeof(Education)).Cast<Education>();
        //        break;

        //    default:
        //        break;
        //}

        ////var tmp1 = "enum";
        ////var t22 = Type.GetType(tmp1);
        ////var tmp2 = tmp1.GetType();
        ////var temp = Enum.GetValues(t22);

        //var result = new List<GetEnum>();
        //foreach (var item in temp)
        //{
        //    result.Add(new GetEnum((int)item, item.GetDescription()!));
        //}
        //return Ok(result);
        await Task.CompletedTask;
        return Ok();
    }
    */


}




