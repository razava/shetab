﻿using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Persistence;
using Application.Common.Statics;
using Application.Reports.Queries.GetUserReports;
using Application.Users.Commands.CreateContractor;
using Application.Users.Commands.CreateNewPassword;
using Application.Users.Commands.CreateUser;
using Application.Users.Commands.UpdateCategories;
using Application.Users.Commands.UpdateRegions;
using Application.Users.Commands.UpdateRoles;
using Application.Users.Commands.UpdateUserProfile;
using Application.Users.Common;
using Application.Users.Queries.GetContractors;
using Application.Users.Queries.GetUserById;
using Application.Users.Queries.GetUserCategories;
using Application.Users.Queries.GetUserRegions;
using Application.Users.Queries.GetUserRoles;
using Application.Users.Queries.GetUsers;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AdminUserManagementController : ApiController
{
    public AdminUserManagementController(ISender sender) : base(sender)
    {
    }

    //todo : define roles for Authorize


    [Authorize(Roles = $"{RoleNames.Admin}, {RoleNames.ComplaintAdmin}")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, UpdateUserDto updateUserDto)
    {
        var command = new UpdateUserProfileCommand(
            id,
            updateUserDto.FirstName,
            updateUserDto.LastName,
            updateUserDto.Title,
            updateUserDto.Organization,
            updateUserDto.NationalId,
            updateUserDto.TwoFactorEnabled,
            updateUserDto.Gender,
            updateUserDto.Education,
            updateUserDto.BirthDate,
            updateUserDto.PhoneNumber2,
            updateUserDto.SmsAlert);

        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }



    [Authorize(Roles = RoleNames.Admin)]
    [HttpPut("Password/{id}")]
    public async Task<IActionResult> ChangePasswordById(string id, [FromBody] NewPasswordDto newPasswordDto)
    {
        var command = new CreateNewPasswordCommand(id, newPasswordDto.NewPassword);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = RoleNames.Admin)]
    [HttpPut("Roles/{id}")]
    public async Task<IActionResult> SetRoles(string id, UpdateRolesDto updateRolesDto)
    {
        var mappedRoles = updateRolesDto.Roles.Adapt<List<IsInRoleModel>>();
        var commond = new UpdateRolesCommand(id, mappedRoles);
        var result = await Sender.Send(commond);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet("Roles/{id}")]
    public async Task<ActionResult> GetUserRoles(string id)
    {
        var query = new GetUserRolesQuery(id);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet("Roles")]
    public async Task<ActionResult> GetRolesForCreate()
    {
        var query = new GetRolesQuery();
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = RoleNames.Admin)]
    [HttpPut("Regions/{id}")]
    public async Task<ActionResult> SetUserRegions(string id, List<IsInRegionDto> regions)
    {
        var instanceId = User.GetUserInstanceId();
        var mappedRegions = regions.Adapt<List<IsInRegionModel>>();
        var command = new UpdateRegionsCommand(instanceId, id, mappedRegions);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet("Regions/{id}")]
    public async Task<ActionResult> GetUserRegions(string id)
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetUserRegionsQuery(instanceId, id);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = RoleNames.Admin)]
    [HttpPut("Categories/{id}")]
    public async Task<ActionResult> UpdateUserCategories(string id, UpdateUserCategories updateModel)
    {
        var command = new UpdateCategoriesCommand(id, updateModel.CategoryIds);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet("Categories/{id}")]
    public async Task<ActionResult> GetUserCategories(string id)
    {
        var query = new GetUserCategoriesQuery(id);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("AllUsers")]
    public async Task<ActionResult> GetAllUsers(
        [FromQuery] PagingInfo pagingInfo,
        [FromQuery] UserFilters filter)
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetUsersQuery(pagingInfo, instanceId, filter);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = RoleNames.Admin)]
    [HttpGet("User/{id}")]
    public async Task<ActionResult> GetUserById(string id)
    {
        var query = new GetUserByIdQuery(id);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = RoleNames.Admin)]
    [HttpPost]
    public async Task<ActionResult> CreateUser(CreateUserDto model)
    {
        var instanceId = User.GetUserInstanceId();
        var command = new CreateUserCommand(
            instanceId,
            model.Username,
            model.Password,
            model.Roles,
            model.RegionIds,
            model.FirstName,
            model.LastName,
            model.Title);
        var user = await Sender.Send(command);

        return user.Match(
            s => CreatedAtAction(nameof(GetUserById), new { id = s.Value.Id }, s),
            f => Problem(f));
    }


    [Authorize(Roles = RoleNames.Executive)]
    [HttpPost("RegisterContractor")]
    public async Task<ActionResult> RegisterContractor(CreateContractorDto model)
    {
        var instanceId = User.GetUserInstanceId();
        var userId = User.GetUserId();
        var userRoles = User.GetUserRoles();
        if (userId is null)
            return Unauthorized();
        var command = new CreateContractorCommand(
            userId,
            userRoles,
            model.PhoneNumber,
            model.FirstName,
            model.LastName,
            model.Title,
            model.Organization);
        var contractor = await Sender.Send(command);

        return contractor.Match(
            s => CreatedAtAction(nameof(GetUserById), new { id = s.Value.Id }, s),
            f => Problem(f));
        //TODO: Does executive have access to this endpoint?
    }


    [Authorize(Roles = RoleNames.Executive)]
    [HttpGet("GetContractors")]
    public async Task<ActionResult> GetContractors([FromQuery] PagingInfo pagingInfo)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized();
        var query = new GetContractorsQuery(userId, pagingInfo);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s), 
            f => Problem(f));
    }

    [Authorize(Roles = RoleNames.ComplaintAdmin)]
    [HttpPost("ComplaintInspectorUser")]
    public async Task<ActionResult> CreateComplaintInspectorUser(CreateComplaintInspectorUserDto model)
    {
        var command = new CreateUserCommand(
            model.InstanceId,
            model.Username,
            model.Password,
            model.Roles,
            null,
            model.Firstname,
            model.Lastname,
            model.Title);
        var user = await Sender.Send(command);

        return user.Match(
            s => CreatedAtAction(nameof(GetUserById), new { id = s.Value.Id }, s),
            f => Problem(f));
    }
}
