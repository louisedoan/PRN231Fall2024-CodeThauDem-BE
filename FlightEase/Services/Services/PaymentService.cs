using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using Repositories.Repositories;
using Services.VnPay;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IPaymentService
    {
        public PaymentDTO CreatePayment(PaymentDTO paymentCreate, HttpContext context);  // Thêm HttpContext
        public PaymentDTO UpdatePayment(PaymentDTO paymentUpdate);
        public bool DeletePayment(int idTmp);
        public List<PaymentDTO> GetAll();
        public PaymentDTO GetById(int idTmp);
        public bool ProcessVnPayCallBack(IQueryCollection collection);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IVnPayService _vnPayService;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository, IVnPayService vnPayService)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _vnPayService = vnPayService; // Khởi tạo VnPayService
        }

        // Tạo Payment mới từ thông tin Order
        public PaymentDTO CreatePayment(PaymentDTO paymentCreate, HttpContext context)
        {
            // Lấy Order từ database
            var order = _orderRepository.Get(paymentCreate.OrderId.Value);
            if (order == null)
            {
                throw new Exception("Order not found");
            }

            // Gán tổng giá trị của Order (TotalPrice) cho số tiền thanh toán
            var payment = new Payment
            {
                OrderId = paymentCreate.OrderId,
                PaymentDate = DateTime.Now,
                PaymentMethod = paymentCreate.PaymentMethod,
                Status = "Pending",
                Amount = order.TotalPrice // Lấy TotalPrice của đơn hàng để làm số tiền thanh toán
            };

            // Tạo Payment mới
            var createdPayment = _paymentRepository.Create(payment);
            _paymentRepository.Save();

            // Sử dụng VnPayService để tạo URL thanh toán
            var vnPayRequestModel = new VnPaymentRequestModel
            {
                Amount = (decimal)payment.Amount,  // Không cần ép kiểu
                ServiceId = payment.OrderId.Value
            };

            var paymentUrl = _vnPayService.CreatePaymentUrl(context, vnPayRequestModel);

            if (string.IsNullOrEmpty(paymentUrl))
            {
                throw new Exception("Payment URL not generated");
            }

            // Trả về DTO bao gồm URL thanh toán
            return new PaymentDTO
            {
                PaymentId = createdPayment.PaymentId,
                OrderId = createdPayment.OrderId,
                PaymentDate = createdPayment.PaymentDate,
                Status = createdPayment.Status,
                Amount = createdPayment.Amount,
                PaymentUrl = paymentUrl // Thêm URL thanh toán vào DTO
            };
        }



        // Cập nhật thông tin Payment
        public PaymentDTO UpdatePayment(PaymentDTO paymentUpdate)
        {
            var payment = _paymentRepository.Get(paymentUpdate.PaymentId);
            if (payment == null) throw new Exception("Payment not found");

            // Cập nhật thông tin Payment
            payment.PaymentMethod = paymentUpdate.PaymentMethod;
            payment.Status = paymentUpdate.Status;
            payment.Amount = paymentUpdate.Amount;
            payment.PaymentDate = paymentUpdate.PaymentDate;

            _paymentRepository.Update(payment);
            _paymentRepository.Save();

            return paymentUpdate;
        }

        // Xóa Payment theo ID
        public bool DeletePayment(int idTmp)
        {
            var payment = _paymentRepository.Get(idTmp);
            if (payment == null) return false;

            _paymentRepository.Delete(payment);
            _paymentRepository.Save();
            return true;
        }

        // Lấy tất cả Payment
        public List<PaymentDTO> GetAll()
        {
            var payments = _paymentRepository.Get().ToList();
            return payments.Select(p => new PaymentDTO
            {
                PaymentId = p.PaymentId,
                OrderId = p.OrderId,
                PaymentDate = p.PaymentDate,
                Status = p.Status,
                Amount = p.Amount
            }).ToList();
        }

        // Lấy thông tin Payment theo ID
        public PaymentDTO GetById(int idTmp)
        {
            var payment = _paymentRepository.Get(idTmp);
            if (payment == null) return null;

            return new PaymentDTO
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                PaymentDate = payment.PaymentDate,
                Status = payment.Status,
                Amount = payment.Amount
            };
        }

        // Hàm xử lý phản hồi từ VNPay sau khi người dùng thanh toán
        public bool ProcessVnPayCallBack(IQueryCollection collection)
        {
            string txnRefString = collection["vnp_TxnRef"].ToString();
            string statusCode = collection["vnp_ResponseCode"].ToString();

            if (!int.TryParse(txnRefString, out int vnPayTransactionId))
            {
                // Xử lý khi txnRefString không phải số hợp lệ
                return false;
            }

            var payment = _paymentRepository.FirstOrDefault(p => p.PaymentId == vnPayTransactionId);
            if (payment == null) return false;

            payment.Status = statusCode == "00" ? "Success" : "Failed";
            _paymentRepository.Update(payment);
            _paymentRepository.Save();

            return payment.Status == "Success";
        }


    }

}
