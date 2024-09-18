using BusinessObjects.DTOs;
using FlightEaseDB.Repositories.Repositories;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IPilotService {
        public PilotDTO CreatePilot(PilotDTO pilotCreate);
        public PilotDTO UpdatePilot(PilotDTO pilotUpdate);
        public bool DeletePilot(int idTmp);
        public List<PilotDTO> GetAll();
        public PilotDTO GetById(int idTmp);
    }

    public class PilotService : IPilotService {

      private readonly IPilotRepository _pilotRepository;

        public PilotService(IPilotRepository pilotRepository)
        {
            _pilotRepository = pilotRepository;
        }

        public PilotDTO CreatePilot(PilotDTO pilotCreate)
        {
            throw new NotImplementedException();
        }

        public PilotDTO UpdatePilot(PilotDTO pilotUpdate) 
        {
            throw new NotImplementedException();
        }

        public bool DeletePilot(int idTmp)
        {
            throw new NotImplementedException();
        }

        public List<PilotDTO> GetAll() 
        {
            throw new NotImplementedException();
        }

        public PilotDTO GetById(int idTmp) 
        {
            throw new NotImplementedException();
        }

    }

}
