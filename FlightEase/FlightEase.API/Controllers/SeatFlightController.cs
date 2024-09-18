using BusinessObjects.DTOs;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/seatflights")]
    public class SeatFlightController : ControllerBase {

        private ISeatFlightService _seatflightService;

         public SeatFlightController(ISeatFlightService seatflightService)
        {
            _seatflightService = seatflightService;
        }

        [MapToApiVersion("1")]
        [HttpPost]
        public ActionResult<SeatFlightDTO> CreateSeatFlight(SeatFlightDTO seatflightCreate)
        {
            var seatflightCreated = _seatflightService.CreateSeatFlight(seatflightCreate);

            if (seatflightCreated == null)
            {
                return NotFound("");
            }
            return seatflightCreated;
        }

        [MapToApiVersion("1")]
        [HttpGet]
        public ActionResult<List<SeatFlightDTO>> GetAll()
        {
            var seatflightList = _seatflightService.GetAll();

            if (seatflightList == null)
            {
                return NotFound("");
            }
            return seatflightList;
        }

        [MapToApiVersion("1")]
        [HttpGet("idTmp")]
        public ActionResult<SeatFlightDTO> GetById(int idTmp)
        {
            var seatflightDetail = _seatflightService.GetById(idTmp);

            if (seatflightDetail == null)
            {
                return NotFound("");
            }
            return seatflightDetail;
        }

        [MapToApiVersion("1")]
        [HttpDelete]
        public ActionResult<bool> DeleteSeatFlight(int idTmp)
        {
            var check = _seatflightService.DeleteSeatFlight(idTmp);

            if (check == false)
            {
                return NotFound("");
            }
            return check;
        }

        [MapToApiVersion("1")]
        [HttpPut]
        public ActionResult<SeatFlightDTO> UpdateSeatFlight(SeatFlightDTO seatflightCreate)
        {
            var seatflightUpdated = _seatflightService.UpdateSeatFlight(seatflightCreate);

            if (seatflightUpdated == null)
            {
                return NotFound("");
            }
            return seatflightUpdated;
        }
    }

}
