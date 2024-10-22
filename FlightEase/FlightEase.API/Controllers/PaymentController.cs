using BusinessObjects.DTOs;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/payments")]
    public class PaymentController : ControllerBase
    {
        private IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // Tạo thanh toán mới
        [MapToApiVersion("1")]
        [HttpPost]
        public ActionResult<PaymentDTO> CreatePayment([FromBody] PaymentDTO paymentCreate)
        {
            try
            {
                var paymentCreated = _paymentService.CreatePayment(paymentCreate, HttpContext); // Truyền HttpContext để lấy IP và tạo URL VNPay
                return Ok(paymentCreated); // Trả về DTO bao gồm URL thanh toán
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Lấy tất cả thanh toán
        [MapToApiVersion("1")]
        [HttpGet]
        public ActionResult<List<PaymentDTO>> GetAll()
        {
            var paymentList = _paymentService.GetAll();
            return Ok(paymentList);
        }

        // Lấy thông tin thanh toán theo ID
        [MapToApiVersion("1")]
        [HttpGet("{idTmp}")]
        public ActionResult<PaymentDTO> GetById(int idTmp)
        {
            var paymentDetail = _paymentService.GetById(idTmp);
            if (paymentDetail == null)
            {
                return NotFound("Payment not found");
            }
            return Ok(paymentDetail);
        }

        // Xóa thanh toán
        [MapToApiVersion("1")]
        [HttpDelete("{idTmp}")]
        public ActionResult<bool> DeletePayment(int idTmp)
        {
            var check = _paymentService.DeletePayment(idTmp);
            if (!check)
            {
                return NotFound("Payment not found");
            }
            return Ok(true);
        }

        // Cập nhật thanh toán
        [MapToApiVersion("1")]
        [HttpPut]
        public ActionResult<PaymentDTO> UpdatePayment([FromBody] PaymentDTO paymentUpdate)
        {
            try
            {
                var paymentUpdated = _paymentService.UpdatePayment(paymentUpdate);
                return Ok(paymentUpdated);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Xử lý callback từ VNPay sau khi thanh toán
        [HttpGet("vnpay_return")]
        public IActionResult VnPayReturn(string txnRef, string responseCode, string secureHash)
        {
            try
            {
                // Gọi service để xử lý callback từ VNPay
                var paymentResult = _paymentService.ProcessVnPayCallBack(HttpContext.Request.Query);

                if (paymentResult)
                {
                    return Ok(new { status = "success", message = "Thanh toán thành công" });
                }
                else
                {
                    return BadRequest(new { status = "fail", message = "Thanh toán thất bại" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "fail", message = ex.Message });
            }
        }


    }


}
