﻿using BusinessObjects.DTOs;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // Tạo mới đơn hàng
        [MapToApiVersion("1")]
        [HttpPost]
        public async Task<ActionResult<ResultModel>> CreateOrder(OrderCreateDTO orderCreate)
        {
            var result = await _orderService.CreateOrder(orderCreate);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        // Lấy tất cả các đơn hàng
        [MapToApiVersion("1")]
        [HttpGet]
        public ActionResult GetAll()
        {
            try
            {
                var orderList = _orderService.GetAll();

                if (orderList == null || !orderList.Any())
                {
                    return NotFound(new { message = "No orders found." });
                }

                return Ok(new { message = "Orders retrieved successfully.", orders = orderList });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving orders.", error = ex.Message });
            }
        }

        // Lấy đơn hàng theo ID
        [MapToApiVersion("1")]
        [HttpGet("{idTmp}")]
        public ActionResult GetById(int idTmp)
        {
            try
            {
                var orderDetail = _orderService.GetById(idTmp);

                if (orderDetail == null)
                {
                    return NotFound(new { message = $"Order with ID {idTmp} not found." });
                }

                return Ok(new { message = "Order retrieved successfully.", order = orderDetail });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the order.", error = ex.Message });
            }
        }

        // Xóa đơn hàng theo ID
        [MapToApiVersion("1")]
        [HttpDelete("{idTmp}")]
        public ActionResult DeleteOrder(int idTmp)
        {
            try
            {
                var result = _orderService.DeleteOrder(idTmp);

                if (!result)
                {
                    return NotFound(new { message = $"Order with ID {idTmp} could not be deleted or does not exist." });
                }

                return Ok(new { message = "Order deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the order.", error = ex.Message });
            }
        }

        // Cập nhật đơn hàng
        [MapToApiVersion("1")]
        [HttpPut]
        public ActionResult UpdateOrder(OrderDTO orderUpdate)
        {
            try
            {
                var orderUpdated = _orderService.UpdateOrder(orderUpdate);

                if (orderUpdated == null)
                {
                    return NotFound(new { message = $"Order with ID {orderUpdate.OrderId} could not be updated or does not exist." });
                }

                return Ok(new { message = "Order updated successfully.", order = orderUpdated });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the order.", error = ex.Message });
            }
        }
        [MapToApiVersion("1")]
        [HttpGet("userId={id}")]
        public ActionResult GetOrderByUserId(int id)
        {
            try
            {
                var orders = _orderService.GetOrderByUserId(id);

                if (orders == null)
                {
                    return NotFound(new { message = $"Order with ID {id} not found." });
                }

                return Ok(new { message = "Order retrieved successfully.", order = orders });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the order.", error = ex.Message });
            }
        }
        

        
    }


}