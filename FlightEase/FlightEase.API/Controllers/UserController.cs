using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Helpers;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/users")]
    public class UserController : ControllerBase {

        private IUserService _userService;
       
        public UserController(IUserService userService)
        {
            _userService = userService; 
        }

        [MapToApiVersion("1")]
        [HttpPost]
        public ActionResult<UserDTO> CreateUser(UserDTO userCreate)
        {
            var userCreated = _userService.CreateUser(userCreate);

            if (userCreated == null)
            {
                return NotFound("");
            }
            return userCreated;
        }

        [MapToApiVersion("1")]
        [HttpGet]
        public ActionResult<List<UserDTO>> GetAll()
        {
            var userList = _userService.GetAll();

            if (userList == null)
            {
                return NotFound("");
            }
            return userList;
        }

        [MapToApiVersion("1")]
        [HttpGet("idTmp")]
        public ActionResult<UserDTO> GetById(int idTmp)
        {
            var userDetail = _userService.GetById(idTmp);

            if (userDetail == null)
            {
                return NotFound("");
            }
            return userDetail;
        }

        [MapToApiVersion("1")]
        [HttpDelete]
        public ActionResult<bool> DeleteUser(int idTmp)
        {
            var check = _userService.DeleteUser(idTmp);

            if (check == false)
            {
                return NotFound("");
            }
            return check;
        }

        [MapToApiVersion("1")]
        [HttpPut]
        public ActionResult<UserDTO> UpdateUser(UserDTO userCreate)
        {
            var userUpdated = _userService.UpdateUser(userCreate);

            if (userUpdated == null)
            {
                return NotFound("");
            }
            return userUpdated;
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
