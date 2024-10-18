using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.DTOs
{
    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        public string Name { get; set; }
        public DateTime? DoB { get; set; }
        [Required]
        public string? Nationality { get; set; }

        [Required]
        public string? Email { get; set; }
        [Required]
        public int? FlightId { get; set; }
        [Required]
        public string TripType { get; set; }
        [Required]
        public int? SeatId { get; set; }
        public string Status { get; set; }
        public double? TotalAmount { get; set; }
    }
    public class PassengerDTO
    {
        [Required]
        public string Name { get; set; }
        public DateTime DoB { get; set; }
        [Required]
        public string Nationality { get; set; }
        [Required]
        public string Email { get; set; }
        public int? FlightId { get; set; }
        [Required]
        public string TripType { get; set; }
        [Required]
        public int? SeatId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be greater than 0")]
        public double Price { get; set; }
    }
}
