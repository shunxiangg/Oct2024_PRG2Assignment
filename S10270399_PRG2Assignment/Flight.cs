using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10270399_PRG2Assignment
{
    abstract class Flight //: IComparable<Flight>
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




        public abstract double CalculateFees();

        //public override string ToString()
        //{
        //    return "FlightNumber" + FlightNumber +
        //        "Origin" + Origin +
        //        "Destination" + Destination +
        //        "ExpectedTime" + ExpectedTime +
        //        "Status" + Status;
        //}
        public override string ToString()
        {
            return $"Flight {FlightNumber} from {Origin} to {Destination} at {ExpectedTime:g} - Status: {Status}";
        }


        //public int CompareTo(Flight other)
        //{
        //    if (other == null) return 1;
        //    return string.Compare(this.FlightNumber, other.FlightNumber, StringComparison.Ordinal);
        //}
    }
}
