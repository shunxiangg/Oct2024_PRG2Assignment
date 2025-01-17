using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10270399_PRG2Assignment
{
    class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();

        public void AddFlight(Flight flight)
        {
            Flights.Add(flight.FlightNumber, flight);
        }

        public void CalculateFees()
        {
            //Calculate fees
        }

        public void RemoveFlight(Flight flight)
        {
            Flights.Remove(flight.FlightNumber);
        }

        public override string ToString()
        {
            return Name;
        }




    }
}
