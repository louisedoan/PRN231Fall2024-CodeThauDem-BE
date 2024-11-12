using BusinessObjects.DTOs;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/planes")]
    public class PlaneController : ControllerBase
    {
        private readonly IPlaneService _planeService;

        public PlaneController(IPlaneService planeService)
        {
            _planeService = planeService;
        }

        [MapToApiVersion("1")]
        [HttpPost]
        public ActionResult<PlaneDTO> CreatePlane(PlaneDTO planeCreate)
        {
            var planeCreated = _planeService.CreatePlane(planeCreate);

            if (planeCreated == null)
            {
                return BadRequest("Failed to create plane.");
            }
            return CreatedAtAction(nameof(GetById), new { idTmp = planeCreated.PlaneId }, planeCreated);
        }

        [MapToApiVersion("1")]
        [HttpGet]
        public ActionResult<List<PlaneDTO>> GetAll()
        {
            var planeList = _planeService.GetAll();
            if (planeList == null || !planeList.Any())
            {
                return NotFound("No planes found.");
            }
            return Ok(planeList);
        }

        [MapToApiVersion("1")]
        [HttpGet("{idTmp}")]
        public ActionResult<PlaneDTO> GetById(int idTmp)
        {
            var planeDetail = _planeService.GetById(idTmp);
            if (planeDetail == null)
            {
                return NotFound($"Plane with ID {idTmp} not found.");
            }
            return Ok(planeDetail);
        }

        [MapToApiVersion("1")]
        [HttpDelete("{idTmp}")]
        public ActionResult<bool> DeletePlane(int idTmp)
        {
            var check = _planeService.DeletePlane(idTmp);
            if (!check)
            {
                return NotFound($"Plane with ID {idTmp} not found or couldn't be deleted.");
            }
            return Ok("Plane deleted successfully.");
        }

        [MapToApiVersion("1")]
        [HttpPut("{idTmp}")]
        public ActionResult<PlaneDTO> UpdatePlane(int idTmp, PlaneDTO planeUpdate)
        {
            if (idTmp != planeUpdate.PlaneId)
            {
                return BadRequest("Plane ID mismatch.");
            }

            var planeUpdated = _planeService.UpdatePlane(planeUpdate);
            if (planeUpdated == null)
            {
                return NotFound($"Plane with ID {idTmp} not found.");
            }
            return Ok(planeUpdated);
        }

        [MapToApiVersion("1")]
        [HttpGet("search")]
        public ActionResult<List<PlaneDTO>> SearchPlanes([FromQuery] string status = null)
        {
            var planeDTOs = _planeService.GetSuitablePlane();

            if (!string.IsNullOrEmpty(status))
            {
                planeDTOs = planeDTOs.Where(p => p.Status.Contains(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (planeDTOs == null || !planeDTOs.Any())
            {
                return NotFound("No suitable planes found.");
            }
            return Ok(planeDTOs);
        }
    }

}
