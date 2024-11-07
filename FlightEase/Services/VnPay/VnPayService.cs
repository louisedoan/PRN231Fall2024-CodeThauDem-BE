using BusinessObjects.DTOs;
using BusinessObjects.Entities;
using BusinessObjects.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Repositories.Repositories;
using Services.EmailService;
using System.Text.RegularExpressions;

namespace Services.VnPay
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentResponse(IQueryCollection collection);
    }

    public class VnPayService : IVnPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public VnPayService(IConfiguration configuration, IPaymentRepository paymentRepository, IOrderDetailRepository orderDetailRepository, IOrderRepository orderRepository, IUserRepository userRepository, IEmailService emailService)
        {
            _configuration = configuration;
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
        {
            var tick = DateTime.Now.Ticks.ToString();

            var vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", _configuration["VnPay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", ((int)(model.Amount * 100)).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Order ID:" + model.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "billpayment");
            vnpay.AddRequestData("vnp_ReturnUrl", _configuration["VnPay:ReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", tick);
            vnpay.AddRequestData("vnp_BankCode", "NCB");

            var paymentUrl = vnpay.CreateRequestUrl(_configuration["VnPay:BaseUrl"], _configuration["VnPay:HashSecret"]);

            return paymentUrl;
        }

        public VnPaymentResponseModel PaymentResponse(IQueryCollection collection)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collection)
            {
                if (!string.IsNullOrEmpty(value) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, value.ToString());
                }
            }

            VnPaymentResponseModel response = new VnPaymentResponseModel();

            var vnp_orderId = vnpay.GetResponseData("vnp_TxnRef");
            var vnp_SecureHash = collection.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _configuration["VnPay:HashSecret"]);
            if (!checkSignature)
            {
                return new VnPaymentResponseModel
                {
                    Status = false,
                    Message = "Invalid Signature"
                };
            }

            if (vnp_ResponseCode == "00")
            {
                
                var orderId = ExtractOrderId(vnp_OrderInfo);
                var order = _orderRepository.FirstOrDefault(o => o.OrderId == orderId);
                var user = _userRepository.FirstOrDefault(u => u.UserId == order.UserId);
                var email = user.Email;

                _emailService.SendEmailAsync(email, "Payment Success", $"Dear User,\n\nYour payment with order number {orderId} has been successfully processed.\nThank you for choosing FlightEase!\n\nBest Regards,\nFlightEase Team");

                UpdatePayment(orderId);
                return new VnPaymentResponseModel
                {
                    Status = true,
                    TransactionId = vnp_orderId,
                    Description = vnp_OrderInfo,
                    ResponseCode = vnp_ResponseCode,
                    Message = "Purchase successfully"
                };
            }
            else
            {
                return new VnPaymentResponseModel
                {
                    Status = false,
                    Message = $"Payment failed, mã lỗi: {vnp_ResponseCode}"
                };
            }
        }
        private int ExtractOrderId(string description)
        {
            var match = Regex.Match(description, @"Order ID:(\d+)");
            if (match.Success && int.TryParse(match.Groups[1].Value, out int orderId))
            {
                return orderId;
            }
            throw new Exception("Invalid order ID in description");
        }
        public async Task<bool> UpdatePayment(int orderId)
        {
            try
            {
                var payment = _paymentRepository.FirstOrDefault(o => o.OrderId == orderId);
                if (payment == null) return false;

                // Cập nhật thông tin Payment
                payment.Status = OrderEnums.Success.ToString();
                payment.PaymentDate = DateTime.Now;
                payment.PaymentMethod = "VNPay";

                var order = _orderRepository.Get().FirstOrDefault(o => o.OrderId == orderId);
                if (order == null) return false;
                order.Status = OrderEnums.Success.ToString();

                var orderDetails = _orderDetailRepository.Get().Where(o => o.OrderId == orderId).ToList();
                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.Status = OrderEnums.Success.ToString();
                    _orderDetailRepository.Update(orderDetail);
                    _orderDetailRepository.Save();
                }
                _paymentRepository.Update(payment);
                _paymentRepository.Save();
                _orderRepository.Update(order);
                _orderRepository.Save();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public class VnPaymentResponseModel
    {
        public bool Status { get; set; }
        public string TransactionId { get; set; }
        public string Description { get; set; }
        public string ResponseCode { get; set; }
        public string Message { get; set; }
    }
}
