using BusinessObjects.DTOs;
using FlightEaseDB.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/flightroutes")]
    public class FlightRouteController : ControllerBase
    {

        private IFlightRouteService _flightrouteService;

        public FlightRouteController(IFlightRouteService flightrouteService)
        {
            _flightrouteService = flightrouteService;
        }

        [MapToApiVersion("1")]
        [HttpPost("create-location")]
        public async Task<ActionResult<FlightRouteDTO>> CreateFlightLocation(FlightRouteDTO flightRoute)
        {
            var result = await _flightrouteService.CreateLocation(flightRoute);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [MapToApiVersion("1")]
        [HttpPut("update-location")]
        public async Task<ActionResult<ResultModel>> UpdateFlightRoute(FlightRouteDTO location)
        {
            var result = await _flightrouteService.UpdateLocation(location);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [MapToApiVersion("1")]
        [HttpDelete("delete-location")]
        public async Task<ActionResult<ResultModel>> DeleteFlightRoute(int flightRouteId)
        {
            var result = await _flightrouteService.DeleteLocation(flightRouteId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [MapToApiVersion("1")]
        [HttpGet("get-all")]
        public async Task<ActionResult<ResultModel>> GetAll()
        {
            var result = await _flightrouteService.GetAll();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [MapToApiVersion("1")]
        [HttpGet("id")]
        public async Task<ActionResult<ResultModel>> GetById(int idTmp)
        {
            var result = await _flightrouteService.GetById(idTmp);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
