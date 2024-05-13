using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Uploads.Commands.CreateUpload;
using Application.Uploads.Queries.CheckUploads;
using Application.Uploads.Queries.GetMediaById;
using Application.Uploads.Queries.GetMedias;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ApiController
{
    
    public FilesController(ISender sender): base(sender)
    {
    }

    //[HttpGet("{id}")]
    //public async Task<ActionResult<MediaDto>> GetFile(Guid id)
    //{
    //    //var command = new GetMediaQuery(id);
    //    //var media = await Sender.Send(command);
    //    //var mappedMedia = media.Adapt<MediaDto>();
    //    //return Ok(mappedMedia);
    //    await Task.CompletedTask;
    //    throw new NotImplementedException();
    //}

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> PostFile([FromForm] UploadDto upload)
    {
        var userId = User.GetUserId();
        if (userId is null)
            return Unauthorized();
        var command = new AddUploadCommand(userId, upload.File, upload.AttachmentType);
        var result = await Sender.Send(command);

        return result.Match(
            s => StatusCode(StatusCodes.Status201Created, Result.Ok(new { id = s.Value.Id })),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("Check")]
    public async Task<ActionResult> CheckUploads([FromQuery]CheckUploadsDto dto)
    {
        var query = new CheckUploadsQuery(dto.AttachmentIds, dto.UserId);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    /*
    [Authorize]
    [HttpPut]
    public async Task<ActionResult> UpdateList(UpdateListDto dto)
    {
        var command = new UpdateUploadsListCommand(
            dto.OldList, dto.NewList, dto.UserId);
        var result = await Sender.Send(command);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }
    */ 


    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetMediaById(Guid id)
    {
        var query = new GetMediaByIdQuery(id);
        var result = await Sender.Send(query);

        return result.Match(
           s => Ok(s),
           f => Problem(f));
    }


    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetMedias([FromQuery]GetMediasDto dto)
    {
        var query = new GetMediasQuery(dto.AttachmentIds);
        var result = await Sender.Send(query);

        return result.Match(
           s => Ok(s),
           f => Problem(f));
    }


}


