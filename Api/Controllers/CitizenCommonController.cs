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
        if (result.IsFailed)
        {
            return Problem(result.ToResult());
        }
        var resultValue = result.Value;
        //TODO: This can be improved
        resultValue.ForEach(c => c.Categories = resultValue.Where(p => p.ParentId == c.Id).ToList());

        return Ok(resultValue.Where(c => c.ParentId == null).Single().Adapt<CategoryGetDto>());
    }

    
    [Authorize]
    [HttpGet("ViolationTypes")]
    public async Task<ActionResult<List<ViolationTypeDto>>> GetViolationTypes()
    {
        var query = new ViolationTypesQuery();
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<List<ViolationTypeDto>>()),
            f => Problem(f));
    }


    //[Authorize]
    [HttpGet("ShahrbinInstances")]
    public async Task<ActionResult<List<ShahrbinInstance>>> GetShahrbinInstances()
    {
        var query = new ShahrbinInstancesQuery();
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
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




