


namespace S10270399_PRG2Assignment;




string flightfilePath = "flights.csv";
string[] flightDetails = File.ReadAllLines(filePath);



if (File.Exists(flightfilePath))
{
    if (flightDetails.Length > 0)
    {
        // Display the headers
        string[] header = flightDetails[0].Split(',');
Console.WriteLine("{0,-15} {1,-15} {2,-25} {3,-20}", header[0], header[1], header[2], header[3]);

        // Process and display each bus stop's data
        for (int i = 1; i<busDetails.Length; i++)
        {
            string[] data = busDetails[i].Split(',');
double distance = double.Parse(data[0]);
string code = data[1];
string road = data[2];
string description = data[3];

// Create BusStop object (assuming you have a BusStop class defined elsewhere)
BusStop busStop = new BusStop(distance, code, road, description);

// Display the details of each bus stop
Console.WriteLine("{0,-15} {1,-15} {2,-25} {3,-20}", busStop.Distance, busStop.Code, busStop.Road, busStop.Description);
        }
    }
    else
{
    Console.WriteLine("No data found in the file.");
}
}
else
{
    Console.WriteLine("File not found: " + filePath);
}







