using System;
using System.Collections.Generic;
using System.Text;

namespace TestConnOpen
{
    public class Record
    {

        public string Vendor { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string InFranchise { get; set; }
        public string Tier { get; set; }


        //public override string ToString()
        //{
        //    return $"{Address} {City}";
        //}
    }
}
