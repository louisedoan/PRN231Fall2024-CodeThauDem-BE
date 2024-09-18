using BusinessObjects.DTOs;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/orders")]
    public class OrderController : ControllerBase {

        private IOrderService _orderService;

         public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [MapToApiVersion("1")]
        [HttpPost]
        public ActionResult<OrderDTO> CreateOrder(OrderDTO orderCreate)
        {
            var orderCreated = _orderService.CreateOrder(orderCreate);

            if (orderCreated == null)
            {
                return NotFound("");
            }
            return orderCreated;
        }

        [MapToApiVersion("1")]
        [HttpGet]
        public ActionResult<List<OrderDTO>> GetAll()
        {
            var orderList = _orderService.GetAll();

            if (orderList == null)
            {
                return NotFound("");
            }
            return orderList;
        }

        [MapToApiVersion("1")]
        [HttpGet("idTmp")]
        public ActionResult<OrderDTO> GetById(int idTmp)
        {
            var orderDetail = _orderService.GetById(idTmp);

            if (orderDetail == null)
            {
                return NotFound("");
            }
            return orderDetail;
        }

        [MapToApiVersion("1")]
        [HttpDelete]
        public ActionResult<bool> DeleteOrder(int idTmp)
        {
            var check = _orderService.DeleteOrder(idTmp);

            if (check == false)
            {
                return NotFound("");
            }
            return check;
        }

        [MapToApiVersion("1")]
        [HttpPut]
        public ActionResult<OrderDTO> UpdateOrder(OrderDTO orderCreate)
        {
            var orderUpdated = _orderService.UpdateOrder(orderCreate);

            if (orderUpdated == null)
            {
                return NotFound("");
            }
            return orderUpdated;
        }
    }

}
