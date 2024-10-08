using BusinessObjects.DTOs;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightEaseDB.Presentation.Controllers
{

	[ApiController]
	[ApiVersion("1")]
	[Route("/api/v1/memberships")]
	public class MembershipController : ControllerBase
	{
		private readonly IMembershipService _membershipService;

		public MembershipController(IMembershipService membershipService)
		{
			_membershipService = membershipService;
		}

		[MapToApiVersion("1")]
		[HttpPost("create")]
		public ActionResult<MemBershipDTO> CreateMembership([FromBody] MemBershipDTO membershipCreate)
		{
			if (membershipCreate == null)
			{
				return BadRequest("Invalid user data");
			}

			var userCreated = _membershipService.CreateMembership(membershipCreate);

			if (userCreated == null)
			{
				return NotFound("Failed to create user");
			}

			return Ok(new { message = "Membership created successfully", data = membershipCreate });
		}

		[MapToApiVersion("1")]
		[HttpGet]
		public ActionResult<List<MemBershipDTO>> GetAll()
		{
			var membershipList = _membershipService.GetAll();

			if (membershipList == null || !membershipList.Any())
			{
				return NotFound();
			}
			return Ok(membershipList);
		}

		[MapToApiVersion("1")]
		[HttpGet("{idTmp}")]
		public ActionResult<MemBershipDTO> GetById(int idTmp)
		{
			var membershipDetail = _membershipService.GetById(idTmp);

			if (membershipDetail == null)
			{
				return NotFound();
			}
			return Ok(membershipDetail);
		}

		[MapToApiVersion("1")]
		[HttpDelete("{idTmp}")]
		public ActionResult<bool> DeleteMembership(int idTmp)
		{
			var check = _membershipService.DeleteMembership(idTmp);

			if (!check)
			{
				return NotFound();
			}
			return Ok(check);
		}

		[MapToApiVersion("1")]
		[HttpPut]
		public ActionResult<MemBershipDTO> UpdateMembership(MemBershipDTO membershipUpdate)
		{
			var membershipUpdated = _membershipService.UpdateMembership(membershipUpdate);

			if (membershipUpdated == null)
			{
				return NotFound();
			}
			return Ok(membershipUpdated);
		}
	}


}
