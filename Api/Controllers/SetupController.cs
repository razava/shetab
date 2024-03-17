using Api.Abstractions;
using Api.ExtensionMethods;
using Application.Common.Statics;
using Application.Setup.Commands.AddDummyCategoriesForStaff;
using Application.Setup.Commands.AddDummyDataCommand;
using Application.Setup.Commands.AddGoldenUser;
using Application.Setup.Commands.AddInstance;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SetupController : ApiController
{
    public SetupController(ISender sender) : base(sender)
    {
    }

    public class MultipleFilesUploadModel
    {
        public string DummyString { get; set; } = string.Empty;
        public List<IFormFile> Files { get; set; } = null!;
    }

    //[Authorize(Roles = "PowerUser")]
    [HttpPost]
    public async Task<ActionResult<bool>> AddInstance([FromForm] MultipleFilesUploadModel filesModel)
    {

        var command = new AddInstanceCommand(filesModel.Files);
        var result = await Sender.Send(command);
        
        return Ok(result);
    }

    [HttpGet("AddDummies")]
    public async Task<ActionResult> AddDummies(int count)
    {
        var command = new AddDummyDataCommand(count);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = RoleNames.PowerUser)]
    [HttpPost("AddGoldenUser")]
    public async Task<ActionResult> AddGoldenUser(AddGoldenUserDto addGoldenUserDto)
    {
        var query = new AddGoldenUserCommand(addGoldenUserDto.InstanceId, addGoldenUserDto.Username, addGoldenUserDto.Password);

        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize(Roles = RoleNames.PowerUser)]
    [HttpPost("AddClerkRole")]
    public async Task<ActionResult> AddGoldenUser()
    {
        var query = new AddDummyCategoriesForStaffCommand();

        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }
}

public record AddGoldenUserDto(int InstanceId, string Username, string Password);