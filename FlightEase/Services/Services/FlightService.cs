using BusinessObjects.DTOs;
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
        public List<FlightDTO> SearchFlight();
    }

    public class FlightService : IFlightService
    {

        private readonly IFlightRepository _flightRepository;
        private readonly IFlightRouteRepository _flightRouteRepository;
        public FlightService(IFlightRepository flightRepository, IFlightRouteRepository flightRouteRepository)
        {
            _flightRepository = flightRepository;
            _flightRouteRepository = flightRouteRepository;
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
            var exitFlight = _flightRepository.Get().SingleOrDefault(x => x.FlightId == idTmp);
            if (exitFlight == null)
            {
                return false;

            }
            _flightRepository.Delete(exitFlight);
            _flightRepository.Save();
            return true;
               
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
        public List<FlightDTO> SearchFlight()
        {

            var flights = _flightRepository.Get().ToList();
            var flightRoutes = _flightRouteRepository.Get().ToList();


            var flightDTOs = (from flight in flights
                              join departureRoute in flightRoutes on flight.DepartureLocation equals departureRoute.FlightRouteId into departureGroup
                              from departureRoute in departureGroup.DefaultIfEmpty()
                              join arrivalRoute in flightRoutes on flight.ArrivalLocation equals arrivalRoute.FlightRouteId into arrivalGroup
                              from arrivalRoute in arrivalGroup.DefaultIfEmpty()
                              select new FlightDTO
                              {
                                  FlightId = flight.FlightId,
                                  FlightNumber = flight.FlightNumber,
                                  DepartureLocation = flight.DepartureLocation,
                                  DepartureLocationName = departureRoute?.Location ?? "Unknown",
                                  DepartureTime = flight.DepartureTime,
                                  ArrivalLocation = flight.ArrivalLocation,
                                  ArrivalLocationName = arrivalRoute?.Location ?? "Unknown",
                                  ArrivalTime = flight.ArrivalTime,
                                  FlightStatus = flight.FlightStatus,
                              }).ToList();

            return flightDTOs;
        }

    }

}
