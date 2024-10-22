namespace BusinessObjects.DTOs
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int? OrderId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }
        public double? Amount { get; set; }
        public string? PaymentUrl { get; set; } // Thêm trường URL thanh toán
    }

}
