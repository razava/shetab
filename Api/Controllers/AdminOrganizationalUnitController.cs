using Api.Abstractions;
using Api.Contracts;
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
    public async Task<ActionResult<List<GetOrganizationalUnitListDto>>> GetAllOrgaizationalUnits(QueryFilter queryFilter)
    {
        await Task.CompletedTask;
        return Ok();
    }


    //todo : can move to Authenticate (??) cause relate to current User.
    //todo : ............check usage.............
    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetOrgaizationalUnitsOfUser()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetOrganizationalUnitDto>> GetOrgaizationalUnitById(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles ="Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateOrgaizationalUnit(OrganizationalUnitCreateDto organizationalUnitCreateDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditOrgaizationalUnit(int id, OrganizationalUnitUpdateDto organizationalUnitUpdateDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //there was an Get/Actors endpoint in old version that returned Executives, that can replace by Get/Executives from
    //Info Controller (if they be similar)

}
