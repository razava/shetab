using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Common.FilterModels;
using Application.OrganizationalUnits.Commands.AddOrganizationalUnitCommand;
using Application.OrganizationalUnits.Commands.DeleteOrganizationalUnitCommand;
using Application.OrganizationalUnits.Commands.UpdateOrganizationalUnitCommand;
using Application.OrganizationalUnits.Queries.GetOrganizationalUnitByIdQuery;
using Application.OrganizationalUnits.Queries.GetOrganizationalUnitsQuery;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class AdminOrganizationalUnitController : ApiController
{
    public AdminOrganizationalUnitController(ISender sender) : base(sender)
    {
    }

    //TODO: Define access policy


    [Authorize]
    [HttpGet("All")]
    public async Task<ActionResult<List<GetOrganizationalUnitListDto>>> GetAllOrgaizationalUnits([FromQuery]QueryFilter queryFilter)
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetOrganizationalUnitsQuery(instanceId, queryFilter.Adapt<QueryFilterModel>());
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<List<GetOrganizationalUnitListDto>>()),
            f => Problem(f));
    }


    //todo : can move to Authenticate (??) cause relate to current User.
    //todo : ............check usage........................................not used :))
    //[Authorize]
    //[HttpGet]
    //public async Task<ActionResult> GetOrgaizationalUnitsOfUser()
    //{
    //    //...........todo : query shoul return a list?
    //    var userId = User.GetUserId();
    //    var query = new GetOrganizationalUnitByUserIdQuery(userId);
    //    var result = await Sender.Send(query);
    //    //var mappedResult = result.Adapt<>
    //    return Ok();
    //}

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetOrganizationalUnitDto>> GetOrgaizationalUnitById(int id)
    {
        var query = new GetOrganizationalUnitByIdQuery(id);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<GetOrganizationalUnitDto>()),
            f => Problem(f));
    }


    [Authorize(Roles ="Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateOrgaizationalUnit(OrganizationalUnitCreateDto createDto)
    {
        var instanceId = User.GetUserInstanceId();
        var command = new AddOrganizationalUnitCommand(
            instanceId,
            createDto.Title,
            createDto.Username,
            createDto.Password,
            createDto.ExecutiveActorsIds,
            createDto.OrganizationalUnitsIds);
        var result = await Sender.Send(command);

        return result.Match(
            s => CreatedAtAction(nameof(GetOrgaizationalUnitById), new { id = s.Id, instanceId = instanceId }, s.Adapt<GetOrganizationalUnitDto>()),
            f => Problem(f));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditOrgaizationalUnit(int id, OrganizationalUnitUpdateDto updateDto)
    {
        var command = new UpdateOrganizationalUnitCommand(
            id,
            updateDto.Title,
            updateDto.ExecutiveActorsIds,
            updateDto.OrganizationalUnitsIds);
        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }



    [Authorize(Roles = "Admin")]
    [HttpDelete]
    public async Task<ActionResult> DeleteOrganizationalUnit(int id)
    {
        var command = new DeleteOrganizationalUnitCommand(id);
        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }


}
