using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.EntityFrameworkCore;
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

    Task<ResultModel> GetAllFlightReports();
    Task<ResultModel> GetFlightReportByOrderID(int orderId);
}

public class FlightService : IFlightService
{

    private readonly IFlightRepository _flightRepository;
    private readonly IFlightRouteRepository _flightRouteRepository;
    private readonly IPlaneRepository _planeRepository;
    private readonly ISeatRepository _seatRepository;
    private readonly IOrderDetailRepository _orderDetailRepository;


    public FlightService(IFlightRepository flightRepository, IFlightRouteRepository flightRouteRepository, IPlaneRepository planeRepository, ISeatRepository seatRepository, IOrderDetailRepository orderDetailRepository)
    {
        _flightRepository = flightRepository;
        _flightRouteRepository = flightRouteRepository;
        _planeRepository = planeRepository;
        _seatRepository = seatRepository;
        _orderDetailRepository = orderDetailRepository;
    }

    public FlightDTO CreateFlight(FlightDTO flightCreate)
    {
       

       
        var flight = new Flight
        {
            FlightId = flightCreate.FlightId,
            PlaneId = flightCreate.PlaneId,
            FlightNumber = flightCreate.FlightNumber,
            DepartureLocation = flightCreate.DepartureLocation,
            DepartureTime = flightCreate.DepartureTime,
            ArrivalLocation = flightCreate.ArrivalLocation,
            ArrivalTime = flightCreate.ArrivalTime,
            FlightStatus = "Available"
        };

        
        _flightRepository.Create(flight);
        _flightRepository.Save();

       
        var plane = _planeRepository.Get().FirstOrDefault(p => p.PlaneId == flightCreate.PlaneId);
        if (plane != null)
        {
            
            plane.Status = PlaneStatus.InUse.ToString(); 
            _planeRepository.Update(plane);
            _planeRepository.Save(); 
        }

        
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
        // Fetch the flight data, including related FlightRoute entities for Departure and Arrival
        var flights = _flightRepository.Get()
            .Include(f => f.DepartureLocationNavigation) // Include DepartureLocation
            .Include(f => f.ArrivalLocationNavigation) // Include ArrivalLocation
            .ToList();

      
        var flightDTOs = flights.Select(x => new FlightDTO
        {
            FlightId = x.FlightId,
            PlaneId = x.PlaneId,
            FlightNumber = x.FlightNumber,
            DepartureLocation = x.DepartureLocation,
            DepartureLocationName = x.DepartureLocationNavigation?.Location, 
            DepartureTime = x.DepartureTime,
            ArrivalLocation = x.ArrivalLocation,
            ArrivalLocationName = x.ArrivalLocationNavigation?.Location, 
            ArrivalTime = x.ArrivalTime,
            FlightStatus = x.FlightStatus,
            
        }).ToList();

        return flightDTOs;
    }



