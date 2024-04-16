using Api.Abstractions;
using Api.ExtensionMethods;
using Application.Common.Statics;
using Application.Setup.Commands.AddDefaultFormToAllCategories;
using Application.Setup.Commands.AddDummyCategoriesForStaff;
using Application.Setup.Commands.AddDummyDataCommand;
using Application.Setup.Commands.AddGoldenUser;
using Application.Setup.Commands.AddInstance;
using Application.Setup.Commands.FormatReportComments;
using Application.Setup.Commands.SpecifyReplyComments;
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
    public async Task<ActionResult> AddClerkRole(int instanceId)
    {
        var query = new AddDummyCategoriesForStaffCommand(instanceId);

        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = RoleNames.PowerUser)]
    [HttpPost("SpecifyReplyComments")]
    public async Task<ActionResult> SpecifyReplyComments(int instanceId)
    {
        var query = new SpecifyReplyCommand(instanceId);

        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }



    [Authorize(Roles = RoleNames.PowerUser)]
    [HttpPost("AddDefaultFormToAllCategories")]
    public async Task<ActionResult> AddDefaultFormToAllCategories(int instanceId)
    {
        var query = new AddDefaultFormToAllCatCommand(instanceId);

        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize(Roles = RoleNames.PowerUser)]
    [HttpPost("FormatReportComments")]
    public async Task<ActionResult> FormatReportComments(int instanceId)
    {
        //warning : for run on shahrdari server make sure the default form exist in related database!
        //todo : or get the formId form related datebase
        //TODO : later remove added metods in report model for update comments.
        var query = new FormatReportCommentsCommand(instanceId);

        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

}

public record AddGoldenUserDto(int InstanceId, string Username, string Password);