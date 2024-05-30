using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PRACTICAAIR.Model
{
    public class DBContextAir : DbContext
    {
        public DbSet <RegictrationModel> Authorization { get; set; }
        public DbSet <FlightsModel> Flights { get; set; }
        public DbSet<TicketsModel> Tickets { get; set; }
        public DbSet<PassengersModel> Passengers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-ANRB8JC;Initial Catalog=PRACTICAAIR;Integrated Security=True;Trust Server Certificate=True");
        }
    }
}
