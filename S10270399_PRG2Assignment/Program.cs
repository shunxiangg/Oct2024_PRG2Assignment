
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

        public static void Main(string[] args)
        {
            InitializeSystem();
            RunMainMenu();
        }

        public static void InitializeSystem()
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
        public static void LoadAirlines()
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

        public static void LoadBoardingGates()
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
        public static void LoadFlights()
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

        public static void ProcessFlightLine(string line)
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

        public static Flight CreateFlight(string flightNum, string origin, string destination, DateTime expectedTime, string status, string specialRequestCode)
        {
            // This method implements a factory pattern for creating different types of flights
            // It handles the complexity of instantiating specific flight subclasses (CFFT, DDJB, LWTT, NORM)
            // based on the special request code, with appropriate default request fees
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
        public static void ListAllFlights()
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

                Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-18} {4,-25:g}", flight.FlightNumber, airlineName, flight.Origin, flight.Destination, flight.Expectedtime);
            }
        }

        //==========================================================
        // FEATURE 4
        //==========================================================
        public static void ListBoardingGates()
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
                Console.WriteLine("{0,-13} {1,-13} {2,-13} {3,-13} {4,-15}", gate.GateName, gate.SupportsDDJB, gate.SupportsCFFT, gate.SupportsLWTT, flightNumber);
            }
        }

        //==========================================================
        // FEATURE 5
        //==========================================================
        public static void AssignBoardingGate()
        {
            // 1. Validates both flight and gate existence
            // 2. Checks for gate availability
            // 3. Allows status updates during assignment
            // 4. Maintains the relationship between flights and gates
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
        public static void CreateNewFlight()
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
                string[] parts = flightNum.Split(' ');

                if (parts.Length != 2 || parts[0].Length != 2 || !parts[0].All(char.IsUpper) || !parts[1].All(char.IsDigit))
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

                // Add flight to terminal and airline
                terminal.Flights[flightNum] = newFlight;
                terminal.Airlines[airlineCode].AddFlight(newFlight);

                // Append to CSV file
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
        public static void DisplayAirlineFlights()
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

            Console.WriteLine("\n=============================================");
            Console.WriteLine($"List of Flights for {selectedAirline.Name}:");
            Console.WriteLine("=============================================\n");
            Console.WriteLine("{0,-15} {1,-20} {2,-18} {3,-18} {4,-25}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
            foreach (var flight in selectedAirline.Flights.Values)
            {
                DisplayAirline(flight);
            }
        }

        //==========================================================
        // FEATURE 8     OPTION 6
        //==========================================================
        public static void ModifyFlightDetails()
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

            // Display 6. Modify Flight Details
            Console.WriteLine($"\nList of Flights for {selectedAirline.Name}");
            Console.WriteLine("{0,-15} {1,-20} {2,-18} {3,-18} {4,-25}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
            foreach (var flight in selectedAirline.Flights.Values)
            {
                DisplayAirline(flight);
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
            Console.WriteLine("Choose an option: ");

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

        public static void ModifyFlight(Flight flight)
        {
            Console.WriteLine("\n1. Modify Basic Information");
            Console.WriteLine("2. Modify Status");
            Console.WriteLine("3. Modify Special Request Code");
            Console.WriteLine("4. Modify Boarding Gate");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine().Trim();

            try
            {
                switch (choice)
                {
                    case "1":
                        Console.Write("\nEnter new Origin: ");
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
                        Console.WriteLine("\nNote: Changing special request code will create a new flight object.");
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

                        Console.Write("\nEnter new Boarding Gate: ");
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


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error modifying flight: {ex.Message}");
            }
            //BoardingGate assignedGate = null; //make it null
            //foreach (var gate in terminal.BoardingGates.Values)
            //{
            //    if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
            //    {
            //        assignedGate = gate;
            //        Console.WriteLine($"Boarding Gate: {assignedGate.GateName}");

            //    }
            //    else
            //    {
            //        Console.WriteLine($"Boarding Gate: Unassigned"); // display the boarding gate namw if found, otherwise display "Unassigned"
            //        break;
            //    }
            //}
            BoardingGate assignedGate = null;
            bool gateFound = false;

            foreach (var gate in terminal.BoardingGates.Values)
            {
                if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
                {
                    assignedGate = gate;   // if a match is found assign this gate to 'assignedGate'
                    gateFound = true;      // set the flag to true indicating a gate was found
                    break;                 // exit the loop since we've found the matching gatw
                }
            }

            Console.WriteLine($"Boarding Gate: {(gateFound ? assignedGate.GateName : "Unassigned")}");   /// display the boarding gate namw if found, otherwise display "Unassigned"
        }

        public static void DeleteFlight(Airline airline, Flight flight)
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

        public static void DisplayFlightSchedule()
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





        public static void DisplayFlightDetails(Flight flight)
        {
            //Console.WriteLine("{0,-15} {1,-20} {2,-18} {3,-18} {4,-25}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

            //string airlineName = "Unknown";
            //var airline = terminal.GetAirlineFromFlight(flight);   //using terminal get airline method
            //if (airline != null && airline.Name != null)
            //{
            //    airlineName = airline.Name;
            //}

            //Console.WriteLine($"{flight.FlightNumber,-15} {airlineName,-20} {flight.Origin,-18} {flight.Destination,-18} {flight.Expectedtime:g,-25}");


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




        public static void DisplayAirline(Flight flight)
        {

            string airlineName = "Unknown";
            var airline = terminal.GetAirlineFromFlight(flight);   //using terminal get airline method
            if (airline != null && airline.Name != null)
            {
                airlineName = airline.Name;
            }

            Console.WriteLine($"{flight.FlightNumber,-15} {airlineName,-20} {flight.Origin,-18} {flight.Destination,-18} {flight.Expectedtime,-25}");
        }





        public static void UpdateFlightStatus(Flight flight)
        {
            Console.WriteLine("\n1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");
            Console.WriteLine("Select new status:");

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






        public static void RunMainMenu()
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
                    case "8":
                        ProcessUnassignedFlights();
                        break;
                    case "9":
                        DisplayAirlineFees();
                        break;
                    case "10":
                        HandleRemainingUnassignedFlights();
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

        public static void DisplayMainMenu()
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
            Console.WriteLine("8. (ADVANCE Feature) Process all unassigned flights to boarding gates in bulk");
            Console.WriteLine("9. (ADVANCE Feature) Display the total fee per airline for the day");
            Console.WriteLine("10. (Additional Feature) Handle Remaining Unassigned Flights");
            Console.WriteLine("0. Exit");
            Console.WriteLine("\nPlease select your option:");
        }









        //===============================================================
        // ADVANCED FEATURE A - Process Unassigned Flights 
        // NAME: Ang Shun Xiang
        // STUDENT ID: S10270399
        //=============================================================
        public static void ProcessUnassignedFlights()
        {
            Queue<Flight> unassignedFlights = new Queue<Flight>();
            int totalUnassignedFlights = 0;
            int totalUnassignedGates = 0;
            //for each Flight, check if a Boarding Gate is assigned; if there is none, add it to a queue
            foreach (var flight in terminal.Flights.Values)
            {
                bool isAssigned = false;
                foreach (var gate in terminal.BoardingGates.Values)
                {
                    if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
                    {
                        isAssigned = true;
                        break;
                    }
                }
                if (!isAssigned)       //if (isAssigned == false)
                {
                    unassignedFlights.Enqueue(flight);
                    totalUnassignedFlights++;
                }
            }

            // For each Boarding Gate, check if a Flight Number has been assigned
            foreach (var gate in terminal.BoardingGates.Values)
            {
                if (gate.Flight == null)
                {
                    totalUnassignedGates = totalUnassignedGates + 1;

                }
            }
            // 	display the total number of Flights that do not have any Boarding Gate assigned yet
            Console.WriteLine($"\nTotal unassigned flights: {totalUnassignedFlights}");
            //  Display the total number of Boarding Gates that do not have a Flight Number assigned yet
            Console.WriteLine($"Total available gates: {totalUnassignedGates}");
            Console.WriteLine("\n\nAssigned Flight Details:");
            Console.WriteLine("=================ADVANCE FEATURE A========================================================================");
            Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-15} {5,-25}",
                "Flight Number", "Airline Name", "Origin", "Destination", "Gate Name", "Departure Time");
            Console.WriteLine("==========================================================================================================");
            int processedCount = 0;
            // For each Flight in the queue, dequeue the first Flight in the queue
            while (unassignedFlights.Count > 0)
            {
                Flight flight = unassignedFlights.Dequeue();
                bool assigned = false;

                //Check if the Flight has a Special Request Code
                string specialRequest = "";
                if (flight is CFFTFFlight) specialRequest = "CFFT";
                else if (flight is DDJBFlight) specialRequest = "DDJB";
                else if (flight is LWTTFlight) specialRequest = "LWTT";

                //  If yes, search for an unassigned Boarding Gate that matches the Special Request Code
                foreach (var gate in terminal.BoardingGates.Values)
                {
                    if (gate.Flight != null) continue;

                    bool isCompatible = false;

                    //If no, search for an unassigned Boarding Gate that has no Special Request Code
                    if (specialRequest == "")
                    {
                        // For normal flights any free gate is fine
                        isCompatible = true;
                    }
                    else
                    {
                        // Check if gate supports the special request
                        isCompatible = (specialRequest == "CFFT" && gate.SupportsCFFT) ||
                                     (specialRequest == "DDJB" && gate.SupportsDDJB) ||
                                     (specialRequest == "LWTT" && gate.SupportsLWTT);
                    }

                    //Assign the Boarding Gate to the Flight Number
                    if (isCompatible)
                    {
                        gate.Flight = flight;

                        assigned = true;
                        processedCount = processedCount + 1;

                        ////Display the Flight details with Basic Information of all Flights
                        //Console.WriteLine($"Assigned flight {flight.FlightNumber} to gate {gate.GateName}");
                        string airlineCode = flight.FlightNumber.Substring(0, 2); // split the string starting from the first index then select 2 number
                        string airlineName;
                        if (terminal.Airlines.ContainsKey(airlineCode))
                        {
                            airlineName = terminal.Airlines[airlineCode].Name; // get airline name if the code exists
                        }
                        else
                        {
                            airlineName = "Unknown Airline"; // console write line this if the code does not exist
                        }

                        Console.WriteLine("{0,-15} {1,-20} {2,-20} {3,-20} {4,-15} {5,-25}",
                        flight.FlightNumber,
                        airlineName,
                        flight.Origin,
                        flight.Destination,
                        gate.GateName,
                        flight.Expectedtime.ToString("g"));
                        break;
                    }
                }

                if (!assigned)
                {
                    Console.WriteLine($"Could not find suitable gate for flight {flight.FlightNumber}");
                }
            }
            Console.WriteLine("==========================================================================================================");
            //Display the total number of Flights and Boarding Gates processed and assigned
            double processedPercentage = (processedCount / (double)totalUnassignedFlights) * 100;
            Console.WriteLine($"\nProcessed {processedCount} out of {totalUnassignedFlights} flights");
            Console.WriteLine($"Processing percentage: {processedPercentage:F2}%");
        }







        //==========================================================
        // ADVANCED FEATURE B - Display Airline Fees
        // NAME: Aiden Tan
        // STUDENT ID: S10267626E
        //==========================================================
        public static void DisplayAirlineFees()
        {
            // First check if all flights have gates assigned
            bool allFlightsAssigned = true;
            foreach (var flight in terminal.Flights.Values)
            {
                bool hasGate = false;
                foreach (var gate in terminal.BoardingGates.Values)
                {
                    if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
                    {
                        hasGate = true;
                        break;
                    }
                }
                if (!hasGate)
                {
                    allFlightsAssigned = false;
                    break;
                }
            }

            if (!allFlightsAssigned)
            {
                Console.WriteLine("\nWarning: Not all flights have been assigned gates.");
                Console.WriteLine("Please assign gates to all flights before calculating fees.");
                return;
            }

            Console.WriteLine("\n=============================================");
            Console.WriteLine("Airline Fees Report for Terminal 5");
            Console.WriteLine("=============================================\n");

            double totalTerminalFees = 0;
            double totalDiscounts = 0;

            foreach (var airline in terminal.Airlines.Values)
            {
                if (airline.Flights.Count == 0) continue;

                double subtotal = 0;
                double discounts = 0;

                Console.WriteLine($"\nAirline: {airline.Name} ({airline.Code})");
                Console.WriteLine("----------------------------------------");

                /// Calculate base fees and special request fees
                foreach (var flight in airline.Flights.Values)
                {
                    /// Base fee based on origin/destination
                    double flightFee = flight.Origin == "Singapore (SIN)" ? 800.00 : 500.00;
                    subtotal += flightFee;

                    // /Special request fees
                    if (flight is CFFTFFlight)
                    {
                        CFFTFFlight cfftFlight = (CFFTFFlight)flight;
                        subtotal += 150.00;
                    }
                    else if (flight is DDJBFlight)
                    {
                        subtotal += 300.00;
                    }
                    else if (flight is LWTTFlight)
                    {
                        subtotal += 500.00;
                    }

                    // Gate base fee
                    subtotal += 300.00;
                }

                ///calculate discounts
                int flightCount = airline.Flights.Count;

                // discount for every 3 flights
                discounts += Math.Floor(flightCount / 3.0) * 350.00;

                // heck each flight for other discounts
                foreach (var flight in airline.Flights.Values)
                {
                    // Off-peak timing discount
                    var flightTime = flight.Expectedtime.TimeOfDay;
                    if (flightTime < TimeSpan.FromHours(11) || flightTime > TimeSpan.FromHours(21))
                    {
                        discounts += 110.00;
                    }

                    // Special origin discount
                    if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" ||
                        flight.Origin == "Tokyo (NRT)")
                    {
                        discounts += 25.00;
                    }

                    // No special request discount
                    if (flight is NORMFlight)
                    {
                        discounts += 50.00;
                    }
                }

                if (flightCount > 5)
                {
                    discounts += subtotal * 0.03;
                }

                double finalTotal = subtotal - discounts;
                totalTerminalFees += finalTotal;
                totalDiscounts += discounts;

                Console.WriteLine($"Subtotal: ${subtotal:F2}");
                Console.WriteLine($"Discounts: ${discounts:F2}");
                Console.WriteLine($"Final Total: ${finalTotal:F2}");
            }

            Console.WriteLine("\n=============================================");
            Console.WriteLine("Terminal Summary");
            Console.WriteLine("=============================================");
            Console.WriteLine($"Total Terminal Fees: ${totalTerminalFees:F2}");
            Console.WriteLine($"Total Discounts Applied: ${totalDiscounts:F2}");
            double discountPercentage = (totalDiscounts / (totalTerminalFees + totalDiscounts)) * 100;
            Console.WriteLine($"Discount Percentage: {discountPercentage:F2}%\n\n");
        }



        //==========================================================
        // ADDITIONAL FEATURE - Handle Remaining Unassigned Flights From Advance feature A else advance feature B cannot run as there is unassigned flight
        //==========================================================

        public static void HandleRemainingUnassignedFlights()
        {
            Console.WriteLine("\n=============================================");
            Console.WriteLine("Handling Remaining Unassigned Flights");
            Console.WriteLine("=============================================\n");

            List<Flight> unassignedFlights = new List<Flight>();
            List<BoardingGate> availableGates = new List<BoardingGate>();

            /// This double loop finds flights that dont have gates assigned
            foreach (var flight in terminal.Flights.Values)
            {
                bool isAssigned = false;
                foreach (var gate in terminal.BoardingGates.Values)
                {
                    if (gate.Flight != null && gate.Flight.FlightNumber == flight.FlightNumber)
                    {
                        isAssigned = true;
                        break;
                    }
                }
                if (!isAssigned)
                {
                    unassignedFlights.Add(flight);
                }
            }


            if (unassignedFlights.Count == 0)
            {
                Console.WriteLine("No unassigned flights found. All flights have been assigned to gates.");
                return;
            }

            // Find all available gates
            foreach (var gate in terminal.BoardingGates.Values)
            {
                if (gate.Flight == null)
                {
                    availableGates.Add(gate);
                }
            }

            if (availableGates.Count == 0)
            {
                Console.WriteLine("No available gates found. Cannot process remaining unassigned flights.");
                return;
            }

            Console.WriteLine($"Found {unassignedFlights.Count} unassigned flights and {availableGates.Count} available gates.\n");

            Console.WriteLine("Unassigned Flights:");
            foreach (var flight in unassignedFlights)
            {
                string specialRequest = "";
                if (flight is CFFTFFlight) specialRequest = "CFFT";
                else if (flight is DDJBFlight) specialRequest = "DDJB";
                else if (flight is LWTTFlight) specialRequest = "LWTT";
                else specialRequest = "NORM";

                Console.WriteLine($"Flight {flight.FlightNumber} - Type: {specialRequest}");
            }


            Console.WriteLine("\nAvailable Gates and their capabilities:");
            Console.WriteLine("==========================================");
            Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15}", "Gate", "CFFT Support", "DDJB Support", "LWTT Support");
            Console.WriteLine("------------------------------------------");
            foreach (var gate in availableGates)
            {
                Console.WriteLine("{0,-10} {1,-15} {2,-15} {3,-15}",gate.GateName, gate.SupportsCFFT, gate.SupportsDDJB,gate.SupportsLWTT);
            }
            Console.WriteLine("==========================================");


            Console.WriteLine("\nWould you like to attempt to assign these flights? (Y/N)");
            if (Console.ReadLine().Trim().ToUpper() != "Y")
            {
                return;
            }

            int assignedCount = 0;
            Console.WriteLine("\nAssignment Results:");
            Console.WriteLine("==========================================");

            foreach (var flight in unassignedFlights)
            {
                BoardingGate bestGate = null;
                int bestCompatibilityScore = -1; // Make this Assignmennt to Unassigned flights

                foreach (var gate in availableGates)
                {
                    if (gate.Flight != null) continue;

                    int compatibilityScore = 0;

                    // Check compatibility based on flight type
                    // Score 3: Perfect match (special request flight with matching gate capability)
                    // Score 1: Normal flight (can use any gate)
                    // Score -1: No compatibility
                    if (flight is CFFTFFlight && gate.SupportsCFFT) compatibilityScore = 3;
                    else if (flight is DDJBFlight && gate.SupportsDDJB) compatibilityScore = 3;
                    else if (flight is LWTTFlight && gate.SupportsLWTT) compatibilityScore = 3;
                    else if (flight is NORMFlight) compatibilityScore = 1;

                    // If this gate is better than our current best option
                    if (compatibilityScore > bestCompatibilityScore)
                    {
                        bestCompatibilityScore = compatibilityScore;
                        bestGate = gate;
                    }
                }

                if (bestGate != null)
                {
                    bestGate.Flight = flight;
                    availableGates.Remove(bestGate);
                    assignedCount++;

                    string specialRequest = "";
                    if (flight is CFFTFFlight) specialRequest = "CFFT";
                    else if (flight is DDJBFlight) specialRequest = "DDJB";
                    else if (flight is LWTTFlight) specialRequest = "LWTT";
                    else specialRequest = "NORM";

                    Console.WriteLine($"Assigned flight {flight.FlightNumber} ({specialRequest}) to gate {bestGate.GateName}");
                }
                else
                {
                    Console.WriteLine($"Could not find a suitable gate for flight {flight.FlightNumber}");
                }
            }

            Console.WriteLine("\nSummary:");
            Console.WriteLine($"Successfully assigned {assignedCount} out of {unassignedFlights.Count} flights");
            double successRate = (double)assignedCount / unassignedFlights.Count * 100;
            Console.WriteLine($"Success rate: {successRate:F2}%");

            if (assignedCount < unassignedFlights.Count)
            {
                Console.WriteLine($"\nWarning: {unassignedFlights.Count - assignedCount} flights remain unassigned!");
                Console.WriteLine("Please consider manual assignment or reviewing gate availability.");
            }
        }





    }
}























