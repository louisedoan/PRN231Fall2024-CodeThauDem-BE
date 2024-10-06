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
