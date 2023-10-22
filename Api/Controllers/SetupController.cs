using Api.Abstractions;
using Api.Dtos;
using Application.Setup.Commands;
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
    public async Task<ActionResult<CreateReportDto>> AddInstance([FromForm] MultipleFilesUploadModel filesModel)
    {

        var command = new AddInstanceCommand(filesModel.Files);
        var result = await Sender.Send(command);
        
        return Ok(result);
    }


 }
