﻿using Api.Abstractions;
using Api.Authentication;
using Api.Contracts;
using Domain.Models.Relational.IdentityAggregate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;


[Route("api/{instanceId}/[controller]")]
[ApiController]
public class CitizenAccountController : ApiController
{
    protected CitizenAccountController(ISender sender) : base(sender)
    {
    }


    [HttpPost("Login")]
    public async Task<ActionResult> Login(LoginDto loginDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("LoginApp")]
    public async Task<ActionResult> LoginApp(LoginAppDto loginAppDto)
    {
        await Task.CompletedTask;
        return Ok();
    }


    [HttpPost("Register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("RegisterApp")]
    public async Task<ActionResult> RegisterApp(RegisterAppDto registerAppDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("Verify")]
    public async Task<ActionResult> Verify(LoginDto loginDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    [HttpGet]
    public async Task<ActionResult<GetUserProfileDto>> GetUser()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    [HttpPut]
    public async Task<IActionResult> UpdateUser(UpdateUserDto userUpdate)
    {
        await Task.CompletedTask;
        return Ok();
    }
    
    [Authorize(Roles = "Citizen")]
    [HttpPut("Password")]
    public async Task<IActionResult> UpdatePassword(ChangePasswordDto changePassword)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [Authorize(Roles = "Citizen")]
    [HttpPut("PasswordApp")]
    public async Task<ActionResult> UpdatePasswordApp(ChangePasswordAppDto changePasswordAppDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //todo : fix forgot password process
    [HttpPost("ForgotPasswod")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("ForgotPasswodApp")]
    public async Task<IActionResult> ForgotPasswordApp(ForgotPasswordAppDto forgotPasswordDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("RequestToken")]
    public async Task<ActionResult> RequestToken(LoginDto loginDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("ResetPassword")]
    public async Task<ActionResult> ResetPassword(LoginDto loginDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet("Captcha")]
    public string GetCaptcha()
    {
        return "";
    }

    [HttpPost("LoginGov")]
    public async Task<IActionResult> LoginGov(GovLoginDto govLoginDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //Dtos
    //todo : move and compelete dtos in another file & write validation
    //public record UpdateUserDto();
    public record GetUserProfileDto();
    public record ForgotPasswordDto();
    public record ForgotPasswordAppDto();
    public record GovLoginDto();
    
    
}
