//==========================================================
// Student1 Number	: S10267626
// Student1 Name	: Aiden Tan Yihan
// Student2 Number  : S10270399
// Partner2 Name	: Ang Shun Xiang
//==========================================================
namespace S10270399_PRG2Assignment
{
    public abstract class Flight
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime Expectedtime { get; set; }
        public string Status { get; set; }

        public Flight() { }

        public Flight(string flightNumber, string origin, string destination, DateTime expectedtime, string status)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            Expectedtime = expectedtime;
            Status = status;
        }

        public virtual double GetFees()
        {
            return 0;
        }

        public override string ToString()
        {
            return FlightNumber;
        }
    }
}
