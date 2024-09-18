using BusinessObjects.DTOs;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/memberships")]
    public class MembershipController : ControllerBase {

        private IMembershipService _membershipService;

         public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        [MapToApiVersion("1")]
        [HttpPost]
        public ActionResult<MemBershipDTO> CreateMembership(MemBershipDTO membershipCreate)
        {
            var membershipCreated = _membershipService.CreateMembership(membershipCreate);

            if (membershipCreated == null)
            {
                return NotFound("");
            }
            return membershipCreated;
        }

        [MapToApiVersion("1")]
        [HttpGet]
        public ActionResult<List<MemBershipDTO>> GetAll()
        {
            var membershipList = _membershipService.GetAll();

            if (membershipList == null)
            {
                return NotFound("");
            }
            return membershipList;
        }

        [MapToApiVersion("1")]
        [HttpGet("idTmp")]
        public ActionResult<MemBershipDTO> GetById(int idTmp)
        {
            var membershipDetail = _membershipService.GetById(idTmp);

            if (membershipDetail == null)
            {
                return NotFound("");
            }
            return membershipDetail;
        }

        [MapToApiVersion("1")]
        [HttpDelete]
        public ActionResult<bool> DeleteMembership(int idTmp)
        {
            var check = _membershipService.DeleteMembership(idTmp);

            if (check == false)
            {
                return NotFound("");
            }
            return check;
        }

        [MapToApiVersion("1")]
        [HttpPut]
        public ActionResult<MemBershipDTO> UpdateMembership(MemBershipDTO membershipCreate)
        {
            var membershipUpdated = _membershipService.UpdateMembership(membershipCreate);

            if (membershipUpdated == null)
            {
                return NotFound("");
            }
            return membershipUpdated;
        }
    }

}
