using BusinessObjects.DTOs;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/orderdetails")]
    public class OrderDetailController : ControllerBase {

        private IOrderDetailService _orderdetailService;

         public OrderDetailController(IOrderDetailService orderdetailService)
        {
            _orderdetailService = orderdetailService;
        }

        [MapToApiVersion("1")]
        [HttpPost]
        public ActionResult<OrderDetailDTO> CreateOrderDetail(OrderDetailDTO orderdetailCreate)
        {
            var orderdetailCreated = _orderdetailService.CreateOrderDetail(orderdetailCreate);

            if (orderdetailCreated == null)
            {
                return NotFound("");
            }
            return orderdetailCreated;
        }

        [MapToApiVersion("1")]
        [HttpGet]
        public ActionResult<List<OrderDetailDTO>> GetAll()
        {
            var orderdetailList = _orderdetailService.GetAll();

            if (orderdetailList == null)
            {
                return NotFound("");
            }
            return orderdetailList;
        }

        [MapToApiVersion("1")]
        [HttpGet("idTmp")]
        public ActionResult<OrderDetailDTO> GetById(int idTmp)
        {
            var orderdetailDetail = _orderdetailService.GetById(idTmp);

            if (orderdetailDetail == null)
            {
                return NotFound("");
            }
            return orderdetailDetail;
        }

        [MapToApiVersion("1")]
        [HttpDelete]
        public ActionResult<bool> DeleteOrderDetail(int idTmp)
        {
            var check = _orderdetailService.DeleteOrderDetail(idTmp);

            if (check == false)
            {
                return NotFound("");
            }
            return check;
        }

        [MapToApiVersion("1")]
        [HttpPut]
        public ActionResult<OrderDetailDTO> UpdateOrderDetail(OrderDetailDTO orderdetailCreate)
        {
            var orderdetailUpdated = _orderdetailService.UpdateOrderDetail(orderdetailCreate);

            if (orderdetailUpdated == null)
            {
                return NotFound("");
            }
            return orderdetailUpdated;
        }
        [HttpPut("cancel/{orderDetailId}")]
        public IActionResult CancelOrderDetail(int orderDetailId)
        {
            var result = _orderdetailService.CancelTicket(orderDetailId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

    }

}
