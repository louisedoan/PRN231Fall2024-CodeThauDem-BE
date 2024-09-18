using BusinessObjects.DTOs;
using FlightEase.Services.Services;
using Microsoft.AspNetCore.Mvc;
namespace FlightEase.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/flights")]
    public class FlightController : ControllerBase {

        private IFlightService _flightService;

         public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        [MapToApiVersion("1")]
        [HttpPost]
        public ActionResult<FlightDTO> CreateFlight(FlightDTO flightCreate)
        {
            var flightCreated = _flightService.CreateFlight(flightCreate);

            if (flightCreated == null)
            {
                return NotFound("");
            }
            return flightCreated;
        }

        [MapToApiVersion("1")]
        [HttpGet]
        public ActionResult<List<FlightDTO>> GetAll()
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
    }

}
