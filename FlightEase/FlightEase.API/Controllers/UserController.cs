using BusinessObjects.DTOs;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

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
    }

}
