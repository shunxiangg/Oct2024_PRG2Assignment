﻿using System;
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
            {
                // Base fee for normal flights
                return Origin == "Singapore (SIN)" ? 800.00 : 500.00;
            }
        }

        public override string ToString()
        {
            return $"NORM {base.ToString()}";
        }


    }
}
