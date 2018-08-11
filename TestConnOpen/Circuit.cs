using System;
using System.Collections.Generic;
using System.Text;

namespace TestConnOpen
{
    public class Circuit
    {

        public int Id { get; set; }
        public string Vendor { get; set; }
        // Region new with May2018
        public string Region { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        // LAT and LON new with May2018
        // public string LAT { get; set; }
        // public string LON { get; set; }

        public string Interface { get; set; }

        public string Speed { get; set; }
        public string MRC { get; set; }
        public string NRC { get; set; }
        public string Term { get; set; }

        public override string ToString()
        {
            return $"{Address} {City}";
        }
    }
}
