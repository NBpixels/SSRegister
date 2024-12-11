using System;
using System.Collections.Generic;
using System.Text;

namespace SSRegister
{
    public class FunktionærRegistrering
    {
        public int UgeNr { get; set; }
        public int Timer { get; set; }
        public int FeriedageBrugt { get; set; }
        public int Afspadsering { get; set; }
        public int Sygedage { get; set; }

        // En statisk liste til at gemme alle registreringer
        private static List<FunktionærRegistrering> allRegistrations = new List<FunktionærRegistrering>();

        public FunktionærRegistrering(int ugeNr, int timer, int feriedageBrugt, int afspadsering, int sygedage)
        {
            UgeNr = ugeNr;
            Timer = timer;
            FeriedageBrugt = feriedageBrugt;
            Afspadsering = afspadsering;
            Sygedage = sygedage;
        }

        // Metode til at konvertere registreringen til CSV-format
        public string ToCsvLine()
        {
            return $"{UgeNr}, {Timer}, {FeriedageBrugt}, {Afspadsering}, {Sygedage}";
        }

        // Metode til at tilføje registreringen til den statiske liste
        public static void AddRegistration(FunktionærRegistrering registration)
        {
            allRegistrations.Add(registration);
        }

        // Metode til at hente alle registreringer som CSV-format
        public static string GetAllRegistrationsAsCsv()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UgeNr, Timer, FeriedageBrugt, Afspadsering, Sygedage");
            foreach (var registration in allRegistrations)
            {
                sb.AppendLine(registration.ToCsvLine());
            }
            return sb.ToString();
        }
    }
}
