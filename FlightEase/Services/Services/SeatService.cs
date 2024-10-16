using BusinessObjects.DTOs;
using Repositories.Repositories;

namespace Services.Services
{
    public interface ISeatService
    {
        Task<ResultModel> GetBusinessClassSeatsAsync(int flightId);
        Task<ResultModel> GetEconomyClassSeatsAsync(int flightId);
    }
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IFlightRepository _flightRepository;

        public SeatService(ISeatRepository seatRepository, IFlightRepository flightRepository)
        {
            _seatRepository = seatRepository;
            _flightRepository = flightRepository;
        }

        public async Task<ResultModel> GetBusinessClassSeatsAsync(int flightId)
        {
            var flight = await _flightRepository.GetAsync(flightId);
            if (flight == null)
            {
                return new ResultModel
                {
                    IsSuccess = false,
                    Message = "Flight not found",
                    StatusCode = 404
                };
            }

            var seats = _seatRepository.Get()
                .Where(s => s.PlaneId == flight.PlaneId && s.SeatNumer >= 1 && s.SeatNumer <= 12)
                .Select(s => new SeatDTO
                {
                    SeatId = s.SeatId,
                    SeatNumber = s.SeatNumer,
                    Class = s.Class,
                    Status = s.Status,
                    PlaneId = s.PlaneId,
                    Price = s.Price
                })
                .ToList();

            return new ResultModel
            {
                IsSuccess = true,
                Message = "Business class seats retrieved successfully",
                Data = seats,
                StatusCode = 200
            };
        }

        public async Task<ResultModel> GetEconomyClassSeatsAsync(int flightId)
        {
            var flight = await _flightRepository.GetAsync(flightId);
            if (flight == null)
            {
                return new ResultModel
                {
                    IsSuccess = false,
                    Message = "Flight not found",
                    StatusCode = 404
                };
            }

            var seats = _seatRepository.Get()
                .Where(s => s.PlaneId == flight.PlaneId && s.SeatNumer >= 13 && s.SeatNumer <= 42)
                .Select(s => new SeatDTO
                {
                    SeatId = s.SeatId,
                    SeatNumber = s.SeatNumer,
                    Class = s.Class,
                    Status = s.Status,
                    PlaneId = s.PlaneId,
                    Price = s.Price
                })
                .ToList();

            return new ResultModel
            {
                IsSuccess = true,
                Message = "Economy class seats retrieved successfully",
                Data = seats,
                StatusCode = 200
            };
        }
    }
}
