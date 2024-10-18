using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Repositories.Repositories;
using Services.Helpers;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IOrderService
    {
        public Task<ResultModel> CreateOrder(OrderDTO orderCreate);
        public OrderDTO UpdateOrder(OrderDTO orderUpdate);
        public bool DeleteOrder(int idTmp);
        public List<OrderDTO> GetAll();
        public OrderDTO GetById(int idTmp);
    }

    public class OrderService : IOrderService
    {

        private readonly IOrderRepository _orderRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public OrderService(IOrderRepository orderRepository, ISeatRepository seatRepository, IFlightRepository flightRepository, IOrderDetailRepository orderDetailRepository, JwtTokenHelper jwtTokenHelper)
        {
            _orderRepository = orderRepository;
            _seatRepository = seatRepository;
            _flightRepository = flightRepository;
            _orderDetailRepository = orderDetailRepository;
            _jwtTokenHelper = jwtTokenHelper;
        }

        public async Task<ResultModel> CreateOrder(OrderDTO orderCreate)
        {
            try
            {

                var order = new Order
                {
                    UserId = orderCreate.UserId,
                    OrderDate = DateTime.Now,
                    Status = OrderEnums.Pending.ToString(),
                    TotalPrice = orderCreate.TotalPrice
                };

                foreach (var passenger in orderCreate.Passengers)
                {
                    if (passenger.FlightId == null || passenger.SeatId == null)
                    {
                        return new ResultModel
                        {
                            IsSuccess = false,
                            Message = "FlightId and SeatId cannot be null",
                            StatusCode = 400
                        };
                    }

                    var flight = await _flightRepository.GetAsync(passenger.FlightId.Value);
                    var seat = await _seatRepository.GetAsync(passenger.SeatId.Value);

                    if (seat.Status == SeatEnums.Taken.ToString())
                    {
                        return new ResultModel
                        {
                            IsSuccess = false,
                            Message = $"Seat {seat.SeatId} is already taken",
                            StatusCode = 400
                        };
                    }

                    var orderDetail = new OrderDetail
                    {
                        Order = order,
                        Name = passenger.Name ?? throw new ArgumentNullException(nameof(passenger.Name)),
                        DoB = passenger.DoB,
                        Nationality = passenger.Nationality,
                        Email = passenger.Email,
                        FlightId = flight.FlightId,
                        TripType = passenger.TripType ?? throw new ArgumentNullException(nameof(passenger.TripType)),
                        SeatId = seat.SeatId,
                        Status = OrderDetailEnums.Pending.ToString(),
                        TotalAmount = passenger.Price
                    };

                    order.OrderDetails.Add(orderDetail);

                    seat.Status = SeatEnums.Taken.ToString();
                    _seatRepository.Update(seat);
                }

                await _orderRepository.CreateAsync(order);
                await _orderRepository.SaveAsync();

                return new ResultModel
                {
                    IsSuccess = true,
                    Message = "Order created successfully",
                    Data = new OrderDTO
                    {
                        OrderId = order.OrderId,
                        UserId = order.UserId,
                        OrderDate = order.OrderDate,
                        Status = order.Status,
                        TotalPrice = order.TotalPrice,
                        OrderDetails = order.OrderDetails.Select(od => new OrderDetailDTO
                        {
                            OrderDetailId = od.OrderDetailId,
                            Name = od.Name ?? throw new ArgumentNullException(nameof(od.Name)),
                            DoB = od.DoB,
                            Nationality = od.Nationality,
                            Email = od.Email,
                            FlightId = od.FlightId,
                            TripType = od.TripType ?? throw new ArgumentNullException(nameof(od.TripType)),
                            SeatId = od.SeatId,
                            Status = od.Status ?? throw new ArgumentNullException(nameof(od.Status)),
                            TotalAmount = od.TotalAmount
                        }).ToList()
                    },
                    StatusCode = 201
                };
            }
            catch (Exception ex)
            {
                return new ResultModel
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    StatusCode = 500
                };
            }
        }


        public OrderDTO UpdateOrder(OrderDTO orderUpdate)
        {
            throw new NotImplementedException();
        }

        public bool DeleteOrder(int idTmp)
        {
            throw new NotImplementedException();
        }

        public List<OrderDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public OrderDTO GetById(int idTmp)
        {
            throw new NotImplementedException();
        }

    }

}
