using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Repositories.Repositories;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IUserService {
        public UserDTO CreateUser(UserDTO userCreate);
        public UserDTO UpdateUser(UserDTO userUpdate);
        public bool DeleteUser(int idTmp);
        public List<UserDTO> GetAll();
        public UserDTO GetById(int idTmp);
    }

	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public UserDTO CreateUser(UserDTO userCreate)
		{
			userCreate.Role = "Staff";
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
	}


}
