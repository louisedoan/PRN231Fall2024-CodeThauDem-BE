using Microsoft.AspNetCore.Mvc;
using Services.Services;

namespace FlightEaseDB.Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/seats")]
    public class SeatController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [HttpGet("business/{flightId}")]
        public async Task<IActionResult> GetBusinessClassSeats(int flightId)
        {
            var result = await _seatService.GetBusinessClassSeatsAsync(flightId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("economy/{flightId}")]
        public async Task<IActionResult> GetEconomyClassSeats(int flightId)
        {
            var result = await _seatService.GetEconomyClassSeatsAsync(flightId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
