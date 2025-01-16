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

        //public void AddAirline(Airline airline)
        //{
        //    Airline.Add(airline);
        //}
        //public void AddBoardingGate(BoardingGate boardingGate)
        //{
        //    BoardingGates.Add(boardingGate);
        //}
        //public void GetAirlineFromFlight(Flight flight)
        //{
        //    foreach (var airline in Airline)
        //    {
        //        if (airline.Value.Flights.Contains(flight))
        //        {
        //            Console.WriteLine(airline.Value);
        //        }
        //    }
        //}
        //public void PrintAirlineFees()
        //{
        //    foreach (var airline in Airline)
        //    {
        //        Console.WriteLine(airline.Value);
        //    }
        //}



        public void AddAirline(Airline airline)
        {
            if (!Airlines.ContainsKey(airline.Code))
            {
                Airlines.Add(airline.Code, airline);
            }
        }

        public void AddBoardingGate(BoardingGate boardingGate)
        {
            if (!BoardingGates.ContainsKey(boardingGate.GateName))
            {
                BoardingGates.Add(boardingGate.GateName, boardingGate);
            }
        }

        public Airline GetAirlineFromFlight(Flight flight)
        {
            return Airlines.Values.FirstOrDefault(airline =>
                airline.Flights.ContainsKey(flight.FlightNumber));
        }

        public void PrintAirlineFees()
        {
            Console.WriteLine($"Fee Report for Terminal {TerminalName}:");
            Console.WriteLine(new string('-', 50));

            foreach (var airline in Airlines.Values)
            {
                Console.WriteLine($"{airline.Name} ({airline.Code}): ${airline.CalculateFees():F2}");
            }

            Console.WriteLine(new string('-', 50));
            Console.WriteLine($"Total Terminal Fees: ${Airlines.Values.Sum(a => a.CalculateFees()):F2}");
        }

        public override string ToString()
        {
            return $"Terminal {TerminalName} - {Airlines.Count} Airlines, {Flights.Count} Flights, {BoardingGates.Count} Gates, {GateFees.Count} GatesFees";
        }
    }
}
