using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Repositories.Repositories;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IOrderService
    {
        public OrderDTO CreateOrder(OrderDTO orderCreate);
        public OrderDTO UpdateOrder(OrderDTO orderUpdate);
        public bool DeleteOrder(int idTmp);
        public List<OrderDTO> GetAll();
        public OrderDTO GetById(int idTmp);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IFlightRepository _flightRepository;

        public OrderService(IOrderRepository orderRepository, IFlightRepository flightRepository)
        {
            _orderRepository = orderRepository;
            _flightRepository = flightRepository;
        }

        public OrderDTO CreateOrder(OrderDTO orderCreate)
        {
            var orderEntity = new Order
            {
                UserId = orderCreate.UserId,
                OrderDate = orderCreate.OrderDate,
                TripType = orderCreate.TripType,
                Status = orderCreate.Status,
                TotalPrice = orderCreate.TotalPrice,
                /*OrderDetails = orderCreate.OrderDetails.Select(od => new OrderDetail
                {
                    FlightId = od.FlightId,
                    Price = od.Price,
                    Quantity = od.Quantity
                }).ToList()*/
            };

            _orderRepository.Create(orderEntity);
            _orderRepository.Save();

            return MapOrderToDTO(orderEntity, _flightRepository);
        }

        public OrderDTO UpdateOrder(OrderDTO orderUpdate)
        {
            var existingOrder = _orderRepository.Get(orderUpdate.OrderId);
            if (existingOrder == null) return null;

            existingOrder.UserId = orderUpdate.UserId;
            existingOrder.OrderDate = orderUpdate.OrderDate;
            existingOrder.TripType = orderUpdate.TripType;
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
                TripType = order.TripType,
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
