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
    class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }

        public BoardingGate() { }
        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
        }

        public double CalculateFees()
        {
            if (Flight == null) return 0;

            double baseFee = Flight.CalculateFees();
            int supportedTypes = (SupportsCFFT ? 1 : 0) + (SupportsDDJB ? 1 : 0) + (SupportsLWTT ? 1 : 0);

            // Add premium for gates that support multiple special request types
            return baseFee * (1 + (supportedTypes * 0.1));
        }

        public override string ToString()
        {
            return $"Boarding Gate Name: {GateName}\n" +
                   $"Supports DDJB: {SupportsDDJB}\n" +
                   $"Supports CFFT: {SupportsCFFT}\n" +
                   $"Supports LWTT: {SupportsLWTT}";
        }
    }
}
