using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }
        public double? TotalPrice { get; set; }
        public string? DepartureLocation { get; set; } // Lấy từ Flight
        public string? ArrivalLocation { get; set; }   // Lấy từ Flight
        public List<OrderDetailDTO> OrderDetails { get; set; }
    }

    public class OrderCreateDTO
    {
        [Required]
        public int OrderId { get; set; }
        [Required]

        public int? UserId { get; set; }

        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }
        [Required]
        public double? TotalPrice { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
        public List<PassengerDTO> Passengers { get; set; }
    }
}
