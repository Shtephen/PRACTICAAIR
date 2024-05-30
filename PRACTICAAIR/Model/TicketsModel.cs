using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRACTICAAIR.Model
{
    public class TicketsModel
    {
        [Key]
        public int IdTickets { get; set; }
        public string SeatNumber { get; set; }
        public string ClassService { get; set; }
        public string TicketStatus { get; set; }
        public decimal TicketPrice { get; set; }
        public int IdFlights { get; set; }
    }
}
