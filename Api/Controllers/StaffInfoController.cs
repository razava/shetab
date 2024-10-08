﻿using Api.Abstractions;
using Api.ExtensionMethods;
using Application.Common.Interfaces.Info;
using Application.Common.Interfaces.Persistence;
using Application.Info.Queries.GetAllReports;
using Application.Info.Queries.GetExcel;
using Application.Info.Queries.GetInfo;
using Application.Info.Queries.GetListChart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StaffInfoController : ApiController
{
    public StaffInfoController(ISender sender) : base(sender)
    {
    }


    [Authorize]
    [HttpGet("ListChart")]
    public async Task<ActionResult> GetListChart()
    {
        var userRoles = User.GetUserRoles();
        var instanceId = User.GetUserInstanceId();
        var query = new GetListChartQuery(instanceId, userRoles);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("Charts/{code}")]
    public async Task<ActionResult> GetChartsById(
        int code,
        string? parameter,
        [FromQuery] List<double>? geometry,
        [FromQuery] List<ReportsToInclude>? reportsToInclude,
        [FromQuery] ReportFilters reportFilters)
    {
        var instanceId = User.GetUserInstanceId();
        var userId = User.GetUserId();
        var userRoles = User.GetUserRoles();
        List<GeoPoint>? geoPoints = null;
        if(geometry is not null)
        {
            if(geometry.Count%2 == 0)
            {
                geoPoints = new List<GeoPoint>();
                for(var i=0; i<geometry.Count; i += 2)
                {
                    geoPoints.Add(new GeoPoint(geometry[i], geometry[i+1]));
                }
            }
        }
        var query = new GetInfoQuery(code, instanceId, userId, userRoles, parameter, geoPoints, reportsToInclude, reportFilters);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }


    [Authorize]
    [HttpGet("Reports")]
    public async Task<ActionResult> GetAllReports(
        [FromQuery] PagingInfo pagingInfo,
        [FromQuery] List<double>? geometry,
        [FromQuery] List<ReportsToInclude>? reportsToInclude,
        [FromQuery] ReportFilters reportFilters)
    {
        var instanceId = User.GetUserInstanceId();
        var userId = User.GetUserId();
        var userRoles = User.GetUserRoles();
        List<GeoPoint>? geoPoints = null;
        if (geometry is not null)
        {
            if (geometry.Count % 2 == 0)
            {
                geoPoints = new List<GeoPoint>();
                for (var i = 0; i < geometry.Count; i += 2)
                {
                    geoPoints.Add(new GeoPoint(geometry[i], geometry[i + 1]));
                }
            }
        }
        var query = new GetAllReportsQuery(pagingInfo, instanceId, userId, userRoles, geoPoints, reportsToInclude, reportFilters);
        var result = await Sender.Send(query);

        return result.Match(
            s => Ok(s),
            f => Problem(f));
    }

    [Authorize]
    [HttpGet("Excel")]
    public async Task<ActionResult> GetExcel(
        [FromQuery] List<double>? geometry,
        [FromQuery] List<ReportsToInclude>? reportsToInclude,
        [FromQuery] ReportFilters reportFilters)
    {
        var instanceId = User.GetUserInstanceId();
        var userId = User.GetUserId();
        var userRoles = User.GetUserRoles();
        List<GeoPoint>? geoPoints = null;
        if (geometry is not null)
        {
            if (geometry.Count % 2 == 0)
            {
                geoPoints = new List<GeoPoint>();
                for (var i = 0; i < geometry.Count; i += 2)
                {
                    geoPoints.Add(new GeoPoint(geometry[i], geometry[i + 1]));
                }
            }
        }
        var query = new GetExcelQuery(instanceId, userId, userRoles, geoPoints, reportsToInclude, reportFilters);
        var result = await Sender.Send(query);

        if (result.IsFailed)
            return Problem(result.ToResult());

        return File(result.Value.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    DateTime.Now.Ticks.ToString() + ".xlsx");
    }

}
