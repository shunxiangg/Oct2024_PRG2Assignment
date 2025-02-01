//==========================================================
// Student1 Number	: S10267626
// Student1 Name	: Aiden Tan Yihan
// Student2 Number  : S10270399
// Partner2 Name	: Ang Shun Xiang
//==========================================================
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

        public double CalculateFees()  // Changed return type from void to double
        {
            double totalFees = 0;
            foreach (var flight in Flights.Values)
            {
                totalFees += flight.CalculateFees();
            }
            return totalFees;
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