using Api.Abstractions;
using Api.Authentication;
using Application.Common.Interfaces.Persistence;
using Application.Users.Commands.CreateContractor;
using Application.Users.Commands.CreateNewPassword;
using Application.Users.Commands.CreateUser;
using Application.Users.Queries.GetUserById;
using Application.Users.Queries.GetUsers;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

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
        //var command = CreateNewPasswordCommand()
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("Roles/{id}")]
    public async Task<IActionResult> SetRoles(string id)
    {
        //UpdateRolesCommand
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("Roles/{id}")]
    public async Task<IActionResult> GetUserRoles(string id)
    {
        //GetUserRolesQuery
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
        //GetUsersQuery
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize]
    [HttpGet("User/{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        //GetUserByIdQuery
        await Task.CompletedTask;
        return Ok();
    }

    //We won't use this, admin should create the user and after successful creation, assign desired roles
    //[Authorize(Roles = "Admin, Manager")]
    //[HttpPost("RegisterWithRoles")]
    //public async Task<IActionResult> RegisterWithRoles()
    //{
    //    await Task.CompletedTask;
    //    return Ok();
    //}

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ApplicationUser>> CreateUser(CreateUserDto model)
    {
        var command = new CreateUserCommand(model.Username, model.Password, model.FirstName, model.LastName, model.Title);
        var user = await Sender.Send(command);

        return CreatedAtAction(nameof(GetUserById), user.Id, user);
    }

    //This endpoint is accessible by executives only
    [Authorize(Roles = "Executive")]
    [HttpPost("RegisterContractor")]
    public async Task<IActionResult> RegisterContractor(CreateContractorDto model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if(userId is null)
            return Unauthorized();
        var command = new CreateContractorCommand(
            userId,
            model.PhoneNumber,
            model.Organization,
            model.FirstName,
            model.LastName,
            model.Title);
        var contractor = await Sender.Send(command);
        
        //TODO: Does executive have access to this endpoint?
        return CreatedAtAction(nameof(GetUserById), contractor.Id, contractor);
    }

    [Authorize(Roles = "Executive")]
    [HttpPost("GetContractors")]
    public async Task<IActionResult> GetContractors(PagingInfo pagingInfo)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized();
        var command = new GetContractorsQuery(userId, pagingInfo);
        var result = await Sender.Send(command);
        return Ok();
    }

    //todo : how about register Mayor[admin], Executive[admin, manager], Manager(organizationalUnit)
    //These are handled by UpdateRoles

    public record CreateUserDto(
        string Username,
        string Password,
        string FirstName = "",
        string LastName = "",
        string Title = "");
    public record CreateContractorDto(
        string PhoneNumber,
        string Organization = "",
        string FirstName = "",
        string LastName = "",
        string Title = "");
}
