using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories;
using Services.Helpers;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IOrderService
    {
        public Task<ResultModel> CreateOrder(OrderCreateDTO orderCreate);
        public OrderDTO UpdateOrder(OrderDTO orderUpdate);
        public bool DeleteOrder(int idTmp);
        public List<OrderDTO> GetAll();
        public OrderDTO GetById(int idTmp);
        public List<OrderDTO> GetOrderByUserId(int id);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly IFlightRepository _flightRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IFlightRouteRepository _flightRouteRepository;

        public OrderService(IOrderRepository orderRepository, ISeatRepository seatRepository,
                            IFlightRepository flightRepository, IOrderDetailRepository orderDetailRepository,
                            IFlightRouteRepository flightRouteRepository)
        {
            _orderRepository = orderRepository;
            _seatRepository = seatRepository;
            _flightRepository = flightRepository;
            _orderDetailRepository = orderDetailRepository;
            _flightRouteRepository = flightRouteRepository;
        }

        public async Task<ResultModel> CreateOrder(OrderCreateDTO orderCreate)
        {
            try
            {
                var order = new Order
                {
                    UserId = orderCreate.UserId,
                    OrderDate = DateTime.Now,
                    Status = "Pending",
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
                            StatusCode = 400,
                            Data = null
                        };
                    }

                    var flight = await _flightRepository.GetAsync(passenger.FlightId.Value);
                    var seat = await _seatRepository.GetAsync(passenger.SeatId.Value);

                    if (seat.Status == "Taken")
                    {
                        return new ResultModel
                        {
                            IsSuccess = false,
                            Message = $"Seat {seat.SeatId} is already taken",
                            StatusCode = 400,
                            Data = null
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
                        Status = "Pending",
                        TotalAmount = passenger.Price
                    };

                    order.OrderDetails.Add(orderDetail);

                    seat.Status = "Taken";
                    _seatRepository.Update(seat);
                }

                await _orderRepository.CreateAsync(order);
                await _orderRepository.SaveAsync();

                return new ResultModel
                {
                    IsSuccess = true,
                    Message = "Order created successfully",
                    Data = new OrderCreateDTO
                    {
                        OrderId = order.OrderId,
                        UserId = order.UserId,
                        OrderDate = order.OrderDate,
                        Status = order.Status,
                        TotalPrice = order.TotalPrice,
                        OrderDetails = order.OrderDetails.Select(od => new OrderDetailDTO
                        {
                            OrderDetailId = od.OrderDetailId,
                            Name = od.Name,
                            DoB = od.DoB,
                            Nationality = od.Nationality,
                            Email = od.Email,
                            FlightId = od.FlightId,
                            TripType = od.TripType,
                            SeatId = od.SeatId,
                            Status = od.Status,
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
            var existingOrder = _orderRepository.Get(orderUpdate.OrderId);
            if (existingOrder == null) return null;

            existingOrder.UserId = orderUpdate.UserId;
            existingOrder.OrderDate = orderUpdate.OrderDate;
            existingOrder.Status = orderUpdate.Status;
            existingOrder.TotalPrice = orderUpdate.TotalPrice;

            _orderRepository.Update(existingOrder);
            _orderRepository.Save();

            return MapOrderToDTO(existingOrder);
        }

        public bool DeleteOrder(int idTmp)
        {
            var existingOrder = _orderRepository.Get(idTmp);
            if (existingOrder == null) return false;

            _orderRepository.Delete(existingOrder);
            _orderRepository.Save();

            return true;
        }

        public List<OrderDTO> GetAll()
        {
            var orders = _orderRepository
                .Get()
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Flight)  // Include để tải dữ liệu từ Flight
                .ToList();

            return orders.Select(order => MapOrderToDTO(order)).ToList();
        }


        public OrderDTO GetById(int idTmp)
        {
            var order = _orderRepository
                .Get()
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Flight)  // Include để tải dữ liệu từ Flight
                .FirstOrDefault(o => o.OrderId == idTmp);

            return order == null ? null : MapOrderToDTO(order);
        }


        public List<OrderDTO> GetOrderByUserId(int id)
        {
            var orderReturn = _orderRepository
                .Get()
                .Where(o => o.UserId == id)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Flight)
                .ToList();

            if (orderReturn == null || orderReturn.Count == 0)
                return null;

            List<OrderDTO> ordersList = new List<OrderDTO>();

            foreach (var order in orderReturn)
            {
                var firstOrderDetail = order.OrderDetails?.FirstOrDefault();
                if (firstOrderDetail == null || firstOrderDetail.Flight == null)
                {
                    continue;
                }

                var departureRoute = _flightRouteRepository.Get()
                    .FirstOrDefault(fr => fr.FlightRouteId == firstOrderDetail.Flight.DepartureLocation);
                var arrivalRoute = _flightRouteRepository.Get()
                    .FirstOrDefault(fr => fr.FlightRouteId == firstOrderDetail.Flight.ArrivalLocation);

                var orderDTO = new OrderDTO
                {
                    OrderId = order.OrderId,
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    TotalPrice = order.TotalPrice,
                    DepartureLocation = departureRoute?.Location ?? "Unknown",
                    ArrivalLocation = arrivalRoute?.Location ?? "Unknown",
                    DepartureTime = firstOrderDetail.Flight.DepartureTime,
                    ArrivalTime = firstOrderDetail.Flight.ArrivalTime,
                    OrderDetails = order.OrderDetails.Select(od => new OrderDetailDTO
                    {
                        OrderDetailId = od.OrderDetailId,
                        Name = od.Name,
                        DoB = od.DoB,
                        Nationality = od.Nationality,
                        Email = od.Email,
                        FlightId = od.FlightId,
                        TripType = od.TripType,
                        SeatId = od.SeatId,
                        Status = od.Status,
                        TotalAmount = od.TotalAmount
                    }).ToList()
                };

                ordersList.Add(orderDTO);
            }

            return ordersList;
        }



        // Phương thức ánh xạ đơn hàng và lấy thông tin từ FlightRoute
        private OrderDTO MapOrderToDTO(Order order)
        {
            if (order == null)
                return null;

            var firstOrderDetail = order.OrderDetails?.FirstOrDefault();
            if (firstOrderDetail == null || firstOrderDetail.Flight == null)
            {
                // Trả về null hoặc giá trị mặc định thay vì ném ngoại lệ
                return new OrderDTO
                {
                    OrderId = order.OrderId,
                    UserId = order.UserId,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    TotalPrice = order.TotalPrice,
                    DepartureLocation = "Unknown",
                    ArrivalLocation = "Unknown",
                    OrderDetails = new List<OrderDetailDTO>()
                };
            }

            // Lấy dữ liệu departureLocation và arrivalLocation từ FlightRoute
            var departureRoute = _flightRouteRepository.Get()
                .FirstOrDefault(fr => fr.FlightRouteId == firstOrderDetail.Flight.DepartureLocation);
            var arrivalRoute = _flightRouteRepository.Get()
                .FirstOrDefault(fr => fr.FlightRouteId == firstOrderDetail.Flight.ArrivalLocation);

            var orderDTO = new OrderDTO
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                DepartureLocation = departureRoute?.Location ?? "Unknown",
                ArrivalLocation = arrivalRoute?.Location ?? "Unknown",
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailDTO
                {
                    OrderDetailId = od.OrderDetailId,
                    Name = od.Name,
                    DoB = od.DoB,
                    Nationality = od.Nationality,
                    Email = od.Email,
                    FlightId = od.FlightId,
                    TripType = od.TripType,
                    SeatId = od.SeatId,
                    Status = od.Status,
                    TotalAmount = od.TotalAmount
                }).ToList()
            };

            return orderDTO;
        }


    }



}
