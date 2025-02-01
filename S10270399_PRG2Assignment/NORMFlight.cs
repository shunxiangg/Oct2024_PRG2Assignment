//==========================================================
// Student1 Number	: S10267626
// Student1 Name	: Aiden Tan Yihan
// Student2 Number  : S10270399
// Partner2 Name	: Ang Shun Xiang
//==========================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10270399_PRG2Assignment
{
    public class NORMFlight : Flight
    {
        public NORMFlight() { }

        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedtime, string status)
            : base(flightNumber, origin, destination, expectedtime, status)
        {
        }

        public override double CalculateFees()
        {
            return 0;  // No additional fees for normal flights
        }
    }
}