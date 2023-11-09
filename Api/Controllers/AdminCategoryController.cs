using Api.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class AdminCategoryController : ApiController
{
    protected AdminCategoryController(ISender sender) : base(sender)
    {
    }

    //todo : Define & Set Dtos
    //TODO: Define access policies


    [Authorize]
    [HttpGet]
    public async Task<ActionResult> GetCategories()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize]
    [HttpGet("All")]
    public async Task<ActionResult> GetAllCategories()
    {
        await Task.CompletedTask;
        return Ok();
    }

    //!
    [Authorize]
    [HttpGet("Processes")]
    public async Task<ActionResult> GetProcesses()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateCategory()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpPut("{id:Guid}")]
    public async Task<ActionResult> EditCategory(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        await Task.CompletedTask;
        return Ok();
    }


}
