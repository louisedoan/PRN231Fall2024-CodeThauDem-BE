namespace BusinessObjects.DTOs
{
    public class FlightDTO
    {
        public int FlightId { get; set; }

        public int? PilotId { get; set; }

        public int? FlightNumber { get; set; }

        public int? DepartureLocation { get; set; } // Lưu ID của địa điểm khởi hành

        public string? DepartureLocationName { get; set; } // Lưu tên địa điểm khởi hành

        public DateTime? DepartureTime { get; set; }

        public int? ArrivalLocation { get; set; } // Lưu ID của địa điểm đến

        public string? ArrivalLocationName { get; set; } // Lưu tên địa điểm đến

        public DateTime? ArrivalTime { get; set; }

        public string? FlightStatus { get; set; }

        public int? EmptySeat { get; set; }
    }
}
