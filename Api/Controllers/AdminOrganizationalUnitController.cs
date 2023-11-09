using Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class AdminOrganizationalUnitController : ApiController
{
    protected AdminOrganizationalUnitController(ISender sender) : base(sender)
    {
    }

    //todo : Define & Set Dtos
    //TODO: Define access policy


    //todo : can move to UserManagement
    [Authorize]
    [HttpGet("All")]
    public async Task<ActionResult> GetAllOrgaizationalUnits()
    {
        await Task.CompletedTask;
        return Ok();
    }


    //todo : can move to Authenticate (??) cause relate to current User.
    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetOrgaizationalUnitsOfUser()
    {
        await Task.CompletedTask;
        return Ok();
    }

    //todo: can move to UserManagement
    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetOrgaizationalUnitById(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //todo : can move to UserManagement
    [Authorize(Roles ="Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateOrgaizationalUnit()
    {
        await Task.CompletedTask;
        return Ok();
    }

    //todo : can move to UserManagement
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditOrgaizationalUnit(int id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //there was an Get/Actors endpoint in old version that returned Executives, that can replace by Get/Executives from
    //Info Controller (if they be similar)

}
