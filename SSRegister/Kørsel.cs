using System;

namespace SSRegister
{
    public class Kørsel
    {
        public DateTime Dato { get; set; }
        public string Destination { get; set; }
        public string Ærinde { get; set; }
        public int Km { get; set; }

        public Kørsel(DateTime dato, string destination, string ærinde, int km)
        {
            Dato = dato;
            Destination = destination;
            Ærinde = ærinde;
            Km = km;
        }
    }
}
