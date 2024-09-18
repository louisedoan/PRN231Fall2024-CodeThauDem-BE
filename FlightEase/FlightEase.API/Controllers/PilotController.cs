using BusinessObjects.DTOs;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/pilots")]
    public class PilotController : ControllerBase {

        private IPilotService _pilotService;

         public PilotController(IPilotService pilotService)
        {
            _pilotService = pilotService;
        }

        [MapToApiVersion("1")]
        [HttpPost]
        public ActionResult<PilotDTO> CreatePilot(PilotDTO pilotCreate)
        {
            var pilotCreated = _pilotService.CreatePilot(pilotCreate);

            if (pilotCreated == null)
            {
                return NotFound("");
            }
            return pilotCreated;
        }

        [MapToApiVersion("1")]
        [HttpGet]
        public ActionResult<List<PilotDTO>> GetAll()
        {
            var pilotList = _pilotService.GetAll();

            if (pilotList == null)
            {
                return NotFound("");
            }
            return pilotList;
        }

        [MapToApiVersion("1")]
        [HttpGet("idTmp")]
        public ActionResult<PilotDTO> GetById(int idTmp)
        {
            var pilotDetail = _pilotService.GetById(idTmp);

            if (pilotDetail == null)
            {
                return NotFound("");
            }
            return pilotDetail;
        }

        [MapToApiVersion("1")]
        [HttpDelete]
        public ActionResult<bool> DeletePilot(int idTmp)
        {
            var check = _pilotService.DeletePilot(idTmp);

            if (check == false)
            {
                return NotFound("");
            }
            return check;
        }

        [MapToApiVersion("1")]
        [HttpPut]
        public ActionResult<PilotDTO> UpdatePilot(PilotDTO pilotCreate)
        {
            var pilotUpdated = _pilotService.UpdatePilot(pilotCreate);

            if (pilotUpdated == null)
            {
                return NotFound("");
            }
            return pilotUpdated;
        }
    }

}
