using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10270399_PRG2Assignment
{
    class LWTTFlight : Flight
    {
        public double RequestFee { get; set; }
        public LWTTFlight() { }
        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedtime, string status, double requestFee) : base(flightNumber, origin, destination, expectedtime, status)
        {
    
            RequestFee = requestFee;
        }

        public override double CalculateFees()
        {
            return 100.00 + RequestFee; // Base fee + special request fee
        }

        public override string ToString()
        {
            return $"LWTT {base.ToString()} - Request Fee: ${RequestFee:F2}";
        }
    }
}
