namespace BusinessObjects.DTOs
{
    public class FlightDTO
    {
        public int FlightId { get; set; }

        public int? PilotId { get; set; }

        public int? FlightNumber { get; set; }

        public int? DepartureLocation { get; set; } 
        public string? DepartureLocationName { get; set; } 

        public DateTime? DepartureTime { get; set; }

        public int? ArrivalLocation { get; set; } 

        public string? ArrivalLocationName { get; set; } 

        public DateTime? ArrivalTime { get; set; }

        public string? FlightStatus { get; set; }

        public int? EmptySeat { get; set; }

    }
}
