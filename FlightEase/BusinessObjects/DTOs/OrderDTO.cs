namespace BusinessObjects.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? TripType { get; set; }
        public string? Status { get; set; }
        public double? TotalPrice { get; set; }
        public string? DepartureLocation { get; set; } // Lấy từ Flight
        public string? ArrivalLocation { get; set; }   // Lấy từ Flight
    }
}
