using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Api.Abstractions;
using Api.Dtos.ParsiMap;
using Application.Maps.Queries.MapForwardQuery;
using Mapster;
using Application.Maps.Queries.MapBackwardQuery;
using MediatR;

namespace Shahrbin.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ApiController
    {
        public MapController(ISender sender) : base(sender)
        {
        }

        [HttpGet("Forward/{address}")]
        public async Task<ActionResult<ForwardResult>> Forward(string address)
        {
            var query = new MapForwardQuery(address);
            var result = await Sender.Send(query);
            var r2 = result.Adapt<ForwardResult>();
            return Ok(r2);
        }

        [HttpGet("Backward/{lng}/{lat}")]
        public async Task<ActionResult<BackwardResult>> Backward(double lng, double lat)
        {
            var query = new MapBackwardQuery(lng, lat);
            var result = await Sender.Send(query);
            var r2 = result.Adapt<BackwardResult>();
            return Ok(r2);
        }
    }
}
