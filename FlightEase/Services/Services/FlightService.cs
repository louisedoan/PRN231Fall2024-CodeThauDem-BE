using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Repositories.Repositories;

public interface IFlightService
{
    public FlightDTO CreateFlight(FlightDTO flightCreate);
    public FlightDTO UpdateFlight(FlightDTO flightUpdate);
    public bool DeleteFlight(int idTmp);
    public List<FlightDTO> GetAll();
    public FlightDTO GetById(int idTmp);
    public List<FlightDTO> SearchFlight();
    List<FlightDTO> SearchOneWayFlight(int departureLocation, int arrivalLocation, DateTime departureDate);
}

public class FlightService : IFlightService
{

    private readonly IFlightRepository _flightRepository;
    private readonly IFlightRouteRepository _flightRouteRepository;
    private readonly IPlaneRepository _planeRepository;
    private readonly ISeatRepository _seatRepository;


    public FlightService(IFlightRepository flightRepository, IFlightRouteRepository flightRouteRepository, IPlaneRepository planeRepository, ISeatRepository seatRepository)
    {
        _flightRepository = flightRepository;
        _flightRouteRepository = flightRouteRepository;
        _planeRepository = planeRepository;
        _seatRepository = seatRepository;
    }

    public FlightDTO CreateFlight(FlightDTO flightCreate)
    {
        var flight = new Flight
        {
            FlightId = flightCreate.FlightId,

            FlightNumber = flightCreate.FlightNumber,
            DepartureLocation = flightCreate.DepartureLocation,
            DepartureTime = flightCreate.DepartureTime,
            ArrivalLocation = flightCreate.ArrivalLocation,
            ArrivalTime = flightCreate.ArrivalTime,
            FlightStatus = flightCreate.FlightStatus,

        };
        _flightRepository.Create(flight);
        _flightRepository.Save();
        var flightDTO = new FlightDTO
        {
            FlightId = flight.FlightId,

            FlightNumber = flight.FlightNumber,
            DepartureLocation = flight.DepartureLocation,
            DepartureTime = flight.DepartureTime,
            ArrivalLocation = flight.ArrivalLocation,
            ArrivalTime = flight.ArrivalTime,
            FlightStatus = flight.FlightStatus,

        };
        return flightDTO;
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
          //  PilotId = x.PilotId,
          PlaneId = x.PlaneId,
            FlightNumber = x.FlightNumber,
            DepartureTime = x.DepartureTime,
            ArrivalTime = x.ArrivalTime,
            FlightStatus = x.FlightStatus,
        //    EmptySeat = x.EmptySeat,
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

    public List<FlightDTO> SearchOneWayFlight(int departureLocation, int arrivalLocation, DateTime departureDate)
    {
        var flights = _flightRepository.Get()
            .Where(f => f.DepartureLocation == departureLocation && f.ArrivalLocation == arrivalLocation && f.DepartureTime.Value.Date == departureDate.Date && f.FlightStatus == FlightStatus.Available.ToString())
            .ToList();

        var flightRoutes = _flightRouteRepository.Get().ToList();
        var planes = _planeRepository.Get().ToList();
        var seats = _seatRepository.Get().ToList();

        var flightDTOs = (from flight in flights
                          join departureRoute in flightRoutes on flight.DepartureLocation equals departureRoute.FlightRouteId into departureGroup
                          from departureRoute in departureGroup.DefaultIfEmpty()
                          join arrivalRoute in flightRoutes on flight.ArrivalLocation equals arrivalRoute.FlightRouteId into arrivalGroup
                          from arrivalRoute in arrivalGroup.DefaultIfEmpty()
                          join plane in planes on flight.PlaneId equals plane.PlaneId into planeGroup
                          from plane in planeGroup.DefaultIfEmpty()
                          let availableBusinessSeats = seats.Count(s => s.PlaneId == flight.PlaneId && s.SeatNumer >= 1 && s.SeatNumer <= 12 && s.Status == "Available")
                          let availableEconomySeats = seats.Count(s => s.PlaneId == flight.PlaneId && s.SeatNumer >= 13 && s.SeatNumer <= 42 && s.Status == "Available")


                          select new FlightDTO
                          {
                              FlightId = flight.FlightId,
                              FlightNumber = flight.FlightNumber,
                              PlaneId = flight.PlaneId,
                              PlaneCode = plane?.PlaneCode,
                              DepartureLocation = flight.DepartureLocation,
                              DepartureLocationName = departureRoute?.Location ?? "Unknown",
                              DepartureTime = flight.DepartureTime,
                              ArrivalLocation = flight.ArrivalLocation,
                              ArrivalLocationName = arrivalRoute?.Location ?? "Unknown",
                              ArrivalTime = flight.ArrivalTime,
                              FlightStatus = flight.FlightStatus,
                              AvailableBusinessSeats = availableBusinessSeats,
                              AvailableEconomySeats = availableEconomySeats
                          }).ToList();

        return flightDTOs;
    }
}

