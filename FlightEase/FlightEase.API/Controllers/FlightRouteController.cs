using BusinessObjects.DTOs;
using FlightEaseDB.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/flightroutes")]
    public class FlightRouteController : ControllerBase {

        private IFlightRouteService _flightrouteService;

         public FlightRouteController(IFlightRouteService flightrouteService)
        {
            _flightrouteService = flightrouteService;
        }

        [MapToApiVersion("1")]
        [HttpPost]
        public ActionResult<FlightRouteDTO> CreateFlightRoute(FlightRouteDTO flightrouteCreate)
        {
            var flightrouteCreated = _flightrouteService.CreateFlightRoute(flightrouteCreate);

            if (flightrouteCreated == null)
            {
                return NotFound("");
            }
            return flightrouteCreated;
        }

        [MapToApiVersion("1")]
        [HttpGet]
        public ActionResult<List<FlightRouteDTO>> GetAll()
        {
            var flightrouteList = _flightrouteService.GetAll();

            if (flightrouteList == null)
            {
                return NotFound("");
            }
            return flightrouteList;
        }

        [MapToApiVersion("1")]
        [HttpGet("idTmp")]
        public ActionResult<FlightRouteDTO> GetById(int idTmp)
        {
            var flightrouteDetail = _flightrouteService.GetById(idTmp);

            if (flightrouteDetail == null)
            {
                return NotFound("");
            }
            return flightrouteDetail;
        }

        [MapToApiVersion("1")]
        [HttpDelete]
        public ActionResult<bool> DeleteFlightRoute(int idTmp)
        {
            var check = _flightrouteService.DeleteFlightRoute(idTmp);

            if (check == false)
            {
                return NotFound("");
            }
            return check;
        }

        [MapToApiVersion("1")]
        [HttpPut]
        public ActionResult<FlightRouteDTO> UpdateFlightRoute(FlightRouteDTO flightrouteCreate)
        {
            var flightrouteUpdated = _flightrouteService.UpdateFlightRoute(flightrouteCreate);

            if (flightrouteUpdated == null)
            {
                return NotFound("");
            }
            return flightrouteUpdated;
        }
    }

}
