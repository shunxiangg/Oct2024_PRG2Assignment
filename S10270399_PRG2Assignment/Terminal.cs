using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10270399_PRG2Assignment
{
    class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; } = new Dictionary<string, Airline>();
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
        public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new Dictionary<string, BoardingGate>();
        public Dictionary<string, double> GateFees { get; set; } = new Dictionary<string, double>();

        public Terminal() { }
        public Terminal(string terminalName)
        {
            TerminalName = terminalName;
        }

        public void AddAirline(Airline airline)
        {
            Airline.Add(airline);
        }
        public void AddBoardingGate(BoardingGate boardingGate)
        {
            BoardingGates.Add(boardingGate);
        }
        public void GetAirlineFromFlight(Flight flight)
        {
            foreach (var airline in Airline)
            {
                if (airline.Value.Flights.Contains(flight))
                {
                    Console.WriteLine(airline.Value);
                }
            }
        }
        public void PrintAirlineFees()
        {
            foreach (var airline in Airline)
            {
                Console.WriteLine(airline.Value);
            }
        }




        public override string ToString()
        {
            return "TerminalName: " + TerminalName;
        }
    }
}
