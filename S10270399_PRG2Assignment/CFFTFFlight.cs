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
            throw new NotImplementedException();
        }



        public override string ToString()
        {
            return "RequestFee: " + RequestFee;
        }
    }
}
