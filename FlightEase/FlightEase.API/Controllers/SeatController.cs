using BusinessObjects.DTOs;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/seats")]
    public class SeatController : ControllerBase {

        private ISeatService _seatService;

         public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        [MapToApiVersion("1")]
        [HttpPost]
        public ActionResult<SeatDTO> CreateSeat(SeatDTO seatCreate)
        {
            var seatCreated = _seatService.CreateSeat(seatCreate);

            if (seatCreated == null)
            {
                return NotFound("");
            }
            return seatCreated;
        }

        [MapToApiVersion("1")]
        [HttpGet]
        public ActionResult<List<SeatDTO>> GetAll()
        {
            var seatList = _seatService.GetAll();

            if (seatList == null)
            {
                return NotFound("");
            }
            return seatList;
        }

        [MapToApiVersion("1")]
        [HttpGet("idTmp")]
        public ActionResult<SeatDTO> GetById(int idTmp)
        {
            var seatDetail = _seatService.GetById(idTmp);

            if (seatDetail == null)
            {
                return NotFound("");
            }
            return seatDetail;
        }

        [MapToApiVersion("1")]
        [HttpDelete]
        public ActionResult<bool> DeleteSeat(int idTmp)
        {
            var check = _seatService.DeleteSeat(idTmp);

            if (check == false)
            {
                return NotFound("");
            }
            return check;
        }

        [MapToApiVersion("1")]
        [HttpPut]
        public ActionResult<SeatDTO> UpdateSeat(SeatDTO seatCreate)
        {
            var seatUpdated = _seatService.UpdateSeat(seatCreate);

            if (seatUpdated == null)
            {
                return NotFound("");
            }
            return seatUpdated;
        }
    }

}
