using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Configuration;                       // for ConfigurationManger - add references in solution explorer
using System.Data;                                // for DataTable
using System.Text.RegularExpressions;
using System.Diagnostics;
using Baza.Connection;

namespace Baza
{
    public partial class MainWindow : Window
    {
        private string inquiry = null;
        private string id = null;
        private string samID = null;
        private int pracIndex;
        private SqlConnection connection;
        private DataRowView row;
        private DateTime datawyp;
        private DateTime datazwr;
        private Utility utility;
        private List<Button> userButtonsList;                       
        private List<Button> adminButtonsList;                       
        private SetConnection setConnection;
        private DataFill dataFill;
        private InsertCommand insertCommand;
        private UpdateCommand updateCommand;
        private DeleteCommand deleteCommand;
        private UpdateComboBox updateComboBox;
        private SelectCommand selectCommand;

        public MainWindow()
        {
            InitializeComponent();
            adminButtons();                                            // inicjuje przyciski administratora
            userButtons();                                             // inicjuje przycisi użytkownika
            adminButtonsDisable();                                     // deaktywuje wszystkie przyciski
            loginEnable();                                             // aktywuje logowanie

            dataFill = new DataFill();
            setConnection = new SetConnection();
            insertCommand = new InsertCommand();
            updateCommand = new UpdateCommand();
            deleteCommand = new DeleteCommand();
            selectCommand = new SelectCommand();
        }

        public void StartConnection()
        {
            connection = setConnection.GetConnection();                              // otwiera polączenie
            refreshAllTAbles();                                                      // odświeża wszystkie widoki
            utility = new Utility(connection);
            updateComboBox = new UpdateComboBox(connection);
            updateComboBox.UpdateSamochody(samochodyComboBox);
            updateComboBox.UpdateClienci(klienciComboBox);
        }

        private void Start_Click(object sender, RoutedEventArgs e)                  // przycisk start w raportach
        {
            inquiry = inquiryTextBox.Text;                                             // przypisuje zapytanie z textbox do amiennej inquiry
            dataFill.DataShow(inquiry, raportyGrid, connection);                   // funkcja wyświetla zapytanie na gridzie w raportach
        }

        private void Exit_Click(object sender, RoutedEventArgs e)            // przycisk exit w menu Plik
        {
            Application.Current.Shutdown();                                  //  zamyka całą aplikacje
        }

