using BusinessObjects.DTOs;
using Repositories.Repositories;

namespace FlightEaseDB.Services.Services
{

    public interface IFlightRouteService {
        public FlightRouteDTO CreateFlightRoute(FlightRouteDTO flightrouteCreate);
        public FlightRouteDTO UpdateFlightRoute(FlightRouteDTO flightrouteUpdate);
        public bool DeleteFlightRoute(int idTmp);
        public List<FlightRouteDTO> GetAll();
        public FlightRouteDTO GetById(int idTmp);
    }

    public class FlightRouteService : IFlightRouteService {

      private readonly IFlightRouteRepository _flightrouteRepository;

        public FlightRouteService(IFlightRouteRepository flightrouteRepository)
        {
            _flightrouteRepository = flightrouteRepository;
        }

        public FlightRouteDTO CreateFlightRoute(FlightRouteDTO flightrouteCreate)
        {
            throw new NotImplementedException();
        }

        public FlightRouteDTO UpdateFlightRoute(FlightRouteDTO flightrouteUpdate) 
        {
            throw new NotImplementedException();
        }

        public bool DeleteFlightRoute(int idTmp)
        {
            throw new NotImplementedException();
        }

        public List<FlightRouteDTO> GetAll() 
        {
            throw new NotImplementedException();
        }

        public FlightRouteDTO GetById(int idTmp) 
        {
            throw new NotImplementedException();
        }

    }

}
