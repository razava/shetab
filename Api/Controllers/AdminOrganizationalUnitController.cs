using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Common.FilterModels;
using Application.OrganizationalUnits.Commands.AddOrganizationalUnit;
using Application.OrganizationalUnits.Commands.DeleteOrganizationalUnit;
using Application.OrganizationalUnits.Commands.UpdateOrganizationalUnit;
using Application.OrganizationalUnits.Queries.GetOrganizationalUnitById;
using Application.OrganizationalUnits.Queries.GetOrganizationalUnits;
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
    public async Task<ActionResult> GetAllOrgaizationalUnits([FromQuery]QueryFilter queryFilter)
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetOrganizationalUnitsQuery(instanceId, queryFilter.Adapt<QueryFilterModel>());
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetOrgaizationalUnitById(int id)
    {
        var query = new GetOrganizationalUnitByIdQuery(id);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
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
            s => CreatedAtAction(nameof(GetOrgaizationalUnitById), new { id = s.Value.Id, instanceId = instanceId }, s),
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
            s => Ok(s),
            f => Problem(f));
    }



    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteOrganizationalUnit(int id)
    {
        var command = new DeleteOrganizationalUnitCommand(id);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


}
