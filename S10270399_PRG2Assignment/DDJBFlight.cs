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
            //Calculate fees
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "RequestFee: " + RequestFee;
        }
    }
}
