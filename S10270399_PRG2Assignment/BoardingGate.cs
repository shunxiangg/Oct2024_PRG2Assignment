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
            if (CurrentFlight == null) return 0;

            double baseFee = CurrentFlight.CalculateFees();
            int supportedTypes = (SupportsCFFT ? 1 : 0) + (SupportsDDJB ? 1 : 0) + (SupportsLWTT ? 1 : 0);

            // Add premium for gates that support multiple special request types
            return baseFee * (1 + (supportedTypes * 0.1));
        }

        public override string ToString()
        {
            return $"Gate {GateName} - Supports: " +
                   $"{(SupportsCFFT ? "CFFT " : "")}" +
                   $"{(SupportsDDJB ? "DDJB " : "")}" +
                   $"{(SupportsLWTT ? "LWTT" : "")}".Trim();
        }
    }
}
