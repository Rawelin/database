using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        private Utility utility;

        private List<Button> userButtonsList;                         // lista przycisków dostępnych dla użytkownika 
        private List<Button> adminButtonsList;                        // lista przycisków dostępnych dla administratora 


        public MainWindow()
        {
            InitializeComponent();
            adminButtons();                                            // inicjuje przyciski administratora
            userButtons();                                             // inicjuje przycisi użytkownika
            adminButtonsDisable();                                     // deaktywuje wszystkie przyciski
            loginEnable();                                             // aktywuje logowanie
        }

        public void StartConnection()
        {
            try
            {
                connection = new SqlConnection();                      // tworzy nowe polączenie SQL
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["wypozyczalnia"].ConnectionString;  // ścieżka do bazy zmajdująca się w App.config
                connection.Open();                                                       // otwiera polączenie

                refreshAllTAbles();                                                      // odświeża wszystkie widoki

                utility = new Utility(connection);

                string inq1 = "select * from samochody";
                string inq2 = "select * from klienci";
                string inq3 = "select * from pracownicy";

                utility.addItemsToComboBox(inq1, samochodyComboBox);                    // dodawanie pól z bazy do combobox
                utility.addItemsToComboBox(inq2, klienciComboBox);
                utility.addItemsToComboBox(inq3, pracownicyComboBox);

            }
            catch (Exception ex)                                                         // wyłapuje wyjątki kiedy coś pójdzie nie tak
            {
                MessageBox.Show("Connection Failure", ex.Message);                       // wyświetla messagebox na ekranie
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)                       // przycisk start w raportach
        {
              inquiry = inquiryTextBox.Text;                                             // przypisuje zapytanie z textbox do amiennej inquiry

              DataShow(inquiry, raportyGrid);                                            // funkcja wyświetla zapytanie na gridzie w raportach

         //  inquiry = "Select  klientID, count(klientID) as 'Wypożyczenia' from wypozyczenia group by KlientID";
         //  DataShow(inquiry, raportyGrid);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)            // przycisk exit w menu Plik
        {
            Application.Current.Shutdown();                                  //  zamyka całą aplikacje
        }

        private void Login_Click(object sender, RoutedEventArgs e)           // przycisk login w menu Plik
        {
            Login login = new Login(this);                                   //  Tworzenie obiektu klasy Login i pobranie głownego okna jako parametru
           // login.DataContext = this;
            login.ShowDialog();                                              //  Wyświetlenie formularza logowania 
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            userButtonsDisable();                                             // wyłącza przyciski użytkownika
            adminButtonsDisable();                                            // wyłącza przyciski administratora
            loginEnable();                                                    // uaktywnia logowanie
        }

        private void Panel_Click(object sender, RoutedEventArgs e)            // przycisk panel administratora w menu Opcje
        {
            Panel panel = new Panel();                                        // tworzy nowe okno klasy Panel
            panel.ShowDialog();                                               // otwiera okno Panel                                    
        }

        private void add_Click(object sender, RoutedEventArgs e)              // wspolna metoda dla przycisków dodających 
        {
            string name;
            string surname;
            string pesel;

            if (sender.Equals(addClient))                                     // wyłąpuje z której zakładki (TabItem) został naciśnięty przycisk
            {
                name = nameTextBoxC.Text;                                     // przypisuje zmiennej tekst z pola tekstowego w zakładce klienci
                surname = surnameTextBoxC.Text;
                pesel = peselTextBoxC.Text;

                inquiry = "insert into klienci values('" + name + "', '" + surname + "', '" + pesel + "')";  // zapytanie dodające klienta do bazy
                DataShow(inquiry, klienciGrid);                               // dodanie rekordu
                refreshAllTAbles();                                           // odświeża wszystkie widoki
            }
            else if (sender.Equals(addEmployee))                              // wyłąpuje z której zakładki (TabItem) został naciśnięty przycisk                
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

                // string samID = samochodyComboBox.Text;
                // string pracID = pracIDTextBox.Text;
                // string klientID = klientIDTextBox.Text;
                // string datawyp = dataWypozyczeniaDatePicker.Text;
                // string datazwr = dataZwrotuDatePicker.Text;
                // insert into  wypozyczenia values(1, 1, 1, '2018-3-25', '2018-3-28', 1200)          

                string path;
                string trimmedPath;

                path = samochodyComboBox.Text;                             // trimowanie scieżek  z combobox 
                trimmedPath = Utility.TrimPath(path);

                string samID = trimmedPath;

                path = pracownicyComboBox.Text;
                trimmedPath = Utility.TrimPath(path);

                string pracID = trimmedPath;

                path = klienciComboBox.Text;
                trimmedPath = Utility.TrimPath(path);

                string klientID = trimmedPath;

                string koszt = koszTextBox.Text;                              // koszt z pola tekstowego koszTextBox


                if((!string.IsNullOrEmpty(samID)) && (!string.IsNullOrEmpty(pracID)) && (!string.IsNullOrEmpty(klientID)) && (!string.IsNullOrEmpty(koszt)))
                {
                    inquiry = "insert into wypozyczenia values(" + samID + ", " + pracID + ", " + klientID + ", '" + datawyp + "', '" + datazwr + "', " + koszt + ")";

                    DataShow(inquiry, wypozyczeniGrid);

                    refreshAllTAbles();

                    MessageBox.Show("Dodano do bazy");
                }
                else
                {
                    MessageBox.Show("Pola nie zostały wypełnione");
                }

              
            }
        }

        private void edit_Click(object sender, RoutedEventArgs e)        // wspolna metoda dla przycisków edytujących 
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

        private void delete_Click(object sender, RoutedEventArgs e)            // wspolna metoda dla przycisków kasujących 
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

        private void dataPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)    // metoda sczytująca datę z datapicker
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

        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)     // wspólna metoda przypisująca dane do pól tekstowych 
        {                                                                                  // w zależności na który wiersz klikniemy
            if (sender.Equals(klienciGrid))
            {
                row = klienciGrid.SelectedItem as DataRowView;

                if (row != null)
                {
                    id = row.Row.ItemArray[0].ToString();

                    nameTextBoxC.Text = row.Row.ItemArray[1].ToString();
                    surnameTextBoxC.Text = row.Row.ItemArray[2].ToString();
                    peselTextBoxC.Text = row.Row.ItemArray[3].ToString();
                }
              
            }
            else if (sender.Equals(pracownicyGrid))
            {
                row = pracownicyGrid.SelectedItem as DataRowView;

                if (row != null)
                {
                    id = row.Row.ItemArray[0].ToString();

                    nameTextBoxE.Text = row.Row.ItemArray[1].ToString();
                    surnameTextBoxE.Text = row.Row.ItemArray[2].ToString();
                }
             
            }
            else if (sender.Equals(samochodyGrid))
            {
                row = samochodyGrid.SelectedItem as DataRowView;

                if (row != null)
                {
                    id = row.Row.ItemArray[0].ToString();

                    brandTextBoxS.Text = row.Row.ItemArray[1].ToString();
                    modelTextBoxS.Text = row.Row.ItemArray[2].ToString();
                    colorTextBoxS.Text = row.Row.ItemArray[3].ToString();
                }
           
            }
            else if (sender.Equals(wypozyczeniGrid))
            {
                row = wypozyczeniGrid.SelectedItem as DataRowView;
               // int noOfRows = wypozyczeniGrid.Items.Count;

                if(row != null)
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
                SqlCommand command = new SqlCommand();               // tworzy nowy rozkaz SQL

                command.CommandText = inquiry;                       // wczytuje zapytanie SQL
                command.Connection = connection;                     // ścieżka do bazy 

                // command.ExecuteScalar();

                SqlDataAdapter da = new SqlDataAdapter(command);     // pobiera zapytanie do adaptera
                DataTable dt = new DataTable("Car Rent");            // tworzy nową tabelę
                da.Fill(dt);                                         // pobiera tabelę do adaptera

                dataGrid.ItemsSource = dt.DefaultView;               // wyświetla dane na gridzie z tabeli dt
            }
            catch (Exception ex)                                     // wyłapuje wyjątki 
            {
                MessageBox.Show("Komunikat diagnostyczny do odczytywania błędów", ex.Message);    // wyświetla messagebox na ekran
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)    // funkcja zapobiegająca wprowadzaniu liter 
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void refreshAllTAbles()                                       // odświeża wszystkie tabele
        {
            inquiry = "Select * from klienci";                                // odświeżenie widoku klienci po dodaniu rekordu
            DataShow(inquiry, klienciGrid);

            inquiry = "Select * from pracownicy";                             // odświeżenie widoku pracownicy po dodaniu rekordu
            DataShow(inquiry, pracownicyGrid);

            inquiry = "Select * from samochody";                              // odświeżenie widoku samochody po dodaniu rekordu
            DataShow(inquiry, samochodyGrid);

            inquiry = "select wypozyczenia.wypID, klienci.imie, klienci.nazwisko, samochody.marka, samochody.model, pracownicy.pracID, pracownicy.imie as imie, pracownicy.nazwisko as nazwisko, wypozyczenia.datawyp, wypozyczenia.datazwr, wypozyczenia.koszt " +
                "from klienci, samochody, wypozyczenia, pracownicy " +
                "where wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID and wypozyczenia.pracID = pracownicy.pracID";
           // inquiry = "select wypozyczenia.wypID, klienci.klientID, pracownicy.pracID, samochody.marka, samochody.model,  wypozyczenia.datawyp, wypozyczenia.datazwr, wypozyczenia.koszt from klienci, samochody, wypozyczenia, pracownicy where wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID and wypozyczenia.pracID = pracownicy.pracID ";
            DataShow(inquiry, wypozyczeniGrid);                                // odświeżenie widoku wypożyczenia

            inquiry = "select wypozyczenia.wypID, klienci.imie, klienci.nazwisko, samochody.marka, samochody.model, wypozyczenia.datawyp, wypozyczenia.datazwr " +
                "from klienci, samochody, wypozyczenia " +
                "where wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID";
            DataShow(inquiry, historiaGrid);

        }

        private void userButtons()                              // funkcja dodaje przyciski do listy użytkownika
        {
            userButtonsList = new List<Button>();               // utworzenie nowej listy 

            userButtonsList.Add(Start);                         // dodawanie przyciskó do listy
            userButtonsList.Add(addClient);
            userButtonsList.Add(addEmployee);
            userButtonsList.Add(addCar);
            userButtonsList.Add(addOrder);
            userButtonsList.Add(topClients);
            userButtonsList.Add(topCars);
            userButtonsList.Add(topEmployee);
        }

        public void adminButtons()                              // funkcja dodaje przyciski do listy użytkownika
        {
            adminButtonsList = new List<Button>();              // utworzenie nowej listy 

            adminButtonsList.Add(Start);                        // dodawanie przyciskó do listy
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

        public void userButtonsDisable()                                    // funkcja ustawiająca przyciski użytkownika  na disable
        {
            foreach (Button bt in userButtonsList)
            {
                bt.IsEnabled = false;
            }
        }

        public void userButtonsEnable()                                     // funkcja ustawiająca przyciski użytkownika na enable
        {
            foreach (Button bt in userButtonsList)
            {
                bt.IsEnabled = true;
            }
        }

        public void adminButtonsDisable()                                    // funkcja ustawiająca przyciski administratora na disable
        {
            foreach (Button bt in adminButtonsList)
            {
                bt.IsEnabled = false;
            }
        }

        public void adminButtonsEnable()                                     // funkcja ustawiająca przyciski administratora na enable
        {
            foreach (Button bt in adminButtonsList)
            {
                bt.IsEnabled = true;
            }
        }
        public void loginEnable()                                            // funkcja ustawiająca przycisk login w menu Plik na enable
        {
            login.IsEnabled = true;
            logout.IsEnabled = false;
            Status.Content = "Nie polączony z bazą";
        }
        public void loginDisable()                                          // funkcja ustawiająca przycisk login w menu Plik na disable
        {
            login.IsEnabled = false;
            logout.IsEnabled = true;
        }
    }
}
