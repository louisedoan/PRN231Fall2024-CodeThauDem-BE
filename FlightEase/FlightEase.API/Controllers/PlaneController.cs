using BusinessObjects.DTOs;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/planes")]
    public class PlaneController : ControllerBase {

        private IPlaneService _planeService;

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
                return NotFound("");
            }
            return planeCreated;
        }

        [MapToApiVersion("1")]
        [HttpGet]
        public ActionResult<List<PlaneDTO>> GetAll()
        {
            var planeList = _planeService.GetAll();

            if (planeList == null)
            {
                return NotFound("");
            }
            return planeList;
        }

        [MapToApiVersion("1")]
        [HttpGet("idTmp")]
        public ActionResult<PlaneDTO> GetById(int idTmp)
        {
            var planeDetail = _planeService.GetById(idTmp);

            if (planeDetail == null)
            {
                return NotFound("");
            }
            return planeDetail;
        }

        [MapToApiVersion("1")]
        [HttpDelete]
        public ActionResult<bool> DeletePlane(int idTmp)
        {
            var check = _planeService.DeletePlane(idTmp);

            if (check == false)
            {
                return NotFound("");
            }
            return check;
        }

        [MapToApiVersion("1")]
        [HttpPut]
        public ActionResult<PlaneDTO> UpdatePlane(PlaneDTO planeCreate)
        {
            var planeUpdated = _planeService.UpdatePlane(planeCreate);

            if (planeUpdated == null)
            {
                return NotFound("");
            }
            return planeUpdated;
        }

        [MapToApiVersion("1")]

        [HttpGet("sreachPlane")]
        public ActionResult<List<PlaneDTO>> SreachPlane(ODataQueryOptions<PlaneDTO> queryOptions, [FromQuery] string status = null)
        {
            var planeDTOs = _planeService.GetSuitablePlane();

            if (!string.IsNullOrEmpty(status))
            {
                planeDTOs = planeDTOs.Where(p => p.Status.Contains(status)).ToList();
            }
            return planeDTOs;
        }
    }

}
