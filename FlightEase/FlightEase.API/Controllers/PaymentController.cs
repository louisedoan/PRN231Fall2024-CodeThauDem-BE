using BusinessObjects.DTOs;
using FlightEaseDB.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightEaseDB.Presentation.Controllers
{

    [ApiController]
    [ApiVersion("1")]
    [Route("/api/v1/payments")]
    public class PaymentController : ControllerBase {

        private IPaymentService _paymentService;

         public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [MapToApiVersion("1")]
        [HttpPost]
        public ActionResult<PaymentDTO> CreatePayment(PaymentDTO paymentCreate)
        {
            var paymentCreated = _paymentService.CreatePayment(paymentCreate);

            if (paymentCreated == null)
            {
                return NotFound("");
            }
            return paymentCreated;
        }

        [MapToApiVersion("1")]
        [HttpGet]
        public ActionResult<List<PaymentDTO>> GetAll()
        {
            var paymentList = _paymentService.GetAll();

            if (paymentList == null)
            {
                return NotFound("");
            }
            return paymentList;
        }

        [MapToApiVersion("1")]
        [HttpGet("idTmp")]
        public ActionResult<PaymentDTO> GetById(int idTmp)
        {
            var paymentDetail = _paymentService.GetById(idTmp);

            if (paymentDetail == null)
            {
                return NotFound("");
            }
            return paymentDetail;
        }

        [MapToApiVersion("1")]
        [HttpDelete]
        public ActionResult<bool> DeletePayment(int idTmp)
        {
            var check = _paymentService.DeletePayment(idTmp);

            if (check == false)
            {
                return NotFound("");
            }
            return check;
        }

        [MapToApiVersion("1")]
        [HttpPut]
        public ActionResult<PaymentDTO> UpdatePayment(PaymentDTO paymentCreate)
        {
            var paymentUpdated = _paymentService.UpdatePayment(paymentCreate);

            if (paymentUpdated == null)
            {
                return NotFound("");
            }
            return paymentUpdated;
        }
    }

}
