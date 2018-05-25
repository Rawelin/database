using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;                       // for ConfigurationManger - add references in solution explorer
using System.Data;                                // for DataTable
using System.Text.RegularExpressions;

namespace Baza
{
   
    public partial class MainWindow : Window
    {
        private String inquiry = null;
        private String id = null;
        private SqlConnection connection;
        private DataRowView row;
        private DateTime datawyp;
        private DateTime datazwr;

        private List<Button> userButtonsList;
        private List<Button> adminButtonsList;


        public MainWindow()
        {
            InitializeComponent();
            adminButtons();                                            // Inicjuje przyciski administratora
            userButtons();                                             // Inicjuje przycisi użytkownika
            adminButtonsDisable();                                     // Deaktywuje wszystkie przyciski
            loginEnable();                                             // Aktywuje logowanie
        }

        public void StartConnection()
        {
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["wypozyczalnia"].ConnectionString;
                connection.Open();

                refreshAllTAbles();                                                      // odświeża wszystkie widoki
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Failure", ex.Message);
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
              inquiry = inquiryTextBox.Text;

              DataShow(inquiry, raportyGrid);

         //  inquiry = "Select  klientID, count(klientID) as 'Wypożyczenia' from wypozyczenia group by KlientID";
         //  DataShow(inquiry, raportyGrid);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();                                 //  zamyka całą aplikacje
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login(this);                                   //  Tworzenie obiektu klasy Login
           // login.DataContext = this;
            login.ShowDialog();                                              //  Wyświetlenie formularza logowania 
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            userButtonsDisable();                                             // wyłącza przyciski użytkownika
            adminButtonsDisable();                                            // wyłącza przyciski administratora
            loginEnable();                                                    // uaktywnia logowanie
        }

        private void Panel_Click(object sender, RoutedEventArgs e)
        {
            Panel panel = new Panel();
            panel.ShowDialog();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            string name;
            string surname;
            string pesel;

            if (sender.Equals(addClient))
            {
                name = nameTextBoxC.Text;
                surname = surnameTextBoxC.Text;
                pesel = peselTextBoxC.Text;

                inquiry = "insert into klienci values('" + name + "', '" + surname + "', '" + pesel + "')";
                DataShow(inquiry, klienciGrid);                                // dodanie rekordu
                refreshAllTAbles();                                            // odświeża wszystkie widoki
            }
            else if (sender.Equals(addEmployee))
            {
                name = nameTextBoxE.Text;
                surname = surnameTextBoxE.Text;

                inquiry = "insert into pracownicy values('" + name + "', '" + surname + "')";
                DataShow(inquiry, pracownicyGrid);                              // dodanie rekordu
                refreshAllTAbles();                                             // odświeża wszystkie widoki
            }
            else if (sender.Equals(addCar))
            {
                string brand = brandTextBoxS.Text;
                string model = modelTextBoxS.Text;
                string color = colorTextBoxS.Text;

                inquiry = "insert into samochody values('" + brand + "', '" + model + "', '" + color + "')";
                DataShow(inquiry, samochodyGrid);                              // dodanie rekordu
                refreshAllTAbles();                                            // odświeża wszystkie widoki
            }
            else if(sender.Equals(addOrder))
            {

                string samID = samIDTextBox.Text;
                string pracID = pracIDTextBox.Text;
                string klientID = klientIDTextBox.Text;
               // string datawyp = dataWypozyczeniaDatePicker.Text;
               // string datazwr = dataZwrotuDatePicker.Text;
                string koszt = koszTextBox.Text;

                // insert into  wypozyczenia values(1, 1, 1, '2018-3-25', '2018-3-28', 1200)

                inquiry = "insert into wypozyczenia values(" + samID + ", " + pracID + ", " + klientID + ", '" + datawyp + "', '" + datazwr + "', " + koszt + ")";

                DataShow(inquiry, wypozyczeniGrid);

                refreshAllTAbles();
            }
        }

        private void dataPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var picker = sender as DatePicker;
            DateTime? date = picker.SelectedDate;

            if (sender.Equals(dataWypozyczeniaDatePicker))
            {
                this.Title = date.Value.ToLongDateString();
                this.datawyp = (DateTime)date;
            }
            else if (sender.Equals(dataZwrotuDatePicker))
            {
                this.Title = date.Value.ToLongDateString();
                this.datazwr = (DateTime)date;
            }

        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            string name;
            string surname;
            string pesel;

