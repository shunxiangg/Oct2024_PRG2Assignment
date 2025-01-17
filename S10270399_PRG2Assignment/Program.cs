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

        static void LoadBoardingGates()
        {
            if (File.Exists("boardinggates.csv"))
            {
                string[] lines = File.ReadAllLines("boardinggates.csv");
                for (int i = 1; i < lines.Length; i++) // Skip header
                {
                    string[] data = lines[i].Split(',');
                    var gate = new BoardingGate(
                        data[0].Trim(),
                        bool.Parse(data[1]),
                        bool.Parse(data[2]),
                        bool.Parse(data[3])
                    );
                    terminal.AddBoardingGate(gate);
                }
            }
        }

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

        static void ProcessFlightLine(string line)
        {
            string[] data = line.Split(',');
            string flightNum = data[0].Trim();
            string origin = data[1].Trim();
            string destination = data[2].Trim();

            if (!DateTime.TryParse(data[3], out DateTime expectedTime))
            {
                throw new FormatException($"Invalid date format for flight: {flightNum}");
            }

            string specialRequestCode = data.Length > 4 ? data[4].Trim() : "";
            string status = "Scheduled";

            Flight flight = CreateFlight(flightNum, origin, destination, expectedTime, status, specialRequestCode);

            // Add flight to terminal and airline
            string airlineCode = flightNum.Substring(0, 2);
            if (terminal.Airlines.TryGetValue(airlineCode, out Airline airline))
            {
                airline.AddFlight(flight);
                terminal.Flights[flightNum] = flight;
            }
        }

        static Flight CreateFlight(string flightNum, string origin, string destination,
            DateTime expectedTime, string status, string specialRequestCode)
        {
            return specialRequestCode switch
            {
                "CFFT" => new CFFTFFlight(flightNum, origin, destination, expectedTime, status, 50.0),
                "DDJB" => new DDJBFlight(flightNum, origin, destination, expectedTime, status, 75.0),
                "LWTT" => new LWTTFlight(flightNum, origin, destination, expectedTime, status, 60.0),
                _ => new NORMFlight(flightNum, origin, destination, expectedTime, status),
            };
        }

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

            // Display flight and gate details
            DisplayFlightDetails(flight);
            Console.WriteLine($"\nBoarding Gate: {gate}");

            // Update flight status
            Console.WriteLine("\nWould you like to update the status of the flight? (Y/N)");
            if (Console.ReadLine().Trim().ToUpper() == "Y")
            {
                UpdateFlightStatus(flight);
            }

            // Assign gate
            gate.Flight = flight;
            Console.WriteLine($"\nFlight {flightNum} has been assigned to Boarding Gate {gateName}!");
        }

        static void CreateNewFlight()
        {
            Console.WriteLine("add this feature 777777777777777777777777777777");
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
            Console.WriteLine("add this feature 777777777777777777777777777777");
        }

        static void DisplayFlightSchedule()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
            Console.WriteLine("=============================================\n");

            var sortedFlights = terminal.Flights.Values
                .OrderBy(f => f.Expectedtime)
                .ToList();

            Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-15} {4,-25} {5,-10} {6,-15}",
                "Flight Number", "Airline Name", "Origin", "Destination", "Expected Time", "Status", "Gate");

            foreach (var flight in sortedFlights)
            {
                string gateName = terminal.BoardingGates.Values
                    .FirstOrDefault(g => g.Flight?.FlightNumber == flight.FlightNumber)?.GateName ?? "Unassigned";

                Console.WriteLine("{0,-15} {1,-15} {2,-15} {3,-15} {4,-25:g} {5,-10} {6,-15}",
                    flight.FlightNumber,
                    terminal.GetAirlineFromFlight(flight)?.Name ?? "Unknown",
                    flight.Origin,
                    flight.Destination,
                    flight.Expectedtime,
                    flight.Status,
                    gateName);
            }
        }

        static void DisplayFlightDetails(Flight flight)
        {
            Console.WriteLine($"\nFlight Number: {flight.FlightNumber}");
            Console.WriteLine($"Origin: {flight.Origin}");
            Console.WriteLine($"Destination: {flight.Destination}");
            Console.WriteLine($"Expected Time: {flight.Expectedtime:g}");
            Console.WriteLine($"Status: {flight.Status}");

            if (flight is CFFTFFlight)
                Console.WriteLine("Special Request: CFFT");
            else if (flight is DDJBFlight)
                Console.WriteLine("Special Request: DDJB");
            else if (flight is LWTTFlight)
                Console.WriteLine("Special Request: LWTT");
        }

        static void UpdateFlightStatus(Flight flight)
        {
            Console.WriteLine("1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");
            Console.WriteLine("\nSelect new status:");

            string choice = Console.ReadLine().Trim();
            switch (choice)
            {
                case "1":
                    flight.Status = "Delayed";
                    break;
                case "2":
                    flight.Status = "Boarding";
                    break;
                case "3":
                    flight.Status = "On Time";
                    break;
                default:
                    Console.WriteLine("Invalid choice. Status unchanged.");
                    break;
            }
        }
    }
}