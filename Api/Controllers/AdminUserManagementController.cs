using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.Users.Commands.CreateContractor;
using Application.Users.Commands.CreateNewPassword;
using Application.Users.Commands.CreateUser;
using Application.Users.Commands.UpdateRegions;
using Application.Users.Commands.UpdateRoles;
using Application.Users.Commands.UpdateUserProfile;
using Application.Users.Common;
using Application.Users.Queries.GetRegions;
using Application.Users.Queries.GetRoles;
using Application.Users.Queries.GetUserById;
using Application.Users.Queries.GetUsers;
using Domain.Models.Relational.IdentityAggregate;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;


[Route("api/{instanceId}/[controller]")]
[ApiController]
public class AdminUserManagementController : ApiController
{
    public AdminUserManagementController(ISender sender) : base(sender)
    {
    }

    //todo : define roles for Authorize


    [Authorize(Roles ="Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserDto updateUserDto)
    {
        var command = new UpdateUserProfileCommand(
            id,
            updateUserDto.FirstName,
            updateUserDto.LastName,
            updateUserDto.Title,
            null,
            null,
            null,
            null,
            null,
            null);

        var result = await Sender.Send(command);
        if (result == null)
            return Problem();

        return NoContent();
    }



    [Authorize(Roles = "Admin, Manager")]
    [HttpPut("Password/{id}")]
    public async Task<IActionResult> ChangePasswordById(string id, [FromBody] NewPasswordDto newPasswordDto)
    {
        var command = new CreateNewPasswordCommand(id, newPasswordDto.NewPassword);
        var result = await Sender.Send(command);
        if(!result)
            return StatusCode(StatusCodes.Status500InternalServerError);
        return NoContent();
        //todo : set appropriate responses.
    }


    [Authorize(Roles = "Admin, Manager")]
    [HttpPut("Roles/{id}")]
    public async Task<IActionResult> SetRoles(string id, UpdateRolesDto updateRolesDto) 
    {
        var mappedRoles = updateRolesDto.Roles.Adapt<List<IsInRoleModel>>();
        var commond = new UpdateRolesCommand(id, mappedRoles);
        var result = await Sender.Send(commond);
        if (!result)
            return Problem();
        //todo : handle result & set appropriate responses.
        return NoContent();
    }


    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("Roles/{id}")]
    public async Task<ActionResult<List<RolesDto>>> GetUserRoles(string id)
    {
        var query = new GetUserRolesQuery(id);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List<RolesDto>>();
        return Ok(mappedResult);
    }


    [Authorize]
    [HttpPut("Regions/{id}")]
    public async Task<IActionResult> SetUserRegions(string id, List<IsInRegionDto> regions)
    {
        var instanceId = User.GetUserInstanceId();
        var mappedRegions = regions.Adapt<List<IsInRegionModel>>();
        var command = new UpdateRegionsCommand(instanceId, id, mappedRegions);
        var result = await Sender.Send(command);
        if(!result) return Problem();
        return NoContent();
    }
    

    [Authorize]
    [HttpGet("Regions/{id}")]
    public async Task<ActionResult<List<IsInRegionDto>>> GetUserRegions(string id)
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetUserRegionsQuery(instanceId, id);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List< IsInRegionDto >>();
        return Ok(mappedResult);
    }

    
    //instanceId ??
    [Authorize]
    [HttpGet("AllUsers")]
    public async Task<ActionResult<List<AdminGetUserList>>> GetAllUsers([FromQuery]PagingInfo pagingInfo, [FromQuery] FilterGetUsers filter)
    {
        //have FilterGetUsers
        //todo : query shuold get instanceId for returning this instance's staffs
        var t = filter;
        var instanceId = User.GetUserInstanceId();
        var query = new GetUsersQuery(pagingInfo, instanceId);
        var result = await Sender.Send(query);
        Response.AddPaginationHeaders(result.Meta);
        var mappedResult = result.Adapt<List<AdminGetUserList>>();
        return Ok(mappedResult);
    }


    //.....................not used ??????
    [Authorize]
    [HttpGet("User/{id}")]
    public async Task<ActionResult<AdminGetUserDetailsDto>> GetUserById(string id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await Sender.Send(query);
        if (result == null)
            return NotFound();
        return Ok(result.Adapt<AdminGetUserDetailsDto>());
    }

    //We won't use this, admin should create the user and after successful creation, assign desired roles
    //[Authorize(Roles = "Admin, Manager")]
    //[HttpPost("RegisterWithRoles")]
    //public async Task<IActionResult> RegisterWithRoles()
    //{
    //    await Task.CompletedTask;
    //    return Ok();
    //}


    //todo : review returning response and dto.......
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ApplicationUser>> CreateUser(CreateUserDto model)
    {
        var instanceId = User.GetUserInstanceId();
        var command = new CreateUserCommand(model.Username, model.Password, model.FirstName, model.LastName, model.Title);
        var user = await Sender.Send(command);
        var routeValues = new { id = user.Id, instanceId = instanceId };
        return CreatedAtAction(nameof(GetUserById), routeValues, user.Adapt<AdminGetUserDetailsDto>());
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
        //todo: map returning contractor in below response
        return CreatedAtAction(nameof(GetUserById), contractor.Id, contractor.Adapt<GetContractorsList>());
    }


    [Authorize(Roles = "Executive")]
    [HttpGet("GetContractors")]
    public async Task<ActionResult<List<GetContractorsList>>> GetContractors([FromQuery] PagingInfo pagingInfo)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized();
        var query = new GetContractorsQuery(userId, pagingInfo);
        var result = await Sender.Send(query);
        Response.AddPaginationHeaders(result.Meta);
        var mappedResult = result.Adapt<GetContractorsList>();
        return Ok(mappedResult);
    }

    //todo : how about register Mayor[admin], Executive[admin, manager], Manager(organizationalUnit)
    //These are handled by UpdateRoles

    
}
