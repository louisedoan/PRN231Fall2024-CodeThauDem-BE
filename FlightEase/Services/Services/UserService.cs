using BusinessObjects.DTOs;
using FlightEaseDB.Repositories.Repositories;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IUserService {
        public UserDTO CreateUser(UserDTO userCreate);
        public UserDTO UpdateUser(UserDTO userUpdate);
        public bool DeleteUser(int idTmp);
        public List<UserDTO> GetAll();
        public UserDTO GetById(int idTmp);
    }

    public class UserService : IUserService {

      private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDTO CreateUser(UserDTO userCreate)
        {
            throw new NotImplementedException();
        }

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
            throw new NotImplementedException();
        }

        public UserDTO GetById(int idTmp) 
        {
            throw new NotImplementedException();
        }

    }

}