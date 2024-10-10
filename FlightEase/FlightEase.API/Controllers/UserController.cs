
using BusinessObjects.DTOs;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightEaseDB.Presentation.Controllers
{

	[ApiController]
	[ApiVersion("1")]
	[Route("/api/v1/users")]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		// API: Create a new user
		// POST: /api/v1/users/create
		[MapToApiVersion("1")]
		[HttpPost("create")]
		public ActionResult<UserDTO> CreateUser([FromBody] CreateUserDTO userCreate)
		{
			if (userCreate == null)
			{
				return BadRequest("Invalid user data");
			}

			var userCreated = _userService.CreateUser(userCreate);

			if (userCreated == null)
			{
				return NotFound("Failed to create user");
			}

			return Ok(new { message = "User created successfully", data = userCreated });
		}

		// API: Get all users
		// GET: /api/v1/users/all
		[MapToApiVersion("1")]
		[HttpGet("all")]
		public ActionResult<List<UserDTO>> GetAll()
		{
			var userList = _userService.GetAll();

			if (userList == null || !userList.Any())
			{
				return NotFound("No users found");
			}

			return Ok(new { message = "User list retrieved successfully", data = userList });
		}

		// API: Get user by ID
		// GET: /api/v1/users/{idTmp}
		[MapToApiVersion("1")]
		[HttpGet("{idTmp}")]
		public ActionResult<UserDTO> GetById(int idTmp)
		{
			var userDetail = _userService.GetById(idTmp);

			if (userDetail == null)
			{
				return NotFound($"User with ID {idTmp} not found");
			}

			return Ok(new { message = "User details retrieved successfully", data = userDetail });
		}

		// API: Delete user by ID
		// DELETE: /api/v1/users/delete/{idTmp}
		[MapToApiVersion("1")]
		[HttpDelete("delete/{idTmp}")]
		public ActionResult DeleteUser(int idTmp)
		{
			var isDeleted = _userService.DeleteUser(idTmp);

			if (!isDeleted)
			{
				return NotFound($"User with ID {idTmp} not found");
			}

			return Ok(new { message = "User deleted successfully" });
		}

		// API: Update user by ID
		// PUT: /api/v1/users/update/{idTmp}
		[MapToApiVersion("1")]
		[HttpPut("update")]
		public ActionResult<UserDTO> UpdateUser(UserDTO userUpdate)
		{
			if (userUpdate == null)
			{
				return BadRequest("Invalid user data or ID mismatch");
			}

			var updatedUser = _userService.UpdateUser(userUpdate);

			if (updatedUser == null)
			{
				return NotFound($"User not found");
			}

			return Ok(new { message = "User updated successfully", data = updatedUser });
		}


		[HttpPost("register")]
		public async Task<IActionResult> AddUser([FromBody] RegisterDTO userDto)
		{
			try
			{
				var result = await _userService.Register(userDto);
				if (!result.IsSuccess)
				{
					return BadRequest("Email already exists.");
				}

				return Ok("User created successfully.");
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message); // Trả về thông báo lỗi định dạng email
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred."); // Trả về lỗi chung
			}
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginDTO userDto)
		{

			var result = await _userService.AuthenticateAsync(userDto);

			if (!result.IsSuccess)
			{
				return StatusCode(result.StatusCode, new { message = result.Message });
			}

			return Ok(result.Data);
		}
	}
}


