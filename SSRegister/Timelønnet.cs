using System;

namespace SSRegister
{
    public class Timelønnet
    {
        public bool LoggedIn { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public string LoginName { get; set; }

        public Timelønnet(string name, string jobTitle, string loginName)
        {
            Name = name;
            JobTitle = jobTitle;
            LoginName = loginName;
            LoggedIn = false;
        }

        public bool IsValidName(string loginName)
        {
            // Forenklet logik til at validere loginName. Du kan ændre det til en database eller en liste.
            return loginName.Equals(LoginName, StringComparison.OrdinalIgnoreCase);
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

