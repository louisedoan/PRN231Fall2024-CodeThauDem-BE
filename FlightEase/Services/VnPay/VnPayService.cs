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
            vnpay.AddRequestData("vnp_TmnCode", _configuration["VNPAY:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", ((int)(model.Amount * 100)).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Purchase service:" + model.ServiceId);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", _configuration["VNPAY:ReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", tick);
            vnpay.AddRequestData("vnp_BankCode", "NCB");

            var paymentUrl = vnpay.CreateRequestUrl(_configuration["VNPAY:BaseUrl"], _configuration["VNPAY:HashSecret"]);

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

            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _configuration["VNPAY:HashSecret"]);
            if (!checkSignature)
            {
                return new VnPaymentResponseModel
                {
                    Status = false
                };
            }
            return new VnPaymentResponseModel
            {
                Status = true,
                TransactionId = vnp_orderId.ToString(),
                Description = vnp_OrderInfo,
                ResponseCode = vnp_ResponseCode
            };
        }
    }
}
