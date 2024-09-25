using BusinessObjects.DTOs;
using Repositories.Repositories;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IOrderService {
        public OrderDTO CreateOrder(OrderDTO orderCreate);
        public OrderDTO UpdateOrder(OrderDTO orderUpdate);
        public bool DeleteOrder(int idTmp);
        public List<OrderDTO> GetAll();
        public OrderDTO GetById(int idTmp);
    }

    public class OrderService : IOrderService {

      private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public OrderDTO CreateOrder(OrderDTO orderCreate)
        {
            throw new NotImplementedException();
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
