using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10270399_PRG2Assignment
{
    class NORMFlight : Flight
    {
        public NORMFlight() { }
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedtime, string status) : base(flightNumber, origin, destination, expectedtime, status)
        {
        }

        public override double CalculateFees()
        {
            //Calculate fees
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return FlightNumber;
        }
    
    
    }
}
