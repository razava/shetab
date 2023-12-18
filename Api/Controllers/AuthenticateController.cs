using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
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

[Route("api/{instanceId}/[controller]")]
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
        if (result.UserNotConfirmed)
        {
            return StatusCode(StatusCodes.Status428PreconditionRequired, "");
        }
        else
        {
            //TODO: Consider converting token to base64
            return Ok(result.JwtToken);
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
        if (result)
        {
            return NoContent();
        }
        else
        {
            return Problem();
        }
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
        var mappedUser = result.Adapt<GetStaffProfileDto>();
        return Ok(mappedUser);
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
        if (result == null)
            return Problem();

        return NoContent();
    }

    //todo : Define Access Policy
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
        //todo : no need to handle result??
        return NoContent();
    }
    

    //Probably remove and instead use of Get/Regions/{id} from UserManagementController
    /*
    [Authorize]
    [HttpGet("Regions")] //{instanceId}
    public async Task<ActionResult<GetUserRegionsDto>> GetRegions()
    {
        await Task.CompletedTask;   //........................................  
        return Ok();
    }
    */

}


