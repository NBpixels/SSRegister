using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SSRegister
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TEST
        public Funktionær Funktionær;
        public Timelønnet Timelønnet;
        public Stack<UIElement> pageHistory = new Stack<UIElement>();
        public FunktionærRegistrering FunktionærRegistrering;
        public MainWindow()
        {
            InitializeComponent();

            Funktionær = new Funktionær("John Johnsen", "Funktionær", "email@email.dk", 10);
            Timelønnet = new Timelønnet("Hans Hansen", "Timelønnet", "hans hansen");

            InitializeComponent();
            pageHistory = new Stack<UIElement>();
        }

        public void ShowPage(UIElement currentPage, UIElement nextPage)
        {
            // Tilføj kun til historik, hvis vi skifter side
            if (currentPage != nextPage)
            {
                if (pageHistory.Count == 0 || pageHistory.Peek() != currentPage)
                {
                    pageHistory.Push(currentPage);
                }

                // Skjul den aktuelle side
                currentPage.Visibility = Visibility.Collapsed;

                // Vis den næste side
                nextPage.Visibility = Visibility.Visible;
            }
        }

        public void RegChoiceLabels()
        {
            if (Funktionær.LoggedIn)
            {
                JobTitle.Content = "Funktionær";
                WorkerName.Content = Funktionær.Name;
            }

            else if (Timelønnet.LoggedIn)
            {
                JobTitle.Content = "Timelønnet";
                WorkerName.Content = Timelønnet.Name;
            }
        }

        public void RegLabels()
        {
            if (Funktionær.LoggedIn)
            {
                DrivingRegJobtitle.Content = "Funktionær kørselsregistrering";
                DrivingRegWorkerName.Content = Funktionær.Name;
                DrivingRegSchemeJobtitle.Content = "Funktionær kørselsregistrering";
                DrivingRegSchemeWorkerName.Content = Funktionær.Name;

                SalariedNameAndVacationDays.Content = Funktionær.Name + ", du har lige nu " + Funktionær.FeriedageTilbage + " feriedage tilbage";
                SalariedRegSchemeName.Content = Funktionær.Name + ", du har lige nu " + Funktionær.FeriedageTilbage + " feriedage tilbage";
            }

            else if (Timelønnet.LoggedIn)
            {
                DrivingRegJobtitle.Content = "Timelønnet kørselsregistrering";
                DrivingRegWorkerName.Content = Timelønnet.Name;
                DrivingRegSchemeJobtitle.Content = "Timelønnet kørselsregistrering";
                DrivingRegSchemeWorkerName.Content = Timelønnet.Name;

                HourlyRegWorkerName.Content = Timelønnet.Name;
                HourlyRegSchemeWorkerName.Content = Timelønnet.Name;
            }
        }

        //Kørselsregistrering
        public void DrivingFillInRegScheme(Kørsel kørsel)
        {
            if (kørsel == null)
            {
                return;
            }

            DateTextBox.Text = kørsel.Dato.ToString("dd/MM/yyyy");
            DestinationTextBox.Text = kørsel.Destination;
            ErrindTextBox.Text = kørsel.Ærinde;
            KmTextBox.Text = kørsel.Km.ToString();
        }

        public void DateTextBox_GotFocus(object sender, EventArgs e)
        {
            if (DateTextBox.Text == "dd/mm/yy")
            {
                DateTextBox.Text = string.Empty;
                DateTextBox.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        public void DateTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DateTextBox.Text))
            {
                DateTextBox.Text = "dd/mm/yy";
                DateTextBox.Foreground = new SolidColorBrush(Color.FromRgb(160, 163, 165));
            }
        }

        // Funktionær tidsregistrering
        public void SalariedTimeAddBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(SalariedTimeReg, SalariedRegScheme);
        }

        public void SalariedEditBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(SalariedTimeReg, SalariedRegScheme);
        }

        public void SalariedDeleteBtn_Click(object sender, RoutedEventArgs e) 
        {
            ShowPage(SalariedTimeReg, SalariedRegScheme);
        }

        public void SaveBtnSalaried_Click(object sender, RoutedEventArgs e)
        {
            int ugeNr;
            int timer;
            int feriedageBrugt;
            int afspadsering;
            int sygedage;

            if (!int.TryParse(SalariedRegWeek.Text, out ugeNr) || !int.TryParse(SalariedRegHours.Text, out timer))
            {
                MessageBox.Show("Uge nr. og timer skal være tal.");
                return;
            }

            // Behandling af optional felter
            feriedageBrugt = string.IsNullOrWhiteSpace(SalariedRegVacation.Text) ? 0 : (int.TryParse(SalariedRegVacation.Text, out int tempFeriedage) ? tempFeriedage : 0);
            afspadsering = string.IsNullOrWhiteSpace(SalariedRegDaysOff.Text) ? 0 : (int.TryParse(SalariedRegDaysOff.Text, out int tempAfspadsering) ? tempAfspadsering : 0);
            sygedage = string.IsNullOrWhiteSpace(SalariedRegSickDays.Text) ? 0 : (int.TryParse(SalariedRegSickDays.Text, out int tempSygedage) ? tempSygedage : 0);

            FunktionærRegistrering registrering = new FunktionærRegistrering(ugeNr, timer, feriedageBrugt, afspadsering, sygedage);

            // Gem registreringen til CSV
            SaveToCSV(registrering);

            // Vis den næste side (om nødvendigt)
            ShowPage(SalariedRegScheme, SalariedTimeReg);
        }


        public void SaveToCSV(FunktionærRegistrering registrering)
        {
            string userFileName = Funktionær.Name.Replace(" ", "_");
            string filePath = $@"C:\Users\nadia\OneDrive\Skrivebord\Datamatiker\Afleveringer\Getting Real\SSRegisterKopi\{userFileName}_tidsregistrering.csv";

            if (!File.Exists(filePath))
            {
                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    sw.WriteLine("UgeNr, Timer, FeriedageTilbage, FeriedageBrugt, Afspadsering, Sygedage");
                }
            }

            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(registrering.ToCsvLine());
            }
        }

        //Nav knapper
        public void AddDrivingBtn_Click(object sender, EventArgs e)
        {
            ShowPage(DrivingRegistration, DrivingRegScheme);
        }

        public void SaveBtn_Click(object sender, EventArgs e)
        {
            ShowPage(DrivingRegScheme, DrivingReg);
        }

        public void SalariedBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(Frontpage, SalariedLogin);
        }

        public void HourlyBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(Frontpage, HourlyLogin);
        }

        public void SalariedLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string enteredEmail = EmailTextBox.Text;

            if (Funktionær.IsValidEmail(enteredEmail))
            {
                Funktionær.Login();
                RegChoiceLabels();
                RegLabels();
                ShowPage(SalariedLogin, RegChoice);
            }
            else
            {
                EmailInvalid.Visibility = Visibility.Visible;
            }
        }

        public void HourlyLoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string enteredLoginName = NameTextBox.Text;

            if (Timelønnet.IsValidName(enteredLoginName))
            {
                Timelønnet.Login();
                RegChoiceLabels();
                RegLabels();
                ShowPage(HourlyLogin, RegChoice);
            }
            else
            {
                NameInvalid.Visibility = Visibility.Visible;
            }
        }

        public void DrivingRegBtn_Click(object sender, RoutedEventArgs e)
        {
            ShowPage(RegChoice, DrivingRegistration);
        }

        public void TimeRegBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Funktionær.LoggedIn)
            {
                ShowPage(RegChoice, SalariedTimeReg);
            }

            else if (Timelønnet.LoggedIn)
            {
                ShowPage(RegChoice, HourlyRegistration);
            }
        }

        public void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (pageHistory.Count > 0)
            {
                // Skjul den nuværende synlige side
                foreach (UIElement page in MainGrid.Children)
                {
                    if (page.Visibility == Visibility.Visible)
                    {
                        page.Visibility = Visibility.Collapsed;
                        break;
                    }
                }

                // Gør den forrige side synlig
                UIElement previousPage = pageHistory.Pop();
                previousPage.Visibility = Visibility.Visible;
            }
            else
            {
                // Ingen flere sider i historikken
                MessageBox.Show("Du er allerede på den første side.");
            }
        }

        public void RegBtn_Click(Object sender, RoutedEventArgs e)
        {
            // Find den nuværende synlige side og skift til RegChoice
            foreach (UIElement page in MainGrid.Children)
            {
                if (page.Visibility == Visibility.Visible)
                {
                    ShowPage(page, RegChoice);  // Skift fra den synlige side til RegChoice
                    break;
                }
            }
        }

        public void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            // Log ud afhængigt af hvilken bruger der er logget ind
            if (Funktionær.LoggedIn)
            {
                Funktionær.Logout();
            }
            else if (Timelønnet.LoggedIn)
            {
                Timelønnet.Logout();
            }

            // Opdater UI'et for at reflektere log ud-processen
            ShowPage(RegChoice, Frontpage); // Vis forsiden igen
            ResetUI(); // Resæt UI'ets state
        }

        public void ResetUI()
        {
            // Skjul de sider der ikke skal være synlige
            SalariedLogin.Visibility = Visibility.Collapsed;
            HourlyLogin.Visibility = Visibility.Collapsed;
            RegChoice.Visibility = Visibility.Collapsed;
            DrivingRegistration.Visibility = Visibility.Collapsed;
            EmailInvalid.Visibility = Visibility.Collapsed;
            NameInvalid.Visibility = Visibility.Collapsed;

            // Vis forsiden eller login siden
            Frontpage.Visibility = Visibility.Visible;
        }
    }
}