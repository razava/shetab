using Api.Abstractions;
using Api.Dtos;
using Domain.Models.Relational;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class CitizenReportController : ApiController
{
    public CitizenReportController(ISender sender) : base(sender)
    {
    }

    [HttpGet]
    public async Task<ActionResult<Report>> GetReports()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<Report>> GetReportById(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet("Nearest")]
    public async Task<ActionResult<Report>> GetNearest()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet("Locations")]
    public async Task<ActionResult<Report>> GetLocations()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet("Locations/{id:Guid}")]
    public async Task<ActionResult<Report>> GetLocations(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet("Mine")]
    public async Task<ActionResult<Report>> GetMyReports()
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpGet("Mine/{id:Guid}")]
    public async Task<ActionResult<Report>> GetMyReports(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<Report>> CreateReport(CreateReportDto model)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("Like/{id:Guid}")]
    public async Task<ActionResult> Like(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }

    [HttpPost("Comment/{id:Guid}")]
    public async Task<ActionResult> Comment(Guid id, string comment)
    {
        await Task.CompletedTask;
        return Ok();
    }
}
