using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using Microsoft.AspNetCore.Http;
using Repositories.Repositories;
using Services.EmailService;
using Services.VnPay;

namespace FlightEaseDB.BusinessLogic.Services
{

    public interface IPaymentService
    {
        public PaymentDTO CreatePayment(PaymentDTO paymentCreate, HttpContext context);  // Thêm HttpContext
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
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMembershipService _membershipService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository, IVnPayService vnPayService, IOrderDetailRepository orderDetailRepository, IMembershipService membershipService, IUserService userService, IEmailService emailService)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _vnPayService = vnPayService;
            _orderDetailRepository = orderDetailRepository;
            _membershipService = membershipService;
            _userService = userService;
            _emailService = emailService;
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
                PaymentDate = null,
                Status = "Pending",
                Amount = order.TotalPrice // Lấy TotalPrice của đơn hàng để làm số tiền thanh toán
            };

            var createdPayment = _paymentRepository.Create(payment);
            _paymentRepository.Save();

            // Sử dụng VnPayService để tạo URL thanh toán
            var vnPayRequestModel = new VnPaymentRequestModel
            {
                Amount = (decimal)payment.Amount,  // Không cần ép kiểu
                ServiceId = payment.OrderId.Value,
                OrderId = payment.OrderId.Value
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
                return false;
            }

            var payment = _paymentRepository.FirstOrDefault(p => p.PaymentId == vnPayTransactionId);
            if (payment == null) return false;

            payment.Status = statusCode == "00" ? "Success" : "Failed";
            payment.PaymentDate = DateTime.Now;
            _paymentRepository.Update(payment);
            _paymentRepository.Save();

            if (payment.Status == "Success")
            {
                // Cập nhật Rank cho User khi thanh toán thành công
                var order = _orderRepository.Get(payment.OrderId.Value);
                if (order != null)
                {
                    _userService.UpdateUserRank(order.UserId.Value).Wait(); // Gọi hàm UpdateUserRank để cập nhật Rank

                }
            }

            return payment.Status == "Success";
        }
    }
}
