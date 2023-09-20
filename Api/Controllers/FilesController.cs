﻿using Api.Abstractions;
using Api.Contracts;
using Application.Medias.Commands.AddMedia;
using Domain.Models.Relational;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ApiController
{
    
    public FilesController(ISender sender): base(sender)
    {
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Media>> GetFile(Guid id)
    {
        var command = new GetMediaQuery(id);
        var media = await Sender.Send(command);
        return Ok(media);
    }

    [HttpPost]
    public async Task<IActionResult> PostFile([FromForm] UploadDto upload)
    {
        var command = new AddMediaCommand(upload.File, upload.AttachmentType);
        var media = await Sender.Send(command);
        if (media == null)
        {
            return Problem();
        }
        
        return CreatedAtAction(nameof(GetFile), media.Id, media);
    }
}