        private void Login_Click(object sender, RoutedEventArgs e)           // przycisk login w menu Plik
        {
            Login login = new Login(this);                                   //  Tworzenie obiektu klasy Login i pobranie głownego okna jako parametru
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
            Panel panel = new Panel(this, connection);                        // tworzy nowe okno klasy Panel i pobiera aktualne połączenie z bazą
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

                List<string> lista = new List<string>() { name, surname, pesel};

                insertCommand.InsertKlient(klienciGrid, lista, connection);
                refreshAllTAbles();                                           // odświeża wszystkie widoki
            }
            else if (sender.Equals(addEmployee))                              // wyłąpuje z której zakładki (TabItem) został naciśnięty przycisk                
            {
                name = nameTextBoxE.Text;
                surname = surnameTextBoxE.Text;

                List<string> lista = new List<string>() { name, surname};

                insertCommand.InsertEmployee(pracownicyGrid, lista, connection);
                refreshAllTAbles();                                             // odświeża wszystkie widoki
            }
            else if (sender.Equals(addCar))
            {
                string marka = brandTextBoxS.Text;
                string model = modelTextBoxS.Text;
                string kolor = colorTextBoxS.Text;
                string cenaDoby = cenaTextBoxS.Text;
                string zajety = "false";

                List<string> lista = new List<string>() {marka, model, kolor, cenaDoby, zajety };
             
                insertCommand.InsertCar(samochodyGrid, lista, connection);
                updateComboBox.UpdateSamochody(samochodyComboBox);
                refreshAllTAbles();                                             // odświeża wszystkie widoki
            }
            else if (sender.Equals(addOrder))
            {
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

                List<string> lista = new List<string>() { samID, pracID, klientID, datawyp.ToString(), datazwr.ToString(), koszt };


                if ((!string.IsNullOrEmpty(samID)) && (!string.IsNullOrEmpty(pracID)) && (!string.IsNullOrEmpty(klientID)) && (!string.IsNullOrEmpty(koszt)))
                {
                    insertCommand.InsertWypozyczenia(wypozyczeniGrid, lista, connection);
                    updateCommand.UpdateCarBooked(samochodyGrid, true, samID, connection);
                    updateComboBox.UpdateSamochody(samochodyComboBox);
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

                List<string> lista = new List<string>() { name, surname, pesel, id};

                updateCommand.UpdateClient(klienciGrid, lista, connection);
                refreshAllTAbles();                                           // odświeża wszystkie widoki
            }
            else if (sender.Equals(editEmployee))
            {
                name = nameTextBoxE.Text;
                surname = surnameTextBoxE.Text;


                if (row != null)
                {
                    List<string> lista = new List<string>() { name, surname, id };

                    updateCommand.UpdateEmployee(pracownicyGrid, lista, connection);
                }

                List<User> usersList = new List<User>();                    // tymczasowa lista użytkowników
                usersList = Serialization.LoadUserFromFile(@"user.xml");    // lista zapisanych użytkowników z pliku
                usersList[pracIndex].Name = name;
                usersList[pracIndex].Surname = surname;

                Serialization.SaveUserListToFile(usersList, @"user.xml");   // zapisanie zmienionej listy do pliku
                MessageBox.Show(pracIndex.ToString());

                refreshAllTAbles();                                          // odświeża wszystkie widoki
            }
            else if (sender.Equals(editCar))
            {
                string marka = brandTextBoxS.Text;
                string model = modelTextBoxS.Text;
                string kolor = colorTextBoxS.Text;
                string cena = cenaTextBoxS.Text;

                List<string> lista = new List<string>() { marka, model, kolor, cena, id };

                updateCommand.UpdateCar(samochodyGrid, lista, connection);
                refreshAllTAbles();                                             // odświeża wszystkie widoki
            }
            else if (sender.Equals(releaseOrder))
            {
                updateCommand.UpdateCarBooked(samochodyGrid, false, samID, connection);
                updateComboBox.UpdateSamochody(samochodyComboBox);
                refreshAllTAbles();                                             // odświeża wszystkie widoki
            }
            else if (sender.Equals(editOrder))
            {
                string dataWyporzyczenia = dataWypozyczeniaDatePicker.Text;
                string dataZwrotu = dataZwrotuDatePicker.Text;
                string koszt = koszTextBox.Text;
            
                List<string> lista = new List<string>() { dataWyporzyczenia, dataZwrotu, koszt, id};

                updateCommand.UpdateWypozyczenia(wypozyczeniGrid, lista, connection);
                refreshAllTAbles();
            }
        }

