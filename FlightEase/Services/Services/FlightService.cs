using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Repositories.Repositories;

namespace FlightEase.Services.Services
{

    public interface IFlightService {
        public FlightDTO CreateFlight(FlightDTO flightCreate);
        public FlightDTO UpdateFlight(FlightDTO flightUpdate);
        public bool DeleteFlight(int idTmp);
        public List<FlightDTO> GetAll();
        public FlightDTO GetById(int idTmp);
        public IQueryable<FlightDTO> SearchFlight();
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
            var flight = _flightRepository.Get().ToList();
            var flightDTO = flight.Select(x => new FlightDTO
            {
                FlightId = x.FlightId,


                PilotId = x.PilotId,
                FlightNumber = x.FlightNumber,

                DepartureTime = x.DepartureTime,

                ArrivalTime = x.ArrivalTime,

                FlightStatus = x.FlightStatus,
                EmptySeat = x.EmptySeat,

            }).ToList();
            return flightDTO;
        }

        public FlightDTO GetById(int idTmp) 
        {
            throw new NotImplementedException();
        }
        public IQueryable<FlightDTO> SearchFlight()
        {
            var flights = _flightRepository.Get().AsQueryable(); 

        
            var flightDTOs = flights.Select(flight => new FlightDTO
            {
                FlightId = flight.FlightId,
                FlightNumber = flight.FlightNumber,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                FlightStatus = flight.FlightStatus,
                EmptySeat = flight.EmptySeat
            });

            return flightDTOs;
        }
    }

}
