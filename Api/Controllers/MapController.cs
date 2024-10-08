﻿using Microsoft.AspNetCore.Mvc;
using Api.Abstractions;
using Application.Maps.Queries.MapForwardQuery;
using MediatR;
using Api.ExtensionMethods;
using Microsoft.AspNetCore.Authorization;
using Application.Maps.Queries.MapBackward;

namespace Shahrbin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ApiController
    {
        public MapController(ISender sender) : base(sender)
        {
        }

        [Authorize]
        [HttpGet("Forward/{address}")]
        public async Task<ActionResult> Forward(string address)
        {
            var query = new MapForwardQuery(address);
            var result = await Sender.Send(query);

            return result.Match(
                s => Ok(s),
                f => Problem(f));
            
        }


        [Authorize]
        [HttpGet("Backward/{instanceId}/{lng}/{lat}")]
        public async Task<ActionResult> Backward(int instanceId, double lng, double lat)
        {
            var query = new MapBackwardQuery(instanceId, lng, lat);
            var result = await Sender.Send(query);
            return result.Match(
                s => Ok(s),
                f => Problem(f));
        }
    }
}
