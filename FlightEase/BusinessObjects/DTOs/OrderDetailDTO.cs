using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.DTOs
{
    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        public string Name { get; set; }
        [Range(typeof(DateTime), "1950-01-01", "2024-12-31", ErrorMessage = "DoB must be between 1950 and 2024")]
        public DateTime? DoB { get; set; }
        public string? Nationality { get; set; }
        public string? Email { get; set; }
        public int? FlightId { get; set; }
        public string TripType { get; set; }
        public int? SeatId { get; set; }
        public string Status { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be greater than 0")]
        public double? TotalAmount { get; set; }
    }
    public class PassengerDTO
    {
        public string Name { get; set; }

        [Range(typeof(DateTime), "1950-01-01", "2024-12-31", ErrorMessage = "DoB must be between 1950 and 2024")]
        public DateTime DoB { get; set; }
        public string Nationality { get; set; }
        public string Email { get; set; }
        public int? FlightId { get; set; }
        public string TripType { get; set; }
        public int? SeatId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be greater than 0")]
        public double Price { get; set; }
    }
}
