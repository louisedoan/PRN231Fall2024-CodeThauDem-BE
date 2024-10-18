using BusinessObjects.DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
namespace FlightEase.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/flights")]
    public class FlightController : ControllerBase
    {

        private IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [MapToApiVersion("1")]
        [HttpPost]
        public ActionResult<FlightDTO> CreateFlight(FlightDTO flightCreate)
        {
            if (flightCreate == null)
            {
                return BadRequest("Invalid flight data");
            }
            var flightCreated = _flightService.CreateFlight(flightCreate);


            return flightCreated;
        }

        [MapToApiVersion("1")]
        [HttpGet]
        public ActionResult<List<FlightDTO>> GetAllReport()
        {
            var flightList = _flightService.GetAll();

            if (flightList == null)
            {
                return NotFound("");
            }
            return flightList;
        }

        [MapToApiVersion("1")]
        [HttpGet("idTmp")]
        public ActionResult<FlightDTO> GetById(int idTmp)
        {
            var flightDetail = _flightService.GetById(idTmp);

            if (flightDetail == null)
            {
                return NotFound("");
            }
            return flightDetail;
        }

        [MapToApiVersion("1")]
        [HttpDelete]
        public ActionResult<bool> DeleteFlight(int idTmp)
        {

            var check = _flightService.DeleteFlight(idTmp);

            if (check == false)
            {
                return NotFound("");
            }
            return check;
        }

        [MapToApiVersion("1")]
        [HttpPut]
        public ActionResult<FlightDTO> UpdateFlight(FlightDTO flightCreate)
        {
            var flightUpdated = _flightService.UpdateFlight(flightCreate);

            if (flightUpdated == null)
            {
                return NotFound("");
            }
            return flightUpdated;
        }
        [MapToApiVersion("1")]
        [EnableQuery]
        [HttpGet("query")]
        public ActionResult<List<SeatDTO>> GetFlight(ODataQueryOptions<SeatDTO> queryOptions, [FromQuery] string departureTime = null)
        {

            var seats = _flightService.SearchFlight();

            if (!string.IsNullOrEmpty(departureTime))
            {
                seats = seats.Where(s => s.DepartureTime.HasValue &&
                            s.DepartureTime.Value.ToString("yyyy-MM-dd").Contains(departureTime)).ToList();
            }



            return Ok(seats);
        }

        [MapToApiVersion("1")]
        [HttpGet("search-one-way")]
        public ActionResult<List<FlightDTO>> SearchOneWayFlight([FromQuery] int departureLocation, [FromQuery] int arrivalLocation, [FromQuery] DateTime departureDate)
        {
            var flights = _flightService.SearchOneWayFlight(departureLocation, arrivalLocation, departureDate);
            if (flights == null || flights.Count == 0)
            {
                return NotFound("No flights found for the given criteria.");
            }
            return Ok(flights);
        }

        [MapToApiVersion("1")]
        [HttpGet("get-all-flightReport")]
        public async Task<ActionResult<ResultModel>> GetAll()
        {
            var result = await _flightService.GetAllFlightReports();

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        [MapToApiVersion("1")]
        [HttpPost("get-flight-report-by-order/{orderId}")]
        public async Task<IActionResult> GetFlightReportByOrderID(int orderId)
        {
            var result = await _flightService.GetFlightReportByOrderID(orderId);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return StatusCode(result.StatusCode, result);
        }
    }
}
