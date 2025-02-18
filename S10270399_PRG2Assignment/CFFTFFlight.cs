﻿//==========================================================
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
    class CFFTFFlight : Flight
    {
        public double RequestFee { get; set; }
        public CFFTFFlight() { }
        public CFFTFFlight(string flightNumber, string origin, string destination, DateTime expectedtime, string status, double requestFee) : base(flightNumber, origin, destination, expectedtime, status)
        {
            RequestFee = requestFee;
        }
        public override double CalculateFees()
        {
            // Base fee + CFFT special request fee
            double baseFee = Origin == "Singapore (SIN)" ? 800.00 : 500.00;
            return baseFee + 150.00 + RequestFee;
        }

        public override string ToString()
        {
            return $"CFFT {base.ToString()} - Request Fee: ${RequestFee:F2}";
        }
    }
}
