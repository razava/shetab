using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Categories.Commands.AddCategory;
using Application.Categories.Commands.UpdateCategory;
using Application.Categories.Queries.GetCategory;
using Application.Categories.Queries.GetCategoryById;
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

    //[Authorize]
    //[HttpGet]
    //public async Task<ActionResult<List<FlattenShortCategoryDto>>> GetCategories(int instanceId)
    //{
    //    //todo : need a query that return flat list.............................................
    //    //var query = new GetCategoryQuery(instanceId);
    //    //var result = await Sender.Send(query);
    //    //var mappedResult = result.Adapt<List<FlattenShortCategoryDto>>();
    //    //return Ok(mappedResult);
    //    await Task.CompletedTask;
    //    return Ok();
    //}

    [Authorize]
    [HttpGet("All")]
    public async Task<ActionResult<CategoryGetDto>> GetAllCategories([FromQuery] QueryFilter queryFilter)
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetCategoryQuery(instanceId, true);
        var result = await Sender.Send(query);
        result.ForEach(x => x.Categories = result.Where(c => c.ParentId == x.Id).ToList());
        var root = result.Where(r => r.ParentId == null).Single();

        return Ok(root.Adapt<CategoryGetDto>());
    }


    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryGetDetailDto>> GetCategoryById(int id)
    {
        var query = new GetCategoryByIdQuery(id);
        var result = await Sender.Send(query);
        if (result == null)
            return Problem();
        var mappedResult = result.Adapt<CategoryGetDetailDto>();
        return Ok(mappedResult);
    }



    /*
    [Authorize]
    [HttpGet("Processes")]
    public async Task<ActionResult<List<GetProcessListDto>>> GetProcesses()
    {
        await Task.CompletedTask;
        return Ok();
    }
    */

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> CreateCategory(CategoryCreateDto categoryCreateDto)
    {
        var instanceId = User.GetUserInstanceId();

        var command = new AddCategoryCommand(
            instanceId,
            categoryCreateDto.Code,
            categoryCreateDto.Title,
            categoryCreateDto.Description,
            categoryCreateDto.Order,
            categoryCreateDto.ParentId,
            categoryCreateDto.Duration,
            categoryCreateDto.ResponseDuration,
            categoryCreateDto.ProcessId,
            categoryCreateDto.IsDeleted,
            categoryCreateDto.ObjectionAllowed,
            categoryCreateDto.EditingAllowed,
            categoryCreateDto.HideMap,
            categoryCreateDto.AttachmentDescription,
            categoryCreateDto.FormElements);

        var result = await Sender.Send(command);
        if (result == null)
            return Problem();
        //return Created();
        var routeValues = new { id = result.Id, instanceId = instanceId };
        return CreatedAtAction(nameof(GetCategoryById), routeValues, result.Adapt<CategoryGetDetailDto>());
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}")]
    public async Task<ActionResult> EditCategory(int id, CategoryUpdateDto categoryUpdateDto)
    {
        var command = new UpdateCategoryCommand(
            id,
            categoryUpdateDto.Code,
            categoryUpdateDto.Title,
            categoryUpdateDto.Description,
            categoryUpdateDto.Order,
            categoryUpdateDto.ParentId,
            categoryUpdateDto.Duration,
            categoryUpdateDto.ResponseDuration,
            categoryUpdateDto.ProcessId,
            categoryUpdateDto.IsDeleted,
            categoryUpdateDto.ObjectionAllowed,
            categoryUpdateDto.EditingAllowed,
            categoryUpdateDto.HideMap,
            categoryUpdateDto.AttachmentDescription,
            categoryUpdateDto.FormElements);

        var result = await Sender.Send(command);
        if (result == null)
            return Problem();
        return NoContent();
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult> DeleteCategory(Guid id)
    {
        await Task.CompletedTask;
        return Ok("Not Implemented");
    }


}
