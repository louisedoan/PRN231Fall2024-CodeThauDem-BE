using BusinessObjects.DTOs;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Services.VnPay;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/payments")]
    public class PaymentController : ControllerBase
    {
        private IPaymentService _paymentService;
        private readonly IVnPayService _vnPayService;

        public PaymentController(IPaymentService paymentService, IVnPayService vnPayService)
        {
            _paymentService = paymentService;
            _vnPayService = vnPayService;
        }

        // Tạo thanh toán mới
        [MapToApiVersion("1")]
        [HttpPost]
        public ActionResult<PaymentDTO> CreatePayment([FromBody] PaymentDTO paymentCreate)
        {
            try
            {
                var paymentCreated = _paymentService.CreatePayment(paymentCreate, HttpContext);
                return Ok(paymentCreated);
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
        //[MapToApiVersion("1")]
        //[HttpPut]
        //public ActionResult<PaymentDTO> UpdatePayment([FromBody] PaymentDTO paymentUpdate)
        //{
        //    try
        //    {
        //        var paymentUpdated = _paymentService.UpdatePayment(paymentUpdate);
        //        return Ok(paymentUpdated);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet("vnpay_return")]
        public IActionResult VnPayReturn()
        {
            try
            {
                // Process the payment response using VnPayService
                var paymentResult = _vnPayService.PaymentResponse(HttpContext.Request.Query);

                // Check if the payment was successful
                if (paymentResult.ResponseCode == "00")
                {
                    return Ok(paymentResult);

                }
                else
                {
                    return BadRequest(new { status = "fail", message = "Payment failed" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = "fail", message = ex.Message });
            }
        }


    }


}
