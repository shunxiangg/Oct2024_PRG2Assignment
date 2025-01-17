using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10270399_PRG2Assignment
{
    class DDJBFlight : Flight
    {
        public double RequestFee { get; set; }
        public DDJBFlight() { }
        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedtime, string status, double requestFee) : base(flightNumber, origin, destination, expectedtime, status)
        {
            RequestFee = requestFee;
        }

        public override double CalculateFees()
        {
            // Base fee + DDJB special request fee
            double baseFee = Origin == "Singapore (SIN)" ? 800.00 : 500.00;
            return baseFee + 300.00 + RequestFee;
        }

        public override string ToString()
        {
            return $"DDJB {base.ToString()} - Request Fee: ${RequestFee:F2}";
        }

    }
}
