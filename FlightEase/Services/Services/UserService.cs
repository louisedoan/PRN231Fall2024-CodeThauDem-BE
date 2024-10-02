
using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Repositories.Repositories;
using Services.Helpers;

namespace FlightEaseDB.BusinessLogic.Services
{

	public interface IUserService
	{
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
		public UserService(IUserRepository userRepository, JwtTokenHelper jwtTokenHelper)
		{
			_userRepository = userRepository;
			_jwtTokenHelper = jwtTokenHelper;
		}

		public UserDTO CreateUser(UserDTO userCreate)
		{
			userCreate.Role = UserRole.Manager.ToString();
			var user = new User
			{
				Email = userCreate.Email,
				Password = userCreate.Password,
				Gender = userCreate.Gender,
				Nationality = userCreate.Nationality,
				Address = userCreate.Address,
				Fullname = userCreate.Fullname,
				Dob = userCreate.Dob,
				Role = userCreate.Role,
				MembershipId = userCreate.MembershipId,
				Status = userCreate.Status
			};

			//user.Role = "Staff";

			// Gọi thẳng phương thức từ UserRepository (BaseRepository)
			_userRepository.Create(user);
			_userRepository.Save();

			userCreate.UserId = user.UserId; // Lấy ID từ entity sau khi lưu
			return userCreate;
		}

		public UserDTO UpdateUser(UserDTO userUpdate)
		{
			var user = _userRepository.Get(userUpdate.UserId);
			if (user == null) return null;

			// Cập nhật các trường
			user.Email = userUpdate.Email;
			user.Password = userUpdate.Password;
			user.Gender = userUpdate.Gender;
			user.Nationality = userUpdate.Nationality;
			user.Address = userUpdate.Address;
			user.Fullname = userUpdate.Fullname;
			user.Dob = userUpdate.Dob;
			user.Role = userUpdate.Role;
			user.MembershipId = userUpdate.MembershipId;
			user.Status = userUpdate.Status;

			// Gọi phương thức update từ BaseRepository
			_userRepository.Update(user);
			_userRepository.Save();

			return userUpdate;
		}

		public bool DeleteUser(int idTmp)
		{
			var user = _userRepository.Get(idTmp);
			if (user == null) return false;

			// Gọi phương thức xóa từ BaseRepository
			_userRepository.Delete(user);
			_userRepository.Save();

			return true;
		}

		public List<UserDTO> GetAll()
		{
			// Gọi phương thức lấy tất cả từ BaseRepository
			var users = _userRepository.Get().ToList();
			var userDtos = users.Select(user => new UserDTO
			{
				UserId = user.UserId,
				Email = user.Email,
				Gender = user.Gender,
				Nationality = user.Nationality,
				Address = user.Address,
				Fullname = user.Fullname,
				Dob = user.Dob,
				Role = user.Role,
				MembershipId = user.MembershipId,
				Status = user.Status
			}).ToList();

			return userDtos;
		}

		public UserDTO GetById(int idTmp)
		{
			// Gọi phương thức lấy đối tượng theo ID từ BaseRepository
			var user = _userRepository.Get(idTmp);
			if (user == null) return null;

			// Chuyển đổi entity sang DTO
			return new UserDTO
			{
				UserId = user.UserId,
				Email = user.Email,
				Gender = user.Gender,
				Nationality = user.Nationality,
				Address = user.Address,
				Fullname = user.Fullname,
				Dob = user.Dob,
				Role = user.Role,
				MembershipId = user.MembershipId,
				Status = user.Status
			};
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
						/*Email = userLogin.Email,
						Password = userLogin.Password,
						Role = user.Role*/
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

	}
}
