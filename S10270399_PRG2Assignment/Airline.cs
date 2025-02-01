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
            Flights[flight.FlightNumber] = flight;
        }

        public double CalculateFees()
        {
            double totalFees = Flights.Values.Sum(flight => flight.CalculateFees());

            // Apply discounts
            int flightCount = Flights.Count;

            // Discount for every 3 flights
            double discount = Math.Floor(flightCount / 3.0) * 350.00;

            // Additional 3% discount for more than 5 flights
            if (flightCount > 5)
            {
                discount += totalFees * 0.03;
            }

            // Check for special origin cities discount
            foreach (var flight in Flights.Values)
            {
                if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" ||
                    flight.Origin == "Tokyo (NRT)")
                {
                    discount += 25.00;
                }

                // Check for off-peak timing discount
                var flightTime = flight.Expectedtime.TimeOfDay;
                if (flightTime < TimeSpan.FromHours(11) || flightTime > TimeSpan.FromHours(21))
                {
                    discount += 110.00;
                }

                // Check for no special request code discount
                if (flight is NORMFlight)
                {
                    discount += 50.00;
                }
            }

            return Math.Max(0, totalFees - discount);
        }

        public void RemoveFlight(Flight flight)
        {
            if (Flights.ContainsKey(flight.FlightNumber))
            {
                Flights.Remove(flight.FlightNumber);
            }
        }

            public override string ToString()
        {
            return $"Airline: {Name} ({Code}) - {Flights.Count} flights";
        }
    }
}
