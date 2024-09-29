using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Repositories.Repositories;

namespace FlightEase.Services.Services
{

    public interface IFlightService
    {
        public FlightDTO CreateFlight(FlightDTO flightCreate);
        public FlightDTO UpdateFlight(FlightDTO flightUpdate);
        public bool DeleteFlight(int idTmp);
        public List<FlightDTO> GetAll();
        public FlightDTO GetById(int idTmp);
        public IQueryable<FlightDTO> SearchFlight();
    }

    public class FlightService : IFlightService
    {

        private readonly IFlightRepository _flightRepository;

        public FlightService(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public FlightDTO CreateFlight(FlightDTO flightCreate)
        {
            var flight = new Flight
            {
                FlightId = flightCreate.FlightId,
                PilotId = flightCreate.PilotId,
                FlightNumber = flightCreate.FlightNumber,
                DepartureLocation = flightCreate.DepartureLocation,
                DepartureTime = flightCreate.DepartureTime,
                ArrivalLocation = flightCreate.ArrivalLocation,
                ArrivalTime = flightCreate.ArrivalTime,
                FlightStatus = flightCreate.FlightStatus,
                EmptySeat = flightCreate.EmptySeat,
            };
            _flightRepository.Create(flight);
            _flightRepository.Save();
            var flightDTO = new FlightDTO
            {
                FlightId = flight.FlightId,
                PilotId = flight.PilotId,
                FlightNumber = flight.FlightNumber,
                DepartureLocation = flight.DepartureLocation,
                DepartureTime = flight.DepartureTime,
                ArrivalLocation = flight.ArrivalLocation,
                ArrivalTime = flight.ArrivalTime,
                FlightStatus = flight.FlightStatus,
                EmptySeat = flight.EmptySeat,
            };
            return flightDTO;
        }

        public FlightDTO UpdateFlight(FlightDTO flightUpdate)
        {
            var exitFlight = _flightRepository.Get().SingleOrDefault(x => x.FlightId == flightUpdate.FlightId);

            if (exitFlight != null)
            {
                exitFlight.PilotId = flightUpdate.PilotId;
                exitFlight.FlightNumber = flightUpdate.FlightNumber;
                exitFlight.DepartureLocation = flightUpdate.DepartureLocation;
                exitFlight.DepartureTime = flightUpdate.DepartureTime;
                exitFlight.ArrivalLocation = flightUpdate.ArrivalLocation;
                exitFlight.ArrivalTime = flightUpdate.ArrivalTime;
                exitFlight.FlightStatus = flightUpdate.FlightStatus;
                exitFlight.EmptySeat = flightUpdate.EmptySeat;


                _flightRepository.Update(exitFlight);
                _flightRepository.Save();
            }


            var flightDTO = new FlightDTO
            {
                FlightId = exitFlight.FlightId,
                PilotId = exitFlight.PilotId,
                FlightNumber = exitFlight.FlightNumber,
                DepartureLocation = exitFlight.DepartureLocation,
                DepartureTime = exitFlight.DepartureTime,
                ArrivalLocation = exitFlight.ArrivalLocation,
                ArrivalTime = exitFlight.ArrivalTime,
                FlightStatus = exitFlight.FlightStatus,
                EmptySeat = exitFlight.EmptySeat,
            };
            return flightDTO;

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
