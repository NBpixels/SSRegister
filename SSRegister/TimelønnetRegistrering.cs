using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSRegister
{
    public class TimelønnetRegistrering
    {
        public DateTime Dato { get; set; }
        public int Timer { get; set; }
        public string Jobbeskrivelse { get; set; }
        public string Produktionsordre { get; set; }

        public TimelønnetRegistrering(DateTime dato, int timer, string jobbeskrivelse, string produktionsordre)
        {
            Dato = dato;
            Timer = timer;
            Jobbeskrivelse = jobbeskrivelse;
            Produktionsordre = produktionsordre;
        }
    }
}
