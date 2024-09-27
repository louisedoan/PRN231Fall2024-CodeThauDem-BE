using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Repositories.Repositories;
using Services.Helpers;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IUserService {
        public UserDTO CreateUser(UserDTO userCreate);
        public UserDTO UpdateUser(UserDTO userUpdate);
        public bool DeleteUser(int idTmp);
        public List<UserDTO> GetAll();
        public UserDTO GetById(int idTmp);

        public Task<ResultModel> Register(RegisterDTO userRegister);
        public Task<ResultModel> AuthenticateAsync(LoginDTO userLogin);
    }

    public class UserService : IUserService
    {

      private readonly IUserRepository _userRepository;
      private readonly JwtTokenHelper _jwtTokenHelper;
        public UserService(IUserRepository userRepository,JwtTokenHelper jwtTokenHelper )
        {
            _userRepository = userRepository;
            _jwtTokenHelper = jwtTokenHelper;
        }

        public UserDTO CreateUser(UserDTO userCreate)
        {
            throw new NotImplementedException();
        }

        #region Register
        public async Task<ResultModel> Register(RegisterDTO userRegister)
        {
            var result = new ResultModel();

            try
            {
                // Check if user already exists
                var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == userRegister.Email); ;
                if (existingUser != null)
                {
                    result.IsSuccess = false;
                    result.StatusCode = 409;
                    result.Message = "User already exists.";
                    return result;
                }

                // Register the new user
                var newUser = new User
                {
                    Email = userRegister.Email,
                    Password = userRegister.Password,
                    Role = UserRole.Member.ToString(),

                };

                await _userRepository.CreateAsync(newUser);
                await _userRepository.SaveAsync();

                result.IsSuccess = true;
                result.StatusCode = 201;
                result.Message = "User registered successfully.";
                result.Data = new UserDTO
                {
                    UserId = newUser.UserId,
                    Email = newUser.Email
                };
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.StatusCode = 500;
                result.Message = ex.Message;
            }

            return result;
        }
        #endregion

        #region Login
        public async Task<ResultModel> AuthenticateAsync(LoginDTO userLogin)
        {
            var result = new ResultModel();

            try
            {
                var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == userLogin.Email);
               
                if (user != null && user.Password == userLogin.Password)
                {
                    // Generate JWT token using the extracted User object
                    var token = _jwtTokenHelper.GenerateJwtToken(user);

                    result.IsSuccess = true;
                    result.StatusCode = 200;
                    result.Message = "Login successfully";
                    result.Data = new
                    {
                        Token = token,
                        Email = userLogin.Email,
                        Password = userLogin.Password,
                        Role = user.Role
                    };
                }
                else
                {
                    // Incorrect email or password case
                    result.IsSuccess = false;
                    result.StatusCode = 401;
                    result.Message = "Wrong email or password";
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                result.IsSuccess = false;
                result.StatusCode = 500;
                result.Message = ex.Message;
            }

            return result;
        }
        #endregion


        public UserDTO UpdateUser(UserDTO userUpdate) 
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(int idTmp)
        {
            throw new NotImplementedException();
        }

        public List<UserDTO> GetAll() 
        {
            var users = _userRepository.Get().ToList();
            var user = users.Select(user => new UserDTO
            {
                UserId = user.UserId,
            }).ToList();
            return user;
        }

        public UserDTO GetById(int idTmp) 
        {
            throw new NotImplementedException();
        }

    }

}
