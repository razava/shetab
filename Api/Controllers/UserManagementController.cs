using Api.Abstractions;
using Api.Authentication;
using Api.Contracts;
using Application.Common.Interfaces.Persistence;
using Application.Users.Commands.CreateContractor;
using Application.Users.Commands.CreateNewPassword;
using Application.Users.Commands.CreateUser;
using Application.Users.Commands.UpdateRoles;
using Application.Users.Queries.GetRoles;
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
    public async Task<IActionResult> ChangePasswordById(string id, NewPasswordDto newPasswordDto)
    {
        var command = new CreateNewPasswordCommand(id, newPasswordDto.NewPassword);
        var result = await Sender.Send(command);
        if(!result)
            return StatusCode(StatusCodes.Status500InternalServerError);
        return Ok();
        //todo : set appropriate responses.
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("Roles/{id}")]
    public async Task<IActionResult> SetRoles(string id, UpdateRolesDto updateRolesDto) 
    {
        var commond = new UpdateRolesCommand(id, updateRolesDto.Roles);
        var result = await Sender.Send(commond);
        //todo : handle result & set appropriate responses.
        return Ok();
    }

    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("Roles/{id}")]
    public async Task<IActionResult> GetUserRoles(string id)
    {
        var query = new GetUserRolesQuery(id);
        var result = await Sender.Send(query);
        //todo :  output Dto review
        return Ok(result);
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
    public async Task<IActionResult> GetAllUsers(PagingInfo pagingInfo)
    {
        var query = new GetUsersQuery(pagingInfo);
        var result = await Sender.Send(query);
        //todo :  output Dto review
        return Ok(result);
    }

    [Authorize]
    [HttpGet("User/{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await Sender.Send(query);
        //todo :  output Dto review
        return Ok(result);
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
    [HttpGet("GetContractors")]
    public async Task<IActionResult> GetContractors(PagingInfo pagingInfo)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized();
        var query = new GetContractorsQuery(userId, pagingInfo);
        var result = await Sender.Send(query);
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
