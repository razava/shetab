using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Authentication.Commands.ChangePasswordCommand;
using Application.Authentication.Commands.LoginCommand;
using Application.Users.Commands.UpdateUserAvatar;
using Application.Users.Commands.UpdateUserProfile;
using Application.Users.Queries.GetUserProfile;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticateController : ApiController
{
    public AuthenticateController(ISender sender) : base(sender)
    {
    }


    //TODO: Define access policies



    [HttpPost("LoginStaff")]
    public async Task<ActionResult> LoginStaff(LoginStaffDto loginStaffDto)
    {
        var command = new LoginCommand(loginStaffDto.Username, loginStaffDto.Password);
        var result = await Sender.Send(command);

        if (result.IsFailed)
            return Problem(result.ToResult());
        
        if (result.Value.UserNotConfirmed)
        {
            return StatusCode(StatusCodes.Status428PreconditionRequired, "");
        }
        else
        {
            //TODO: Consider converting token to base64
            return Ok(result.Value.JwtToken);
        }
    }


    [Authorize]
    [HttpPut("ChangePassword")]
    public async Task<ActionResult> ChangePassword(ChangePasswordAppDto changePasswordDto)
    {
        var userName = User.FindFirstValue(ClaimTypes.Name);
        if (userName is null)
        {
            return Unauthorized();
        }
        var command = new ChangePasswordCommand(userName, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("Profile")]
    public async Task<ActionResult<GetStaffProfileDto>> GetUserProfile()
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        var query = new GetUserProfileQuery(userId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s.Adapt<GetStaffProfileDto>()),
            f => Problem(f));
    }


    [Authorize]
    [HttpPut("Profile")]
    public async Task<ActionResult> UpdateProfile(UpdateStaffProfileDto updateDto)
    {
        var userId = User.GetUserId();
        if (userId == null)
            return Unauthorized();

        var command = new UpdateUserProfileCommand(
            userId,
            updateDto.FirstName,
            updateDto.LastName,
            updateDto.Title,
            updateDto.Organization,
            null,
            null,
            updateDto.Education,
            null,
            updateDto.PhoneNumber2);

        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }

    
    [Authorize]
    [HttpPut("Avatar")]
    public async Task<ActionResult> UpdateAvatar([FromForm]UploadDto avatar)
    {
        var userId = User.GetUserId();
        if (userId == null)
        {
            return Unauthorized();
        }
        var command = new UpdateUserAvatarCommand(userId, avatar.File);
        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }
    

}


