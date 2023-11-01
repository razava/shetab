using Api.Abstractions;
using Api.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[Route("api/{instanceId}/[controller]")]
[ApiController]
public class UserManagementController : ApiController
{
    protected UserManagementController(ISender sender) : base(sender)
    {
    }

    //todo : define roles for Authorize

    //todo : set input Dtos

    [Authorize(Roles = "Admin, Manager")]
    [HttpPut("Password/{id}")]
    public async Task<IActionResult> ChangePasswordById(string id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("Roles/{id}")]
    public async Task<IActionResult> SetRoles(string id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("Roles/{id}")]
    public async Task<IActionResult> GetUserRoles(string id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize]
    [HttpPost("Regions/{id}")]
    public async Task<IActionResult> SetUserReegions(string id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize]
    [HttpGet("Regions/{id}")]
    public async Task<IActionResult> GetUserReegions(string id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize]
    [HttpGet("All")]
    public async Task<IActionResult> GetAllUsers()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize]
    [HttpGet("User/{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("RegisterWithRoles")]
    public async Task<IActionResult> RegisterWithRoles()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin, Manager, Executive")]
    [HttpPost("RegisterContractor")]
    public async Task<IActionResult> RegisterContractor()
    {
        await Task.CompletedTask;
        return Ok();
    }

    //todo : how about register Mayor[admin], Executive[admin, manager], Manager(organizationalUnit)


}
