using Api.Abstractions;
using Api.Contracts;
using Application.Common.Interfaces.Persistence;
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

    //todo : .................Commands & queries ............
    //todo : Define & Set Dtos
    //TODO: Define access policies

    //...........review dto...........
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<FlattenCategoryDto>>> GetCategories()
    {
        await Task.CompletedTask;
        return Ok();
    }

    //............review dto...............
    [Authorize]
    [HttpGet("All")]
    public async Task<ActionResult<List<FlattenCategoryDto>>> GetAllCategories([FromQuery] QueryFilter queryFilter)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //.......................................
    [Authorize]
    [HttpGet("Processes")]
    public async Task<ActionResult> GetProcesses()
    {
        await Task.CompletedTask;
        return Ok();
    }

    //.......... review dto.................
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateCategory(CategoryCreateDto categoryCreateDto)
    {
        await Task.CompletedTask;
        return Ok();
    }

    //.......... review dto.................
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:Guid}")]
    public async Task<ActionResult> EditCategory(Guid id, CategoryCreateDto categoryCreateDto)
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
