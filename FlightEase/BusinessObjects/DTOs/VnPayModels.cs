namespace BusinessObjects.DTOs
{
    internal class VnPayModels
    {
    }
    public class VnPaymentRequestModel
    {
        public int ServiceId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public int OrderId { get; set; }
    }
    public class VnPaymentResponseModel
    {
        public bool Status { get; set; }
        public string ResponseCode { get; set; }
        public string Description { get; set; }
        public string TransactionId { get; set; }
    }
}
