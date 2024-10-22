using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

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

        public VnPayService(IConfiguration configuration)
        {
            _configuration = configuration;
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
            vnpay.AddRequestData("vnp_OrderInfo", "Order ID:" + model.ServiceId);
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

            // Debug để kiểm tra giá trị của Secure Hash và Response Code
            Console.WriteLine($"vnp_orderId: {vnp_orderId}");
            Console.WriteLine($"vnp_SecureHash: {vnp_SecureHash}");
            Console.WriteLine($"vnp_ResponseCode: {vnp_ResponseCode}");

            // Kiểm tra tính hợp lệ của chữ ký bảo mật
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _configuration["VnPay:HashSecret"]);
            if (!checkSignature)
            {
                return new VnPaymentResponseModel
                {
                    Status = false,
                    Message = "Sai chữ ký bảo mật"
                };
            }

            // Kiểm tra mã phản hồi từ VNPay
            if (vnp_ResponseCode == "00") // "00" là mã phản hồi thanh toán thành công
            {
                return new VnPaymentResponseModel
                {
                    Status = true,
                    TransactionId = vnp_orderId,
                    Description = vnp_OrderInfo,
                    ResponseCode = vnp_ResponseCode,
                    Message = "Thanh toán thành công"
                };
            }
            else
            {
                return new VnPaymentResponseModel
                {
                    Status = false,
                    Message = $"Thanh toán thất bại, mã lỗi: {vnp_ResponseCode}"
                };
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
