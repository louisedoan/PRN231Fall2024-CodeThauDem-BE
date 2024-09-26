using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/seats")]
    public class SeatController : ODataController {

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
        public ActionResult<SeatDTO> UpdateSeat(SeatDTO seatUpdate)
        {
            try
            {
           
                var seatUpdated = _seatService.UpdateSeat(seatUpdate);

                if (seatUpdated == null)
                {
                    return NotFound("Seat not found or update failed.");
                }
            
                return Ok(new { message = "Update successful" , seatUpdate});
            }
            catch (Exception ex)
            {       
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [MapToApiVersion("1")]
        [EnableQuery]
        [HttpGet("query")]
        public ActionResult<List<SeatDTO>> GetSeats(/*ODataQueryOptions<SeatDTO> queryOptions,*/ [FromQuery] string classFilter = null, [FromQuery] int? planeId = null)
        {

            var seats = _seatService.GetAll();

            if (!string.IsNullOrEmpty(classFilter))
            {
                seats = seats.Where(s => s.Class == classFilter).ToList();
            }

            if (planeId.HasValue)
            {
                seats = seats.Where(s => s.PlaneId == planeId.Value).ToList();
            }

            return Ok(seats);
        }
    }

}
