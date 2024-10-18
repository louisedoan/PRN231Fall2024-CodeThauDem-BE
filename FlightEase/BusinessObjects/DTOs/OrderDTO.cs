using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? Status { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Total price must be greater than 0")]
        public double? TotalPrice { get; set; }
        public List<OrderDetailDTO> OrderDetails { get; set; }
        public List<PassengerDTO> Passengers { get; set; }
    }
}
