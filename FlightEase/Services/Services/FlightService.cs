using BusinessObjects.DTOs;
using FlightEaseDB.Repositories.Repositories;

namespace FlightEase.Services.Services
{

    public interface IFlightService {
        public FlightDTO CreateFlight(FlightDTO flightCreate);
        public FlightDTO UpdateFlight(FlightDTO flightUpdate);
        public bool DeleteFlight(int idTmp);
        public List<FlightDTO> GetAll();
        public FlightDTO GetById(int idTmp);
    }

    public class FlightService : IFlightService {

      private readonly IFlightRepository _flightRepository;

        public FlightService(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public FlightDTO CreateFlight(FlightDTO flightCreate)
        {
            throw new NotImplementedException();
        }

        public FlightDTO UpdateFlight(FlightDTO flightUpdate) 
        {
            throw new NotImplementedException();
        }

        public bool DeleteFlight(int idTmp)
        {
            throw new NotImplementedException();
        }

        public List<FlightDTO> GetAll() 
        {
            throw new NotImplementedException();
        }

        public FlightDTO GetById(int idTmp) 
        {
            throw new NotImplementedException();
        }

    }

}
