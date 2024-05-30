using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRACTICAAIR.Model
{
    public class FlightsModel
    {
        [Key]
        public int IdFlights { get; set; }
        public string Carrier { get; set; }
        public string NamePlane { get; set; }
        public string FlightStatus { get; set; }
        public DateTime DepartureDate { get; set; }
        public TimeSpan DepartureTime { get; set; }
        public string DepartureCity { get; set; }
        public string DepartureAirport { get; set; }
        public string? IntermediateLanding { get; set; }
        public TimeSpan ArrivalTime { get; set; }
        public string ArrivalCity { get; set; }
        public string ArrivalAirport { get; set; }
        public int? EconomyClassSeats { get; set; }
        public int? ComfortClassSeats { get; set; }
        public int? BusinessClassSeats { get; set; }
        public decimal? EconomyPrice { get; set; }
        public decimal? ComfortPrice { get; set; }
        public decimal? BusinessPrice { get; set; }
    }
    
}
