using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Categories.Queries.GetCategory;
using Application.Configurations.Queries.ShahrbinInstanceManagement;
using Application.Configurations.Queries.ViolationTypes;
using Domain.Models.Relational;
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
    public async Task<ActionResult> GetCategories(int? instanceId)
    {
        if (instanceId is null)
            return BadRequest();
        var roles = User.GetUserRoles();
        var query = new GetCategoryQuery(instanceId.Value, roles);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
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




