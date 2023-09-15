using Api.Contracts;
using Api.Services.Storage;
using Domain.Data;
using Domain.Models.Relational;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IStorageService _storageService;

    public FilesController(ApplicationDbContext context, IStorageService storageService)
    {
        _context = context;
        _storageService = storageService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Media>> GetFile(Guid id)
    {
        var media = await _context.Media.Where(p => p.Id == id).SingleOrDefaultAsync();
        if (media == null)
        {
            return NotFound();
        }
        return Ok(media);
    }

    [HttpPost]
    public async Task<IActionResult> PostFile([FromForm] UploadDto upload)
    {
        var media = await _storageService.WriteFileAsync(upload.File, upload.AttachmentType);
        if (media == null)
        {
            return Problem();
        }
        _context.Media.Add(media);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFile), media.Id, media);
    }
}


