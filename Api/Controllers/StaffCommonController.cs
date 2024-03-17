using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.AdministrativeDivisions.Queries.GetRegion;
using Application.Categories.Queries.GetStaffCategories;
using Application.Configurations.Queries.Roles;
using Application.Configurations.Queries.ShahrbinInstanceManagement;
using Application.Configurations.Queries.ViolationTypes;
using Application.Info.Queries.GetReportFilters;
using Application.Info.Queries.GetUserFilters;
using Application.Users.Queries.GetUserRegions;
using Domain.Models.Relational.Common;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Errors;

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
    public async Task<ActionResult<List<ShahrbinInstance>>> GetInstances()
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
    public async Task<ActionResult<List<ShahrbinInstance>>> GetMyShahrbinInstance()
    {
        var instanceId = User.GetUserInstanceId();

        var query = new ShahrbinInstancesQuery();
        var result = await Sender.Send(query);
        if(result.IsFailed)   //todo : needed?
            return Problem(result.ToResult());

        var resultValue = result.Value;
        var instanceResult = resultValue.SingleOrDefault(result => result.Id == instanceId);

        if (instanceResult == null)
            return NotFound();
        return Ok(instanceResult);
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
    public async Task<ActionResult<List<GetRolesDto>>> GetRoles()
    {
        var query = new GetRolesQuery();
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<List<GetRolesDto>>()),
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
    public async Task<ActionResult<List<GetUserRegionsDto>>> GetUserRegions()
    {
        var instanceId = User.GetUserInstanceId();
        var userId = User.GetUserId();
        var query = new GetUserRegionsQuery(instanceId, userId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<List<GetUserRegionsDto>>()),
            f => Problem(f));
    }

    [Authorize]
    [HttpGet("ReportFilters")]
    public async Task<ActionResult> GetReportFilters()
    {
        var userId = User.GetUserId();
        var userRoles = User.GetUserRoles();
        int? instanceId = null;
        try
        {
            instanceId = User.GetUserInstanceId();
        }
        catch
        {
        }

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
        int? instanceId = null;
        try
        {
            instanceId = User.GetUserInstanceId();
        }
        catch
        {
        }
        if (instanceId is null)
            return Problem(NotFoundErrors.Instance);

        var query = new GetUserFiltersQuery(instanceId.Value);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }
}

