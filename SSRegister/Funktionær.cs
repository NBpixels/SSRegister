using System;

namespace SSRegister
{
    public class Funktionær
    {
        public bool LoggedIn { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string Email { get; set; }
        public int FeriedageTilbage { get; set; }

        public Funktionær(string name, string jobTitle, string email, int feriedageTilbage)
        {
            Name = name;
            JobTitle = jobTitle;
            Email = email;
            LoggedIn = false;
            FeriedageTilbage = feriedageTilbage;
        }

        public bool IsValidEmail(string email)
        {
            // I en rigtig applikation kunne du validere email med regulære udtryk.
            return email == Email; // Forenklet for eksempel.
        }

        public void Login()
        {
            LoggedIn = true;
        }

        public void Logout()
        {
            LoggedIn = false;
        }
    }
}

