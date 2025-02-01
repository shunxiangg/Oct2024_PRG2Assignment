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
    class Terminal
    {
        // Existing properties - unchanged
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; } = new Dictionary<string, Airline>();
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
        public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new Dictionary<string, BoardingGate>();
        public Dictionary<string, double> GateFees { get; set; } = new Dictionary<string, double>();

        // Fee constants for calculation
        private const double ARRIVING_FEE = 500;
        private const double DEPARTING_FEE = 800;
        private const double GATE_BASE_FEE = 300;
        private const double DDJB_REQUEST_FEE = 300;
        private const double CFFT_REQUEST_FEE = 150;
        private const double LWTT_REQUEST_FEE = 500;

        // Existing constructors - unchanged
        public Terminal() { }
        public Terminal(string terminalName)
        {
            TerminalName = terminalName;
        }

        // Fixed Airlines reference
        public void AddAirline(Airline airline)
        {
            Airlines.Add(airline.Code, airline);
        }

        // Fixed BoardingGates reference
        public void AddBoardingGate(BoardingGate boardingGate)
        {
            BoardingGates.Add(boardingGate.GateName, boardingGate);
        }

        // Fixed Airlines reference
        public void GetAirlineFromFlight(Flight flight)
        {
            foreach (var airline in Airlines)
            {
                if (airline.Value.Flights.ContainsKey(flight.FlightNumber))
                {
                    Console.WriteLine(airline.Value);
                }
            }
        }
        // Updated to use Airlines and calculate fees
        public void PrintAirlineFees()
        {
            foreach (var airline in Airlines)
            {
                double fees = CalculateAirlineFees(airline.Value);
                Console.WriteLine($"{airline.Value.Name} Total Fees: ${fees:F2}");
            }
        }

        // New method for fee calculation
        private double CalculateAirlineFees(Airline airline)
        {
            double totalFees = 0;

            foreach (var flight in airline.Flights.Values)
            {
                if (flight.Destination.Equals("SIN", StringComparison.OrdinalIgnoreCase))
                    totalFees += ARRIVING_FEE;
                if (flight.Origin.Equals("SIN", StringComparison.OrdinalIgnoreCase))
                    totalFees += DEPARTING_FEE;

                totalFees += GATE_BASE_FEE;

                if (flight is DDJBFlight)
                    totalFees += DDJB_REQUEST_FEE;
                else if (flight is CFFTFFlight)
                    totalFees += CFFT_REQUEST_FEE;
                else if (flight is LWTTFlight)
                    totalFees += LWTT_REQUEST_FEE;
            }

            return ApplyDiscounts(totalFees, airline);
        }

        private double ApplyDiscounts(double totalFees, Airline airline)
        {
            int totalFlights = airline.Flights.Count;

            totalFees -= (totalFlights / 3) * 350;

            if (totalFlights > 5)
                totalFees *= 0.97;

            if (airline.Flights.Values.Any(f => f.Expectedtime.Hour < 11 || f.Expectedtime.Hour >= 21))
                totalFees -= 110;

            if (airline.Flights.Values.Any(f =>
                new[] { "DXB", "BKK", "NRT" }.Contains(f.Origin.ToUpper())))
                totalFees -= 25;

            if (!airline.Flights.Values.Any(f =>
                f is DDJBFlight || f is CFFTFFlight || f is LWTTFlight))
                totalFees -= 50;

            return totalFees;
        }

        // Existing ToString - unchanged
        public override string ToString()
        {
            return "TerminalName: " + TerminalName;
        }
    }
}