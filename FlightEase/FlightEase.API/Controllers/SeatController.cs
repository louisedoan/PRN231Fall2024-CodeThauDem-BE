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

        [HttpGet("{flightId}")]
        public async Task<IActionResult> GetClassSeats(int flightId)
        {
            var result = await _seatService.GetSeatsAsync(flightId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }
    }
}
