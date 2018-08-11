using System;

namespace CircuitLib
{
    public class Circuit
    {

        public int Id { get; set; }
        public string Vendor { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
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
