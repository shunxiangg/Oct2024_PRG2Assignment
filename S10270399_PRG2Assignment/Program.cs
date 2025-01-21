
//==========================================================
// Student1 Number	: S10267626
// Student1 Name	: Aiden Tan Yihan
// Student2 Number  : S10270399
// Partner2 Name	: Ang Shun Xiang
//==========================================================



using S10270399_PRG2Assignment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace S10270399_PRG2Assignment
{
    class Program
    {
        static Terminal? terminal;

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

        //==========================================================
        // FEATURE 1
        //==========================================================
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


        //==========================================================
        // FEATURE 2
        //==========================================================
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
        //shunxiang
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
        //shunxiang
        static Flight CreateFlight(string flightNum, string origin, string destination, DateTime expectedTime, string status, string specialRequestCode)
        {
            if (specialRequestCode == "CFFT")
            {
                return new CFFTFFlight(flightNum, origin, destination, expectedTime, status, 50.0);
            }
            else if (specialRequestCode == "DDJB")
            {
                return new DDJBFlight(flightNum, origin, destination, expectedTime, status, 75.0);
            }
            else if (specialRequestCode == "LWTT")
            {
                return new LWTTFlight(flightNum, origin, destination, expectedTime, status, 60.0);
            }
            else
            {
                return new NORMFlight(flightNum, origin, destination, expectedTime, status);
            }
        }




        //==========================================================
        // FEATURE 3
        //==========================================================
        static void ListAllFlights()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("List of Flights for Changi Airport Terminal 5");
            Console.WriteLine("=============================================\n");
            Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-18} {4,-25}",
                "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

            foreach (var flight in terminal.Flights.Values)
            {
                //set to "Unknown" if no airlineNme is found
                string airlineName = "Unknown";
                var airline = terminal.GetAirlineFromFlight(flight);
                if (airline != null && airline.Name != null)
                {
                    airlineName = airline.Name;
                }

                Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-18} {4,-25:g}", flight.FlightNumber,airlineName,flight.Origin,flight.Destination,flight.Expectedtime);
            }
        }

        //==========================================================
        // FEATURE 4
        //==========================================================
        static void ListBoardingGates()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
            Console.WriteLine("=============================================\n");
            Console.WriteLine("{0,-13} {1,-13} {2,-13} {3,-13} {4,-15}",
                "Gate", "DDJB", "CFFT", "LWTT", "Assigned Flight");

            foreach (var gate in terminal.BoardingGates.Values)
            {
                string flightNumber = "Unassigned";
                if (gate.Flight != null)
                {
                    flightNumber = gate.Flight.FlightNumber;
                }
                Console.WriteLine("{0,-13} {1,-13} {2,-13} {3,-13} {4,-15}",gate.GateName,gate.SupportsDDJB, gate.SupportsCFFT,gate.SupportsLWTT,flightNumber);
            }
        }

        //==========================================================
        // FEATURE 5
        //==========================================================
        static void AssignBoardingGate()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Assign a Boarding Gate to a Flight");
            Console.WriteLine("=============================================");

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

        //==========================================================
        // FEATURE 6
        //==========================================================
        static void CreateNewFlight()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Create New Flight");
            Console.WriteLine("=============================================\n");

            try
            {
                // Get basic flight information
                Console.Write("Enter Flight Number: ");
                string flightNum = Console.ReadLine().Trim().ToUpper();

                // Validate flight number format (2 letters followed by space and numbers)
                if (!System.Text.RegularExpressions.Regex.IsMatch(flightNum, @"^[A-Z]{2}\s\d+$"))
                {
                    Console.WriteLine("Invalid flight number format. Should be like 'SQ 123'");
                    return;
                }

                // Check if flight already exists
                if (terminal.Flights.ContainsKey(flightNum))
                {
                    Console.WriteLine("Flight number already exists!");
                    return;
                }

                // Validate airline code exists
                string airlineCode = flightNum.Substring(0, 2);
                if (!terminal.Airlines.ContainsKey(airlineCode))
                {
                    Console.WriteLine("Invalid airline code!");
                    return;
                }

                Console.Write("Enter Origin: ");
                string origin = Console.ReadLine().Trim();

                Console.Write("Enter Destination: ");
                string destination = Console.ReadLine().Trim();

                Console.Write("Enter Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
                if (!DateTime.TryParse(Console.ReadLine().Trim(), out DateTime expectedTime))
                {
                    Console.WriteLine("Invalid date/time format!");
                    return;
                }

                // Get special request code
                Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                string specialRequestCode = Console.ReadLine().Trim().ToUpper();

                Flight newFlight;

                if (specialRequestCode == "CFFT")
                {
                    newFlight = new CFFTFFlight(flightNum, origin, destination, expectedTime, "Scheduled", 50.0);
                }
                else if (specialRequestCode == "DDJB")
                {
                    newFlight = new DDJBFlight(flightNum, origin, destination, expectedTime, "Scheduled", 75.0);
                }
                else if (specialRequestCode == "LWTT")
                {
                    newFlight = new LWTTFlight(flightNum, origin, destination, expectedTime, "Scheduled", 60.0);
                }
                else if (specialRequestCode == "NONE")
                {
                    newFlight = new NORMFlight(flightNum, origin, destination, expectedTime, "Scheduled");
                }
                else
                {
                    throw new ArgumentException("Invalid special request code");
                }


                // add flight to terminal and airline
                terminal.Flights[flightNum] = newFlight;
                terminal.Airlines[airlineCode].AddFlight(newFlight);

                // append to CSV file
                using (StreamWriter sw = File.AppendText("flights.csv"))
                {
                    sw.WriteLine($"{flightNum},{origin},{destination},{expectedTime:dd/MM/yyyy HH:mm},{specialRequestCode}");
                }

                Console.WriteLine($"\nFlight {flightNum} has been added!");

                Console.Write("\nWould you like to add another flight? (Y/N): ");
                if (Console.ReadLine().Trim().ToUpper() == "Y")
                {
                    CreateNewFlight();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating flight: {ex.Message}");
            }
        }
        //==========================================================
        // FEATURE 7
        //==========================================================
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

        //==========================================================
        // FEATURE 8
        //==========================================================
        static void ModifyFlightDetails()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
            Console.WriteLine("=============================================\n");

            // Display airlines
            Console.WriteLine("{0,-15} {1,-20}", "Airline Code", "Airline Name");
            foreach (var airline in terminal.Airlines.Values)
            {
                Console.WriteLine("{0,-15} {1,-20}", airline.Code, airline.Name);
            }

            // Get airline code
            Console.Write("\nEnter Airline Code: ");
            string airlineCode = Console.ReadLine().Trim().ToUpper();

            if (!terminal.Airlines.TryGetValue(airlineCode, out Airline selectedAirline))
            {
                Console.WriteLine("Airline not found.");
                return;
            }

            // Display flights for selected airline
            Console.WriteLine($"\nList of Flights for {selectedAirline.Name}");
            foreach (var flight in selectedAirline.Flights.Values)
            {
                DisplayFlightDetails(flight);
            }

            // Get flight to modify
            Console.Write("\nChoose an existing Flight to modify or delete: ");
            string flightNum = Console.ReadLine().Trim().ToUpper();

            if (!selectedAirline.Flights.TryGetValue(flightNum, out Flight selectedFlight))
            {
                Console.WriteLine("Flight not found.");
                return;
            }

            Console.WriteLine("\n1. Modify Flight");
            Console.WriteLine("2. Delete Flight");
            Console.WriteLine("\nChoose an option: ");

            string choice = Console.ReadLine().Trim();

            switch (choice)
            {
                case "1":
                    ModifyFlight(selectedFlight);
                    break;
                case "2":
                    DeleteFlight(selectedAirline, selectedFlight);
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        static void ModifyFlight(Flight flight)
        {
            Console.WriteLine("\n1. Modify Basic Information");
            Console.WriteLine("2. Modify Status");
            Console.WriteLine("3. Modify Special Request Code");
            Console.WriteLine("4. Modify Boarding Gate");
            Console.Write("\nChoose an option: ");

            string choice = Console.ReadLine().Trim();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("Enter new Origin: ");
                        flight.Origin = Console.ReadLine().Trim();

                        Console.Write("Enter new Destination: ");
                        flight.Destination = Console.ReadLine().Trim();

                        Console.Write("Enter new Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
                        if (DateTime.TryParse(Console.ReadLine().Trim(), out DateTime newTime))
                        {
                            flight.Expectedtime = newTime;
                        }
                        else
                        {
                            Console.WriteLine("Invalid date/time format!");
                            return;
                        }
                        break;

                    case "2":
                        UpdateFlightStatus(flight);
                        break;

                    case "3":   //-handling of user input for special request
                        Console.WriteLine("Note: Changing special request code will create a new flight object.");
                        Console.Write("Enter new Special Request Code (CFFT/DDJB/LWTT/None): ");
                        string newCode = Console.ReadLine().Trim().ToUpper();

                        Flight newFlight;
                        if (newCode == "CFFT")
                        {
                            newFlight = new CFFTFFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.Expectedtime, flight.Status, 50.0);
                        }
                        else if (newCode == "DDJB")
                        {
                            newFlight = new DDJBFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.Expectedtime, flight.Status, 75.0);
                        }
                        else if (newCode == "LWTT")
                        {
                            newFlight = new LWTTFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.Expectedtime, flight.Status, 60.0);
                        }
                        else if (newCode == "NONE")
                        {
                            newFlight = new NORMFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.Expectedtime, flight.Status);
                        }
                        else
                        {
                            throw new ArgumentException("Invalid special request code");
                        }

                        // Replace flight in terminal and airline
                        string airlineCode = flight.FlightNumber.Substring(0, 2);
                        terminal.Flights[flight.FlightNumber] = newFlight;
                        terminal.Airlines[airlineCode].Flights[flight.FlightNumber] = newFlight;
                        break;

                    case "4":
                        BoardingGate currentGate = null;
                        foreach (var g in terminal.BoardingGates.Values)
                        {
                            if (g.Flight != null && g.Flight.FlightNumber == flight.FlightNumber)
                            {
                                currentGate = g;
                                break;
                            }
                        }


                        if (currentGate != null)
                        {
                            currentGate.Flight = null;  // Remove current assignment
                        }

                        Console.Write("Enter new Boarding Gate: ");
                        string newGateName = Console.ReadLine().Trim().ToUpper();

                        if (terminal.BoardingGates.TryGetValue(newGateName, out BoardingGate newGate))
                        {
                            if (newGate.Flight != null)
                            {
                                Console.WriteLine("Gate already assigned to another flight!");
                                return;
                            }
                            newGate.Flight = flight;
                        }
                        else
                        {
                            Console.WriteLine("Invalid gate name!");
                            return;
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid option.");
                        return;
                }

                Console.WriteLine("\nFlight updated!");
                DisplayFlightDetails(flight);

                BoardingGate assignedGate = null; //make it null
                foreach (var gate in terminal.BoardingGates.Values)
                {
                    if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
                    {
                        assignedGate = gate;
                        Console.WriteLine($"Boarding Gate: {assignedGate?.GateName ?? "Unassigned"}");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error modifying flight: {ex.Message}");
            }
        }
      
        static void DeleteFlight(Airline airline, Flight flight)
        {
            Console.Write("Are you sure you want to delete this flight? (Y/N): ");
            if (Console.ReadLine().Trim().ToUpper() == "Y")
            {
                // Remove from terminal and airline
                terminal.Flights.Remove(flight.FlightNumber);
                airline.RemoveFlight(flight);

                BoardingGate gate = null;
                foreach (var g in terminal.BoardingGates.Values)
                {
                    if (g.Flight != null && g.Flight.FlightNumber == flight.FlightNumber)
                    {
                        gate = g;
                        break;
                    }
                }

                if (gate != null)
                {
                    gate.Flight = null;
                }

                Console.WriteLine($"Flight {flight.FlightNumber} has been deleted.");
            }
        }



        //==========================================================
        // FEATURE 9
        //==========================================================

        static void DisplayFlightSchedule()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
            Console.WriteLine("=============================================\n");

            
            List<Flight> sortedFlights = new List<Flight>(terminal.Flights.Values);
            //  IComparable Sort()
            sortedFlights.Sort();

            Console.WriteLine("{0,-15} {1,-20} {2,-18} {3,-18} {4,-25} {5,-10} {6,-15}", "Flight Number", "Airline Name", "Origin", "Destination", "Departure/Arrival Time", "Status", "Boarding Gate");

            foreach (var flight in sortedFlights)
            {
                string gateName = "Unassigned";

                foreach (var gate in terminal.BoardingGates.Values)
                {
                    if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
                    {
                        gateName = gate.GateName;
                        break; 
                    }
                }

                string airlineName = "Unknown";

                var airline = terminal.GetAirlineFromFlight(flight);
                if (airline != null && airline.Name != null)
                {
                    airlineName = airline.Name;
                }

                Console.WriteLine("{0,-15} {1,-20} {2,-18} {3,-18} {4,-25:G} {5,-10} {6,-15}",
                    flight.FlightNumber,
                    airlineName,
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
                Console.WriteLine("Special Request Code: CFFT");
            else if (flight is DDJBFlight)
                Console.WriteLine("Special Request Code: DDJB");
            else if (flight is LWTTFlight)
                Console.WriteLine("Special Request Code: LWTT");
            else if (flight is NORMFlight)
                Console.WriteLine("Special Request Code: None");
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
    }
}
























