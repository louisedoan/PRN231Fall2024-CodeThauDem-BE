using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories;

namespace FlightEaseDB.Services.Services
{

    public interface IFlightRouteService
    {
        public Task<ResultModel> CreateLocation(FlightRouteDTO flightRoute);
        public Task<ResultModel> UpdateLocation(FlightRouteUpdateDTO location);
        public Task<ResultModel> DeleteLocation(int locationId);
        public Task<ResultModel> GetAll();
        public Task<ResultModel> GetById(int idTmp);
    }

    public class FlightRouteService : IFlightRouteService
    {

        private readonly IFlightRouteRepository _flightrouteRepository;

        public FlightRouteService(IFlightRouteRepository flightrouteRepository)
        {
            _flightrouteRepository = flightrouteRepository;
        }

        public async Task<ResultModel> CreateLocation(FlightRouteDTO flightRoute)
        {
            ResultModel result = new ResultModel();
            try
            {
                var existingLocation = await _flightrouteRepository.FirstOrDefaultAsync(l => l.Location == flightRoute.Location);
                if (existingLocation != null)
                {
                    result.Message = "Location already exists";
                    result.IsSuccess = false;
                    result.StatusCode = 200;
                    return result;
                }
                var newLocation = new FlightRoute
                {
                    Location = flightRoute.Location
                };

                await _flightrouteRepository.CreateAsync(newLocation);
                await _flightrouteRepository.SaveAsync();

                result.Message = "Location created successfully";
                result.IsSuccess = true;
                result.StatusCode = 200;
                result.Data = flightRoute;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
                result.StatusCode = 500;
                result.Data = null;
            }
            return result;
        }

        public async Task<ResultModel> UpdateLocation(FlightRouteUpdateDTO location)
        {
            ResultModel result = new ResultModel();
            try
            {
                var existingLocation = await _flightrouteRepository.FirstOrDefaultAsync(l => l.FlightRouteId == location.FlightRouteId);
                if (existingLocation == null)
                {
                    result.Message = "Location does not exist";
                    result.IsSuccess = false;
                    result.StatusCode = 200;
                    return result;
                }
                existingLocation.Location = location.Location;

                _flightrouteRepository.Update(existingLocation);
                await _flightrouteRepository.SaveAsync();
                result.Message = "Location updated successfully";
                result.IsSuccess = true;
                result.StatusCode = 200;
                result.Data = existingLocation;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
                result.StatusCode = 500;
                result.Data = null;
            }
            return result;

        }

        public async Task<ResultModel> DeleteLocation(int locationId)
        {
            ResultModel result = new ResultModel();
            try
            {
                var location = await _flightrouteRepository.FirstOrDefaultAsync(l => l.FlightRouteId == locationId);
                if (location == null)
                {
                    result.Message = "Location not found";
                    result.IsSuccess = false;
                    result.StatusCode = 200;
                    return result;
                }

                _flightrouteRepository.Delete(location);
                await _flightrouteRepository.SaveAsync();

                result.Message = "Location deleted successfully";
                result.IsSuccess = true;
                result.StatusCode = 200;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
                result.StatusCode = 500;
                result.Data = null;
            }
            return result;
        }
        public async Task<ResultModel> GetAll()
        {
            ResultModel result = new ResultModel();
            try
            {
                var flightRoutes = await _flightrouteRepository.Get().ToListAsync();
                var flightRouteDTOs = flightRoutes.Select(fr => new FlightRouteUpdateDTO
                {
                    FlightRouteId = fr.FlightRouteId,
                    Location = fr.Location
                }).ToList();

                result.Message = "Flight routes retrieved successfully";
                result.IsSuccess = true;
                result.StatusCode = 200;
                result.Data = flightRouteDTOs;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
                result.StatusCode = 500;
                result.Data = null;
            }
            return result;
        }

        public async Task<ResultModel> GetById(int idTmp)
        {
            ResultModel result = new ResultModel();
            try
            {
                var flightRoute = await _flightrouteRepository.FirstOrDefaultAsync(fr => fr.FlightRouteId == idTmp);
                if (flightRoute == null)
                {
                    result.Message = "Flight route not found";
                    result.IsSuccess = false;
                    result.StatusCode = 404;
                    return result;
                }

                var flightRouteDTO = new FlightRouteDTO
                {
                    Location = flightRoute.Location
                };

                result.Message = "Flight route retrieved successfully";
                result.IsSuccess = true;
                result.StatusCode = 200;
                result.Data = flightRouteDTO;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
                result.StatusCode = 500;
                result.Data = null;
            }
            return result;
        }

    }

}
