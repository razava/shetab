﻿using Api.Abstractions;
using Api.Contracts;
using Api.ExtensionMethods;
using Application.Categories.Commands.AddCategory;
using Application.Categories.Commands.DeleteCategory;
using Application.Categories.Commands.UpdateCategory;
using Application.Categories.Queries.GetCategory;
using Application.Categories.Queries.GetCategoryById;
using Application.Common.FilterModels;
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


    [Authorize]
    [HttpGet("All")]
    public async Task<ActionResult<CategoryGetDto>> GetAllCategories([FromQuery] QueryFilter queryFilter)
    {
        //todo : review for result codes and clear if possible
        var instanceId = User.GetUserInstanceId();
        var mappedFilter = queryFilter.Adapt<QueryFilterModel>();
        var query = new GetCategoryQuery(instanceId, mappedFilter, true);
        var result = await Sender.Send(query);
        if(result.IsFailed)
        {
            return Problem(result.ToResult());
        }
        var resultValue = result.Value;
        var tempResult = resultValue;
        if (queryFilter.Query != null)
        {
            var filtered = tempResult.Where(r => r.Title.Contains(queryFilter.Query)).ToList();
            var parentIds = filtered.Select(r => r.ParentId).Distinct().ToList();
            filtered.AddRange(tempResult.Where(t => parentIds.Contains(t.Id)).ToList());
            filtered.Add(tempResult.Where(t => t.ParentId == null).Single());
            resultValue = filtered.Distinct().ToList();
        }
        resultValue.ForEach(x => x.Categories = resultValue.Where(c => c.ParentId == x.Id).ToList());
        
        var root = resultValue.Where(r => r.ParentId == null).Single();

        return Ok(root.Adapt<CategoryGetDto>());
    }


    [Authorize]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryGetDetailDto>> GetCategoryById(int id)
    {
        var query = new GetCategoryByIdQuery(id);
        var result = await Sender.Send(query);
        
        return result.Match(
            s => Ok(s.Adapt<CategoryGetDetailDto>()),
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
            categoryCreateDto.ProcessId,
            categoryCreateDto.IsDeleted,
            categoryCreateDto.ObjectionAllowed,
            categoryCreateDto.EditingAllowed,
            categoryCreateDto.HideMap,
            categoryCreateDto.AttachmentDescription,
            categoryCreateDto.FormElements);

        var result = await Sender.Send(command);
        
        return result.Match(
            s => CreatedAtAction(
                nameof(GetCategoryById),
                new { id = s.Id, instanceId = instanceId },
                s.Adapt<CategoryGetDetailDto>()),
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
            categoryUpdateDto.FormElements);

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
