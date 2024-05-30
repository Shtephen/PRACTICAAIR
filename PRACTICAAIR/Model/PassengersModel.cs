using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRACTICAAIR.Model
{
    public class PassengersModel
    {
        [Key]
        public int IdPassengers { get; set; }
        public string Gender { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string? Patronumic { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PassportData { get; set; }
        public string Phone {  get; set; }
        public string Email { get; set; }
        public int IdTickets { get; set; }
    }
}
