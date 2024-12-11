using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRegister
{
    public abstract class Medarbejder
    {
        public string Navn { get; set; }
        public string Stilling { get; set; }

        public Medarbejder(string navn, string stilling)
        {
            Navn = navn;
            Stilling = stilling;
        }
    }

}
