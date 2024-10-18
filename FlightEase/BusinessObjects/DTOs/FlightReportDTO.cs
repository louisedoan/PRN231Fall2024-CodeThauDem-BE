using BusinessObjects.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.DTOs
{
    public class FlightReportDTO
    {
        public int OrderId { get; set; }  
        public string PaymentStatus { get; set; }  
        public string BookingStatus { get; set; }  

        // Flight details
        public int FlightId { get; set; }
        public int? PlaneId { get; set; }
        public string? PlaneCode { get; set; }
        public int? FlightNumber { get; set; }

        public int? DepartureLocation { get; set; }
        public string? DepartureLocationName { get; set; }
        public DateTime? DepartureTime { get; set; }

        public int? ArrivalLocation { get; set; }
        public string? ArrivalLocationName { get; set; }
        public DateTime? ArrivalTime { get; set; }

        public string FlightStatus { get; set; } 

        public int AvailableBusinessSeats { get; set; }
        public int AvailableEconomySeats { get; set; }

        public DateTime BookingDate { get; set; }  
        public DateTime? CancellationDate { get; set; }  

    }
}
