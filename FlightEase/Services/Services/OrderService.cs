using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
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

        public async Task<ResultModel> CreateOrder(OrderCreateDTO orderCreate)
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
                            StatusCode = 400,
                            Data = null
                        };
                    }

                    if (passenger.DoB < new DateTime(1950, 1, 1) || passenger.DoB > new DateTime(2024, 12, 31))
                    {
                        return new ResultModel
                        {
                            IsSuccess = false,
                            Message = "DoB must be between 1/1/1950 and 31/12/2024",
                            StatusCode = 400,
                            Data = null
                        };
                    }

                    if (passenger.Price <= 0)
                    {
                        return new ResultModel
                        {
                            IsSuccess = false,
                            Message = "Price must be greater than 0",
                            StatusCode = 400,
                            Data = null
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
            var existingOrder = _orderRepository.Get(orderUpdate.OrderId);
            if (existingOrder == null) return null;

            existingOrder.UserId = orderUpdate.UserId;
            existingOrder.OrderDate = orderUpdate.OrderDate;
            existingOrder.Status = orderUpdate.Status;
            existingOrder.TotalPrice = orderUpdate.TotalPrice;

            _orderRepository.Update(existingOrder);
            _orderRepository.Save();

            return MapOrderToDTO(existingOrder, _flightRepository);
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
            var orders = _orderRepository.Get().ToList();
            return orders.Select(order => MapOrderToDTO(order, _flightRepository)).ToList();
        }

        public OrderDTO GetById(int idTmp)
        {
            var order = _orderRepository.Get(idTmp);
            return order == null ? null : MapOrderToDTO(order, _flightRepository);
        }

        // Cập nhật phương thức MapOrderToDTO thành static và chuyển _flightRepository thành tham số
        private static OrderDTO MapOrderToDTO(Order order, IFlightRepository flightRepository)
        {
            var orderDTO = new OrderDTO
            {
                OrderId = order.OrderId,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalPrice = order.TotalPrice
            };

            var flight = flightRepository.GetFlightWithLocations(f => f.OrderDetails.Any(od => od.OrderId == order.OrderId));

            if (flight != null)
            {
                orderDTO.DepartureLocation = flight.DepartureLocationNavigation?.Location;
                orderDTO.ArrivalLocation = flight.ArrivalLocationNavigation?.Location;
            }

            return orderDTO;
        }
    }

}