            if (sender.Equals(editClient))
            {
                name = nameTextBoxC.Text;
                surname = surnameTextBoxC.Text;
                pesel = peselTextBoxC.Text;

                // update klienci set imie='Janina' where klientID=34

                inquiry = "update klienci set imie='" + name + "' where klientID=" + id + "";
                DataShow(inquiry, klienciGrid);

                inquiry = "update klienci set nazwisko='" + surname + "' where klientID=" + id + "";
                DataShow(inquiry, klienciGrid);

                inquiry = "update klienci set pesel='" + pesel + "' where klientID=" + id + "";
                DataShow(inquiry, klienciGrid);
                refreshAllTAbles();                                           // odświeża wszystkie widoki
            }
            else if(sender.Equals(editEmployee))
            {
                name = nameTextBoxE.Text;
                surname = surnameTextBoxE.Text;

                inquiry = "update pracownicy set imie='" + name + "' where pracID=" + id + "";
                DataShow(inquiry, pracownicyGrid);

                inquiry = "update pracownicy set nazwisko='" + surname + "' where pracID=" + id + "";
                DataShow(inquiry, pracownicyGrid);

                refreshAllTAbles();                                           // odświeża wszystkie widoki
            }
            else if(sender.Equals(editCar))
            {
                string brand = brandTextBoxS.Text;
                string model = modelTextBoxS.Text;
                string color = colorTextBoxS.Text;

                inquiry = "update samochody set marka='" + brand + "' where samID=" + id + "";
                DataShow(inquiry, samochodyGrid);

                inquiry = "update samochody set model='" + model + "' where samID=" + id + "";
                DataShow(inquiry, samochodyGrid);

                inquiry = "update samochody set kolor='" + color + "' where samID=" + id + "";
                DataShow(inquiry, samochodyGrid);

                refreshAllTAbles();                                             // odświeża wszystkie widoki
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {

            if (sender.Equals(deleteClient))
            {
                inquiry = "delete from klienci where klientID=" + id + "";
                DataShow(inquiry, klienciGrid);                                  // skasownie rekordu

                refreshAllTAbles();                                              // odświeża wszystkie widoki
            }
            else if (sender.Equals(deleteEmployee))
            {
                inquiry = "delete from pracownicy where pracID=" + id + "";
                DataShow(inquiry, pracownicyGrid);                                // skasownie rekordu
                refreshAllTAbles();                                               // odświeża wszystkie widoki
            }
            else if (sender.Equals(deleteCar))
            {
                inquiry = "delete from samochody where samID=" + id + "";
                DataShow(inquiry, samochodyGrid);                                  // skasownie rekordu
                refreshAllTAbles();                                                // odświeża wszystkie widoki
            }
            else if(sender.Equals(deleteOrder))
            {
                inquiry = "delete from wypozyczenia where wypID=" + id + "";
                DataShow(inquiry, wypozyczeniGrid);                                 // skasownie rekordu
                refreshAllTAbles();
            }
        }

        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender.Equals(klienciGrid))
            {
                row = klienciGrid.SelectedItem as DataRowView;

                id = row.Row.ItemArray[0].ToString();
                nameTextBoxC.Text = row.Row.ItemArray[1].ToString();
                surnameTextBoxC.Text = row.Row.ItemArray[2].ToString();
                peselTextBoxC.Text = row.Row.ItemArray[3].ToString();
            }
            else if (sender.Equals(pracownicyGrid))
            {
                row = pracownicyGrid.SelectedItem as DataRowView;

                id = row.Row.ItemArray[0].ToString();
                nameTextBoxE.Text = row.Row.ItemArray[1].ToString();
                surnameTextBoxE.Text = row.Row.ItemArray[2].ToString();
            }
            else if (sender.Equals(samochodyGrid))
            {
                row = samochodyGrid.SelectedItem as DataRowView;

                id = row.Row.ItemArray[0].ToString();
                brandTextBoxS.Text = row.Row.ItemArray[1].ToString();
                modelTextBoxS.Text = row.Row.ItemArray[2].ToString();
                colorTextBoxS.Text = row.Row.ItemArray[3].ToString();
            }
            else if (sender.Equals(wypozyczeniGrid))
            {
                row = wypozyczeniGrid.SelectedItem as DataRowView;

                id = row.Row.ItemArray[0].ToString();
            }
            else if (sender.Equals(historiaGrid))
            {

            }
        }

        private void topClient_Click(object sender, RoutedEventArgs e)
        {
            // inquiry = "Select klienci.imie, klienci.nazwisko, samochody.marka, wypozyczenia.wypID from klienci, samochody, wypozyczenia where wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID";
            // inquiry = "Select klienci.imie, klienci.nazwisko, samochody.marka, wypozyczenia.wypID from klienci, samochody, wypozyczenia where wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID";
            // inquiry = "Select  klientID, klienci.imie, count(klientID) as 'Wypożyczenia' from wypozyczenia, klienci, samochody where wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID group by KlientID";
            // inquiry = "Select  klientID, klienci.imie, count(klientID) as 'Wypożyczenia' from wypozyczenia, klienci, where wypozyczenia.klientID = klienci.klientID group by KlientID";
            inquiry = "Select  klientID, count(klientID) as 'Wypożyczenia' from wypozyczenia group by KlientID";
            DataShow(inquiry, raportyGrid);
        }