    public FlightDTO GetById(int idTmp)
    {
        var flight = _flightRepository.Get().FirstOrDefault(f => f.FlightId == idTmp);

        // Check if the flight entity was found
        if (flight == null)
        {
            return null; // Or handle it as appropriate, like throwing an exception or returning a custom error DTO
        }

        // Map the flight entity to a FlightDTO
        var flightDTO = new FlightDTO
        {
            FlightId = flight.FlightId,
            PlaneId = flight.PlaneId,
            FlightNumber = flight.FlightNumber,
            DepartureLocation = flight.DepartureLocation,
            DepartureTime = flight.DepartureTime,
            ArrivalLocation = flight.ArrivalLocation,
            ArrivalTime = flight.ArrivalTime,
            FlightStatus = flight.FlightStatus
        };

        // Return the mapped FlightDTO
        return flightDTO;
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



    #region ManageFlightReport
    public async Task<ResultModel> GetAllFlightReports()
    {
        var result = new ResultModel();
        try
        {
            var orders = _orderDetailRepository.Get().ToList();
            var flights = _flightRepository.Get().ToList();
            var flightRoutes = _flightRouteRepository.Get().ToList();
            var planes = _planeRepository.Get().ToList();
            var seats = _seatRepository.Get().ToList();

            var flightReports = (from order in orders
                                 join flight in flights on order.FlightId equals flight.FlightId
                                 join departureRoute in flightRoutes on flight.DepartureLocation equals departureRoute.FlightRouteId into departureGroup
                                 from departureRoute in departureGroup.DefaultIfEmpty()
                                 join arrivalRoute in flightRoutes on flight.ArrivalLocation equals arrivalRoute.FlightRouteId into arrivalGroup
                                 from arrivalRoute in arrivalGroup.DefaultIfEmpty()
                                 join plane in planes on flight.PlaneId equals plane.PlaneId into planeGroup
                                 from plane in planeGroup.DefaultIfEmpty()
                                 join seat in seats on order.SeatId equals seat.SeatId into seatGroup
                                 from seat in seatGroup.DefaultIfEmpty()
                                 let availableBusinessSeats = seats.Count(s => s.PlaneId == flight.PlaneId && s.SeatNumer >= 1 && s.SeatNumer <= 12 && s.Status == "Available")
                                 let availableEconomySeats = seats.Count(s => s.PlaneId == flight.PlaneId && s.SeatNumer >= 13 && s.SeatNumer <= 42 && s.Status == "Available")
                                 select new FlightReportDTO
                                 {
                                     OrderId = order.OrderDetailId,
                                     PaymentStatus = order.Status,
                                     //BookingStatus,...
                                     FlightId = flight.FlightId,
                                     PlaneId = flight.PlaneId,
                                     Name = order.Name,
                                     Email = order.Email,
                                     SeatNumber = seat?.SeatNumer ?? 0,
                                     PlaneCode = plane?.PlaneCode,
                                     FlightNumber = flight.FlightNumber,
                                     DepartureLocation = flight.DepartureLocation,
                                     DepartureLocationName = departureRoute?.Location ?? "Unknown",
                                     DepartureTime = flight.DepartureTime,
                                     ArrivalLocation = flight.ArrivalLocation,
                                     ArrivalLocationName = arrivalRoute?.Location ?? "Unknown",
                                     ArrivalTime = flight.ArrivalTime,
                                     FlightStatus = flight.FlightStatus,
                                     AvailableBusinessSeats = availableBusinessSeats,
                                     AvailableEconomySeats = availableEconomySeats,
                                     //BookingDate = order.BookingDate,
                                     //CancellationDate = order.CancellationDate
                                 }).ToList();

            result.IsSuccess = true;
            result.Message = "Reports retrieved successfully.";
            result.Data = flightReports;
            result.StatusCode = 200;
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = $"Failed to retrieve accounts: {ex.Message}";
            result.StatusCode = 500;
        }

        return result;
    }

    #endregion

    #region GetFlightReportByID

    public async Task<ResultModel> GetFlightReportByOrderID(int orderId)
    {
        var result = new ResultModel();
        try
        {
            
            var order = _orderDetailRepository.Get().FirstOrDefault(o => o.OrderDetailId == orderId);
            if (order == null)
            {
                result.IsSuccess = false;
                result.Message = "Order not found for the specified OrderId.";
                result.StatusCode = 404;
                return result;
            }

         
            var flight = _flightRepository.Get().FirstOrDefault(f => f.FlightId == order.FlightId);
            if (flight == null)
            {
                result.IsSuccess = false;
                result.Message = "Flight not found for the specified Order.";
                result.StatusCode = 404;
                return result;
            }

            var flightRoutes = _flightRouteRepository.Get().ToList();
            var plane = _planeRepository.Get().FirstOrDefault(p => p.PlaneId == flight.PlaneId);
            var seats = _seatRepository.Get().ToList();
            var seat = seats.FirstOrDefault(s => s.SeatId == order.SeatId);

            var departureRoute = flightRoutes.FirstOrDefault(fr => fr.FlightRouteId == flight.DepartureLocation);
            var arrivalRoute = flightRoutes.FirstOrDefault(fr => fr.FlightRouteId == flight.ArrivalLocation);

            
            var availableBusinessSeats = seats.Count(s => s.PlaneId == flight.PlaneId && s.SeatNumer >= 1 && s.SeatNumer <= 12 && s.Status == "Available");
            var availableEconomySeats = seats.Count(s => s.PlaneId == flight.PlaneId && s.SeatNumer >= 13 && s.SeatNumer <= 42 && s.Status == "Available");

            var flightReport = new FlightReportDTO
            {
                OrderId = order.OrderDetailId,
                PaymentStatus = order.Status,
                FlightId = flight.FlightId,
                PlaneId = flight.PlaneId,
                Name = order.Name,
                Email = order.Email,
                PlaneCode = plane?.PlaneCode,
                SeatNumber = seat?.SeatNumer ?? 0,
                FlightNumber = flight.FlightNumber,
                DepartureLocation = flight.DepartureLocation,
                DepartureLocationName = departureRoute?.Location ?? "Unknown",
                DepartureTime = flight.DepartureTime,
                ArrivalLocation = flight.ArrivalLocation,
                ArrivalLocationName = arrivalRoute?.Location ?? "Unknown",
                ArrivalTime = flight.ArrivalTime,
                FlightStatus = flight.FlightStatus,
                AvailableBusinessSeats = availableBusinessSeats,
                AvailableEconomySeats = availableEconomySeats,
            };

            result.IsSuccess = true;
            result.Message = "Flight report retrieved successfully.";
            result.Data = flightReport;
            result.StatusCode = 200;
        }
        catch (Exception ex)
        {
            result.IsSuccess = false;
            result.Message = $"An error occurred: {ex.Message}";
            result.StatusCode = 500;
        }

        return result;
    }


    #endregion
}

