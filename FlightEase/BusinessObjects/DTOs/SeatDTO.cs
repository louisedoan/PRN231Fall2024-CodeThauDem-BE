namespace BusinessObjects.DTOs
{
    public class SeatDTO
    {
        public int SeatId { get; set; }

        public int? SeatNumber { get; set; }

        public string? Class { get; set; }

        public double? Price { get; set; }
        public string? Status { get; set; }

        public int? PlaneId { get; set; }
    }
}