        // funkcje pomocnicze

        private void DataShow(string inquiry, DataGrid dataGrid)     // funkcja do wyświetlania zapytan 
        {                                                            // przyjmuje 2 parametry (zapytanie, siatka do wyświetlania danych)
            try
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = inquiry;
                command.Connection = connection;

                // command.ExecuteScalar();

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable("Car Rent");
                da.Fill(dt);

                dataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Komunikat diagnostyczny do odczytywania błędów", ex.Message);
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)    // funkcja zapobiegająca wprowadzaniu liter 
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void refreshAllTAbles()
        {
            inquiry = "Select * from klienci";                                // Odświeżenie widoku klienci po dodaniu rekordu
            DataShow(inquiry, klienciGrid);

            inquiry = "Select * from pracownicy";                             // Odświeżenie widoku pracownicy po dodaniu rekordu
            DataShow(inquiry, pracownicyGrid);

            inquiry = "Select * from samochody";                              // Odświeżenie widoku samochody po dodaniu rekordu
            DataShow(inquiry, samochodyGrid);

            // inquiry = "select klienci.imie, klienci.nazwisko, samochody.marka, wypozyczenia.wypID from klienci, samochody, wypozyczenia where wypozyczenia.klientID = 2 and wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID";
            inquiry = "select wypozyczenia.wypID, klienci.klientID, pracownicy.pracID, samochody.marka, samochody.model,  wypozyczenia.datawyp, wypozyczenia.datazwr, wypozyczenia.koszt from klienci, samochody, wypozyczenia, pracownicy where wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID and wypozyczenia.pracID = pracownicy.pracID ";
            DataShow(inquiry, wypozyczeniGrid);                                // Odświeżenie widoku wypożyczenia

            inquiry = "select wypozyczenia.wypID, klienci.imie, klienci.nazwisko, samochody.marka, samochody.model, wypozyczenia.datawyp, wypozyczenia.datazwr from klienci, samochody, wypozyczenia where wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID";
            DataShow(inquiry, historiaGrid);

        }

        private void userButtons()                              // funkcja dodaje przyciski do listy
        {
            userButtonsList = new List<Button>();

            userButtonsList.Add(Start);
            userButtonsList.Add(addClient);
            userButtonsList.Add(addEmployee);
            userButtonsList.Add(addCar);
            userButtonsList.Add(addOrder);
            userButtonsList.Add(topClients);
            userButtonsList.Add(topCars);
            userButtonsList.Add(topEmployee);
        }

        public void adminButtons()
        {
            adminButtonsList = new List<Button>();

            adminButtonsList.Add(Start);
            adminButtonsList.Add(addClient);
            adminButtonsList.Add(editClient);
            adminButtonsList.Add(deleteClient);
            adminButtonsList.Add(addEmployee);
            adminButtonsList.Add(editEmployee);
            adminButtonsList.Add(deleteEmployee);
            adminButtonsList.Add(addCar);
            adminButtonsList.Add(editCar);
            adminButtonsList.Add(deleteCar);
            adminButtonsList.Add(addOrder);
            adminButtonsList.Add(deleteOrder);
            adminButtonsList.Add(topClients);
            adminButtonsList.Add(topCars);
            adminButtonsList.Add(topEmployee);
        }

        public void userButtonsDisable()                                    // funkcja ustawiająca przyciski na disable
        {
            foreach (Button bt in userButtonsList)
            {
                bt.IsEnabled = false;
            }
        }

        public void userButtonsEnable()
        {
            foreach (Button bt in userButtonsList)
            {
                bt.IsEnabled = true;
            }
        }

        public void adminButtonsDisable()                                    // funkcja ustawiająca przyciski na disable
        {
            foreach (Button bt in adminButtonsList)
            {
                bt.IsEnabled = false;
            }
        }

        public void adminButtonsEnable()
        {
            foreach (Button bt in adminButtonsList)
            {
                bt.IsEnabled = true;
            }
        }
        public void loginEnable()
        {
            login.IsEnabled = true;
            logout.IsEnabled = false;
            Status.Content = "Nie polączony z bazą";
        }
        public void loginDisable()
        {
            login.IsEnabled = false;
            logout.IsEnabled = true;
        }
    }
}
