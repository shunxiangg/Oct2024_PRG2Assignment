using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace S10270399_PRG2Assignment
{
    class Program
    {
        static Terminal terminal;

        static void Main(string[] args)
        {
            InitializeSystem();
            RunMainMenu();
        }

        static void InitializeSystem()
        {
            Console.WriteLine("Loading Airlines...");
            terminal = new Terminal("Terminal 5");
            LoadAirlines();
            Console.WriteLine($"{terminal.Airlines.Count} Airlines Loaded!\n");

            Console.WriteLine("Loading Boarding Gates...");
            LoadBoardingGates();
            Console.WriteLine($"{terminal.BoardingGates.Count} Boarding Gates Loaded!\n");

            Console.WriteLine("Loading Flights...");
            LoadFlights();
            Console.WriteLine($"{terminal.Flights.Count} Flights Loaded!\n");
        }

        static void LoadAirlines()
        {
            if (File.Exists("airlines.csv"))
            {
                string[] lines = File.ReadAllLines("airlines.csv");
                for (int i = 1; i < lines.Length; i++) // Skip header
                {
                    string[] data = lines[i].Split(',');
                    var airline = new Airline { Name = data[0].Trim(), Code = data[1].Trim() };
                    terminal.AddAirline(airline);
                }
            }
        }

        //static void LoadBoardingGates()


        static void LoadFlights()
        {
            if (File.Exists("flights.csv"))
            {
                string[] lines = File.ReadAllLines("flights.csv");
                for (int i = 1; i < lines.Length; i++) // Skip header
                {
                    try
                    {
                        ProcessFlightLine(lines[i]);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing flight on line {i}: {ex.Message}");
                    }
                }
            }
        }

        //static void ProcessFlightLine(string line)


        static void RunMainMenu()
        {
            while (true)
            {
                DisplayMainMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListAllFlights();
                        break;
                    case "2":
                        ListBoardingGates();
                        break;
                    case "3":
                        AssignBoardingGate();
                        break;
                    case "4":
                        CreateNewFlight();
                        break;
                    case "5":
                        DisplayAirlineFlights();
                        break;
                    case "6":
                        ModifyFlightDetails();
                        break;
                    case "7":
                        DisplayFlightSchedule();
                        break;
                    case "0":
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void DisplayMainMenu()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Welcome to Changi Airport Terminal 5");
            Console.WriteLine("=============================================");
            Console.WriteLine("1. List All Flights");
            Console.WriteLine("2. List Boarding Gates");
            Console.WriteLine("3. Assign a Boarding Gate to a Flight");
            Console.WriteLine("4. Create Flight");
            Console.WriteLine("5. Display Airline Flights");
            Console.WriteLine("6. Modify Flight Details");
            Console.WriteLine("7. Display Flight Schedule");
            Console.WriteLine("0. Exit");
            Console.WriteLine("\nPlease select your option:");
        }

        static void ListAllFlights()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("List of Flights for Changi Airport Terminal 5");
            Console.WriteLine("=============================================\n");
            Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-15} {4,-25}",
                "Flight Number", "Airline Name", "Origin", "Destination", "Expected Time");

            foreach (var flight in terminal.Flights.Values)
            {
                Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-15} {4,-25:g}",
                    flight.FlightNumber,
                    terminal.GetAirlineFromFlight(flight)?.Name ?? "Unknown",
                    flight.Origin,
                    flight.Destination,
                    flight.Expectedtime);
            }
        }

        static void ListBoardingGates()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
            Console.WriteLine("=============================================\n");
            Console.WriteLine("{0,-10} {1,-6} {2,-6} {3,-6} {4,-15}",
                "Gate", "DDJB", "CFFT", "LWTT", "Assigned Flight");

            foreach (var gate in terminal.BoardingGates.Values)
            {
                Console.WriteLine("{0,-10} {1,-6} {2,-6} {3,-6} {4,-15}",
                    gate.GateName,
                    gate.SupportsDDJB,
                    gate.SupportsCFFT,
                    gate.SupportsLWTT,
                    gate.Flight?.FlightNumber ?? "Unassigned");
            }
        }

        static void AssignBoardingGate()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Assign a Boarding Gate to a Flight");
            Console.WriteLine("=============================================\n");

            Console.WriteLine("Enter Flight Number:");
            string flightNum = Console.ReadLine().Trim();

            if (!terminal.Flights.TryGetValue(flightNum, out Flight flight))
            {
                Console.WriteLine("Flight not found.");
                return;
            }

            Console.WriteLine("Enter Boarding Gate Name:");
            string gateName = Console.ReadLine().Trim().ToUpper();

            if (!terminal.BoardingGates.TryGetValue(gateName, out BoardingGate gate))
            {
                Console.WriteLine("Gate not found.");
                return;
            }

            if (gate.Flight != null)
            {
                Console.WriteLine("Gate already assigned to another flight.");
                return;
            }

            // display flight and gate details
            DisplayFlightDetails(flight);
            Console.WriteLine($"\nBoarding Gate: {gate}");

            // update flight status
            Console.WriteLine("\nWould you like to update the status of the flight? (Y/N)");
            if (Console.ReadLine().Trim().ToUpper() == "Y")
            {
                UpdateFlightStatus(flight);
            }

            //assign gate
            gate.Flight = flight;
            Console.WriteLine($"\nFlight {flightNum} has been assigned to Boarding Gate {gateName}!");
        }

        static void CreateNewFlight()
        {
            Console.WriteLine("need to implement77777777777777777777777777777777777777777777777777777777777");
        }

        static void DisplayAirlineFlights()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
            Console.WriteLine("=============================================\n");

            Console.WriteLine("{0,-15} {1,-20}", "Airline Code", "Airline Name");
            foreach (var airline in terminal.Airlines.Values)
            {
                Console.WriteLine("{0,-15} {1,-20}", airline.Code, airline.Name);
            }

            Console.WriteLine("\nEnter Airline Code:");
            string airlineCode = Console.ReadLine().Trim().ToUpper();

            if (!terminal.Airlines.TryGetValue(airlineCode, out Airline selectedAirline))
            {
                Console.WriteLine("Airline not found.");
                return;
            }

            Console.WriteLine($"\nFlights for {selectedAirline.Name}:");
            foreach (var flight in selectedAirline.Flights.Values)
            {
                DisplayFlightDetails(flight);
            }
        }

        static void ModifyFlightDetails()
        {
            Console.WriteLine("need to implement7777777777777777777777777777777777777777777777777777777777t");
        }

        //static void DisplayFlightSchedule()

    }
}

