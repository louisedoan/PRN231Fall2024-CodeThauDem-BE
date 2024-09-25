using BusinessObjects.DTOs;
using Repositories.Repositories;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IOrderDetailService {
        public OrderDetailDTO CreateOrderDetail(OrderDetailDTO orderdetailCreate);
        public OrderDetailDTO UpdateOrderDetail(OrderDetailDTO orderdetailUpdate);
        public bool DeleteOrderDetail(int idTmp);
        public List<OrderDetailDTO> GetAll();
        public OrderDetailDTO GetById(int idTmp);
    }

    public class OrderDetailService : IOrderDetailService {

      private readonly IOrderDetailRepository _orderdetailRepository;

        public OrderDetailService(IOrderDetailRepository orderdetailRepository)
        {
            _orderdetailRepository = orderdetailRepository;
        }

        public OrderDetailDTO CreateOrderDetail(OrderDetailDTO orderdetailCreate)
        {
            throw new NotImplementedException();
        }

        public OrderDetailDTO UpdateOrderDetail(OrderDetailDTO orderdetailUpdate) 
        {
            throw new NotImplementedException();
        }

        public bool DeleteOrderDetail(int idTmp)
        {
            throw new NotImplementedException();
        }

        public List<OrderDetailDTO> GetAll() 
        {
            throw new NotImplementedException();
        }

        public OrderDetailDTO GetById(int idTmp) 
        {
            throw new NotImplementedException();
        }

    }

}
