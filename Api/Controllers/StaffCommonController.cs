using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.AdministrativeDivisions.Queries.GetRegion;
using Application.Categories.Queries.GetStaffCategories;
using Application.Configurations.Queries.Roles;
using Application.Configurations.Queries.ShahrbinInstanceById;
using Application.Configurations.Queries.ShahrbinInstances;
using Application.Configurations.Queries.ViolationTypes;
using Application.Forms.Queries.GetFormById;
using Application.Info.Queries.GetReportFilters;
using Application.Info.Queries.GetUserFilters;
using Application.Users.Queries.GetUserRegions;
using Domain.Models.Relational.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StaffCommonController : ApiController
{
    public StaffCommonController(ISender sender) : base(sender)
    {
    }

    //todo : define access policies

    [Authorize]
    [HttpGet("ShahrbinInstances")]
    public async Task<ActionResult> GetInstances()
    {
        var query = new ShahrbinInstancesQuery();
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    //todo : Define Access policy
    [Authorize]
    [HttpGet("MyShahrbinInstance")]
    public async Task<ActionResult> GetMyShahrbinInstance()
    {
        var instanceId = User.GetUserInstanceId();

        var query = new ShahrbinInstanceByIdQuery(instanceId);
        var result = await Sender.Send(query);
        
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }



    [Authorize]
    [HttpGet("Categories")]
    public async Task<ActionResult> GetCategory()
    {
        var userInstanceId = User.GetUserInstanceId();
        var userId = User.GetUserId();
        var userRoles = User.GetUserRoles();

        var query = new GetStaffCategoriesQuery(userInstanceId, userId, userRoles);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("Form/{id:guid}")]
    public async Task<ActionResult> GetFormById(Guid id)
    {
        var query = new GetFormByIdQuery(id);
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


    [Authorize]
    [HttpGet("Regions/{id}")]
    public async Task<ActionResult> GetRegionsByCityId(int id)
    {
        var query = new GetRegionQuery(id);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("Roles")]
    public async Task<ActionResult> GetRoles()
    {
        var query = new GetRolesQuery();
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
        return Ok(Result.Ok(result));
    }


    //todo : define access policeis
    [Authorize]
    [HttpGet("Executives")]  //will use in reportFilters and ChartFilters, should be filter for current user
    public async Task<ActionResult<List<GetExecutiveDto>>> GetExecutives()
    {
        await Task.CompletedTask;//..............
        return Ok("Not Implemented");
    }

    [Authorize]
    [HttpGet("UserRegions")]
    public async Task<ActionResult> GetUserRegions()
    {
        var instanceId = User.GetUserInstanceId();
        var userId = User.GetUserId();
        var query = new GetUserRegionsQuery(instanceId, userId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize]
    [HttpGet("ReportFilters")]
    public async Task<ActionResult> GetReportFilters()
    {
        var userId = User.GetUserId();
        var userRoles = User.GetUserRoles();
        var instanceId = User.GetUserInstanceId();

        var query = new GetReportFiltersQuery(instanceId, userId, userRoles);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize]
    [HttpGet("UserFilters")]
    public async Task<ActionResult> GetUserFilters()
    {
        var instanceId = User.GetUserInstanceId();

        var query = new GetUserFiltersQuery(instanceId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }
}

