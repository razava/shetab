using Api.Abstractions;
using Api.Contracts;
using Application.Categories.Commands.AddCategory;
using Application.Categories.Queries.GetCategory;
using Application.Common.Interfaces.Persistence;
using Domain.Models.Relational;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/{instanceId}/[controller]")]
[ApiController]
public class AdminCategoryController : ApiController
{
    public AdminCategoryController(ISender sender) : base(sender)
    {
    }

    //todo : .................Commands & queries ............
    //TODO: Define access policies

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<FlattenShortCategoryDto>>> GetCategories(int instanceId)
    {
        var query = new GetCategoryQuery(instanceId);
        var result = await Sender.Send(query);
        var mappedResult = result.Adapt<List<FlattenShortCategoryDto>>();
        return Ok(mappedResult);
    }

    [Authorize]
    [HttpGet("All")]
    public async Task<ActionResult<List<FlattenCategoryDto>>> GetAllCategories([FromQuery] QueryFilter queryFilter)
    {
        await Task.CompletedTask;
        //Does GetCategoryQuery include categories list for this endpoint???
        return Ok();
    }

    
    [Authorize]
    [HttpGet("Processes")]
    public async Task<ActionResult<List<GetProcessListDto>>> GetProcesses()
    {
        await Task.CompletedTask;
        return Ok();
    }


    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateCategory(CategoryCreateDto categoryCreateDto)
    {
        await Task.CompletedTask;
        //AddCategoryCommand  need to complete......
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:Guid}")]
    public async Task<ActionResult> EditCategory(Guid id, CategoryCreateDto categoryCreateDto)
    {
        var category = categoryCreateDto.Adapt<Category>();
        //todo : AddCategoryCommand  need to complete......
        var command = new AddCategoryCommand(category);
        var result = await Sender.Send(command);
        if (result == null)
            return Problem();
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
