using BusinessObjects.DTOs;
using Repositories.Repositories;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IMembershipService {
        public MemBershipDTO CreateMembership(MemBershipDTO membershipCreate);
        public MemBershipDTO UpdateMembership(MemBershipDTO membershipUpdate);
        public bool DeleteMembership(int idTmp);
        public List<MemBershipDTO> GetAll();
        public MemBershipDTO GetById(int idTmp);
    }

    public class MembershipService : IMembershipService {

      private readonly IMembershipRepository _membershipRepository;

        public MembershipService(IMembershipRepository membershipRepository)
        {
            _membershipRepository = membershipRepository;
        }

        public MemBershipDTO CreateMembership(MemBershipDTO membershipCreate)
        {
            throw new NotImplementedException();
        }

        public MemBershipDTO UpdateMembership(MemBershipDTO membershipUpdate) 
        {
            throw new NotImplementedException();
        }

        public bool DeleteMembership(int idTmp)
        {
            throw new NotImplementedException();
        }

        public List<MemBershipDTO> GetAll() 
        {
            throw new NotImplementedException();
        }

        public MemBershipDTO GetById(int idTmp) 
        {
            throw new NotImplementedException();
        }

    }

}
