using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Categories.Queries.GetCategory;
using Application.Configurations.Queries.ShahrbinInstances;
using Application.Configurations.Queries.ViolationTypes;
using Domain.Models.Relational.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CitizenCommonController : ApiController
{
    public CitizenCommonController(ISender sender) : base(sender)
    {
    }


    //todo : define access policies


    [Authorize]
    [HttpGet("Categories/{instanceId:int}")]
    public async Task<ActionResult> GetCategories(int instanceId)
    {
        var roles = User.GetUserRoles();
        var query = new GetCategoryQuery(instanceId, roles);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    
    [Authorize]
    [HttpGet("ViolationTypes")]
    public async Task<ActionResult> GetViolationTypes()
    {
        var query = new ViolationTypesQuery();
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    //[Authorize]
    [HttpGet("ShahrbinInstances")]
    public async Task<ActionResult> GetShahrbinInstances()
    {
        var query = new ShahrbinInstancesQuery();
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("Educations")]
    public ActionResult GetEducations()
    {
        var result = new List<EducationDto>();
        foreach (var item in Enum.GetValues(typeof(Education)).Cast<Education>())
        {
            result.Add(new EducationDto((int)item, item.GetDescription()!));
        }
        return Ok(Result.Ok(result));
    }



}