        private void delete_Click(object sender, RoutedEventArgs e)            // wspolna metoda dla przycisków kasujących 
        {

            if (sender.Equals(deleteClient))
            {
                deleteCommand.DeleteClient(klienciGrid, id, connection);
                deleteCommand.DeleteWypozyczenia(klienciGrid, id, connection);
                refreshAllTAbles();                                              // odświeża wszystkie widoki
            }
            else if (sender.Equals(deleteEmployee))
            {    
                deleteCommand.DeletePracownik(pracownicyGrid, id, connection);
                refreshAllTAbles();                                               // odświeża wszystkie widoki
            }
            else if (sender.Equals(deleteCar))
            {
            
                deleteCommand.DeleteCar(samochodyGrid, id, connection);
                updateComboBox.UpdateSamochody(samochodyComboBox);
                refreshAllTAbles();                                               // odświeża wszystkie widoki
            }
            else if (sender.Equals(deleteOrder))
            {
                deleteCommand.DeleteWypozyczenia(wypozyczeniGrid, id, connection);
                updateCommand.UpdateCarBooked(samochodyGrid, false, samID, connection); 
                updateComboBox.UpdateSamochody(samochodyComboBox);
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
                    pracIndex = pracownicyGrid.SelectedIndex;
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
                    cenaTextBoxS.Text = row.Row.ItemArray[5].ToString();
                }
            }
            else if (sender.Equals(wypozyczeniGrid))
            {
                row = wypozyczeniGrid.SelectedItem as DataRowView;

                if (row != null)
                {
                    id = row.Row.ItemArray[0].ToString();
                    samID = row.Row.ItemArray[3].ToString();
                    dataWypozyczeniaDatePicker.Text = row.Row.ItemArray[11].ToString();
                    dataZwrotuDatePicker.Text = row.Row.ItemArray[12].ToString();
                    koszTextBox.Text = row.Row.ItemArray[13].ToString();
                }
            }
            else if (sender.Equals(historiaGrid)) { }                  
        }

        private void topClient_Click(object sender, RoutedEventArgs e)
        {
            selectCommand.SelectTop(raportyGrid, 1, connection);

        }

        private void topCars_Click(object sender, RoutedEventArgs e)
        {
            selectCommand.SelectTop(raportyGrid, 2, connection);
        }

        private void topEmployees_Click(object sender, RoutedEventArgs e)
        {
            selectCommand.SelectTop(raportyGrid, 3, connection);
        }

        // funkcje pomocnicze

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)    // funkcja zapobiegająca wprowadzaniu liter 
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void refreshAllTAbles()                                       // odświeża wszystkie tabele
        {
            selectCommand.SelectAll(klienciGrid, 1, connection);
            selectCommand.SelectAll(pracownicyGrid, 2, connection);
            selectCommand.SelectAll(samochodyGrid, 3, connection);       
            selectCommand.SelectAll(wypozyczeniGrid, 4, connection);         
            selectCommand.SelectAll(historiaGrid, 5, connection);
        }

        private void userButtons()                              // funkcja dodaje przyciski do listy użytkownika
        {
            userButtonsList = new List<Button>();               // utworzenie nowej listy 

            userButtonsList.Add(Start);                         // dodawanie przyciskó do listy
            userButtonsList.Add(addClient);
            userButtonsList.Add(addOrder);
            userButtonsList.Add(topClients);
            userButtonsList.Add(topCars);
            userButtonsList.Add(topEmployee);
            userButtonsList.Add(printOrder);
            userButtonsList.Add(printRaport);
            userButtonsList.Add(releaseOrder);
            userButtonsList.Add(editOrder);
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
            //  adminButtonsList.Add(deleteEmployee);
            adminButtonsList.Add(addCar);
            adminButtonsList.Add(editCar);
            adminButtonsList.Add(deleteCar);
            adminButtonsList.Add(addOrder);
            adminButtonsList.Add(deleteOrder);
            adminButtonsList.Add(topClients);
            adminButtonsList.Add(topCars);
            adminButtonsList.Add(topEmployee);
            adminButtonsList.Add(printOrder);
            adminButtonsList.Add(printRaport);
            adminButtonsList.Add(releaseOrder);
            adminButtonsList.Add(editOrder);

            deleteEmployee.IsEnabled = false;
            addEmployee.Visibility = Visibility.Hidden;
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
            adminPanel.IsEnabled = false;
        }

        public void adminButtonsEnable()                                     // funkcja ustawiająca przyciski administratora na enable
        {
            foreach (Button bt in adminButtonsList)
            {
                bt.IsEnabled = true;
            }
            adminPanel.IsEnabled = true;
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

        private void print_Click(object sender, RoutedEventArgs e)
        {
          Utility.Print(sender, e, raportyGrid, wypozyczeniGrid, printRaport, printOrder);
        }
    }
}
