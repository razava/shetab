using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Categories.Commands.AddCategory;
using Application.Categories.Commands.DeleteCategory;
using Application.Categories.Commands.UpdateCategory;
using Application.Categories.Queries.GetCategory;
using Application.Categories.Queries.GetCategoryById;
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


    [Authorize]
    [HttpGet("All")]
    public async Task<ActionResult> GetAllCategories()
    {
        var instanceId = User.GetUserInstanceId();
        var query = new GetCategoryQuery(instanceId, null, true);
        var result = await Sender.Send(query);
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetCategoryById(int id)
    {
        var query = new GetCategoryByIdQuery(id);
        var result = await Sender.Send(query);
        
        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }



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
            categoryCreateDto.OperatorIds,
            categoryCreateDto.ProcessId,
            categoryCreateDto.IsDeleted,
            categoryCreateDto.ObjectionAllowed,
            categoryCreateDto.EditingAllowed,
            categoryCreateDto.HideMap,
            categoryCreateDto.AttachmentDescription,
            categoryCreateDto.FormId,
            categoryCreateDto.DefaultPriority);

        var result = await Sender.Send(command);
        
        return result.Match(
            s => CreatedAtAction(
                nameof(GetCategoryById),
                new { id = s.Value.Id, instanceId = instanceId },
                s),
            f => Problem(f));
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
            categoryUpdateDto.FormId,
            categoryUpdateDto.DefaultPriority);

        var result = await Sender.Send(command);
        
        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }


    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        var command = new DeleteCategoryCommand(id, true);
        var result = await Sender.Send(command);

        return result.Match(
            s => NoContent(),
            f => Problem(f));
    }


}
