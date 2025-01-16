


using S10270399_PRG2Assignment;




if (File.Exists("airlines.csv"))
{
    string[] airlineLines = File.ReadAllLines("airlines.csv");
    for (int i = 1; i < airlineLines.Length; i++) // Skip header
    {
        string[] data = airlineLines[i].Split(',');
        terminal.AddAirline(new Airline(data[0], data[1]));
    }
}

// Load Boarding Gates
if (File.Exists("boardinggates.csv"))
{
    string[] gateLines = File.ReadAllLines("boardinggates.csv");
    for (int i = 1; i < gateLines.Length; i++) // Skip header
    {
        string[] data = gateLines[i].Split(',');
        terminal.AddBoardingGate(new BoardingGate(
            data[0],
            bool.Parse(data[1]),
            bool.Parse(data[2]),
            bool.Parse(data[3])
        ));
    }
}

//// Display the headers
//string[] header = flightDetails[0].Split(',');
//        Console.WriteLine("{0,-15} {1,-15} {2,-25} {3,-20} {4,-20}", header[0], header[1], header[2], header[3], header[3]);

//        // Process and display each bus stop's data
//        for (int i = 1; i<flightDetails.Length; i++)
//        {
//            string[] data = flightDetails[i].Split(',');
//            string flightNum = data[0];
//            string origin = data[1];
//            string destination = data[2];
//            datetime departure_arrival = data[3]; 
//            string  specialRequestCode= data[4];

//            // Create BusStop object (assuming you have a BusStop class defined elsewhere)
//            Flight flight = new Flight(flightNum, origin, destination, departure_arrival, specialRequestCode);

//            // Display the details of each bus stop
//            Console.WriteLine("{0,-15} {1,-15} {2,-25} {3,-20}", flight.flightNum, flight.origin, flight.destination, flight.departure_arrival, flight.specialRequestCode);
//        }



//if (File.Exists(flightfilePath))
//{
//    if (flightDetails.Length > 0)
//    {
//        // Display the headers
//        string[] header = flightDetails[0].Split(',');
//        Console.WriteLine("{0,-15} {1,-15} {2,-25} {3,-20} {4,-20}", header[0], header[1], header[2], header[3], header[3]);

//        // Process and display each bus stop's data
//        for (int i = 1; i < flightDetails.Length; i++)
//        {
//            string[] data = flightDetails[i].Split(',');
//            string flightNum = data[0];
//            string origin = data[1];
//            string destination = data[2];
//            datetime departure_arrival = data[3];
//            string specialRequestCode = data[4];

//            // Create BusStop object (assuming you have a BusStop class defined elsewhere)
//            Flight flight = new Flight(flightNum, origin, destination, departure_arrival, specialRequestCode);

//            // Display the details of each bus stop
//            Console.WriteLine("{0,-15} {1,-15} {2,-25} {3,-20}", flight.flightNum, flight.origin, flight.destination, flight.departure_arrival, flight.specialRequestCode);
//        }
//    }
//    else
//    {
//        Console.WriteLine("No data found in the file.");
//    }
//}
//else
//{
//    Console.WriteLine("File not found: " + filePath);
//}






