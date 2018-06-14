using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace Baza
{
    public partial class Panel : Window
    {
        MainWindow mainWindow;
        private SqlConnection connection;                             // ścieżka do bazy
        private List<User> usersList;                                 // lista użytkowników
        private List<User> adminList;                                 // lista administratorów
        private DataRowView row;
        private String id = null;
        private int selectedIndex;

        public Panel(MainWindow mainWindow, SqlConnection connection)
        {
            this.mainWindow = mainWindow;
            this.connection = connection;
            InitializeComponent();
            Initialize();
        }

        private void AdminLaogin_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < adminList.Count(); i++)
            {
                if (this.loginAdminTextBox.Text.Equals(adminList[i].Name) && passwordAdminBox.Password.ToString().Equals(adminList[i].Password))  // walidacja loginu i hasła
                {
                    changePasswordButton.IsEnabled = true;                      // aktywuje przycisk dodaj
                    removeUserButton.IsEnabled = true;                          // aktywuje przycisk usuń
                    daneAdminButton.IsEnabled = true;                           // aktywuje przycisk hasło admin
                    errorMessageLabel.Content = "";                             // czyści etykiete errorMessageLabel
                }
                else
                {
                    errorMessageLabel.Content = "Podano zły login lub hasło";    // wysyła wiadomość do errorMessageLabel
                }
            }

            loginAdminTextBox.Text = "";
            passwordAdminBox.Password = "";  
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)            // metoda dla przycisku addUser
        {
            string name = nameTextBox.Text;                                     // zapis do zmiennych z pól tekstowych i password
            string surname =surnameTextBox.Text;
            string password = passwordBox.Password.ToString();
            string repeatPassword = repeatPasswordBox.Password.ToString();
            string inquiry;

            if (password.Equals(repeatPassword))                                 // walidacja hasła
            {
                errorMessageLabel.Content = "Hasło poprawne";

                inquiry = "insert into pracownicy values('" + name + "', '" + surname + "')";
                DataShow(inquiry, pracownicyGrid);

                nameTextBox.Text = "";                                        // Czyszczenie pól 
                surnameTextBox.Text = "";
                passwordBox.Password = "";
                repeatPasswordBox.Password = "";

                inquiry = "Select * from pracownicy";                          // odświeżenie widoku pracownicy po dodaniu rekordu
                DataShow(inquiry, pracownicyGrid);                             // odświeżenie widoku
                DataShow(inquiry, mainWindow.pracownicyGrid);

                int size = pracownicyGrid.Items.Count - 1;                      // ustalenie wielkości DataGrid
                size = size - 1;
                row = pracownicyGrid.Items.GetItemAt(size) as DataRowView;      // dostanie się do ostatniego rekordu DataGrid
                id = row.Row.ItemArray[0].ToString();                           // odczytanie elementu z pierwszej komórki ostatniego rekokrdu DataGrid
               // MessageBox.Show(id);

                User user = new User(id, name, surname, password);               // tworzy nowego użytkownika
                usersList.Add(user);                                             // dodanie uzytkownika do listy

                // inquiry = "select MAX(pracID) from pracownicy";

                Serialization.SaveUserListToFile(usersList, @"user.xml");        // zapisuje listę użytkowników do pliku
            }
            else
            {
                errorMessageLabel.Content = "Hasło nie jest poprawne";
            }
        }

        private void closePanel_Click(object sender, RoutedEventArgs e)                     // funkcja zamykająca okno Panel
        {
            this.Close();
            connection.Close();
        }

        private void RemoveUser_Click(object sender, RoutedEventArgs e)                     // metoda do usuwania użytkownika z listy 
        {
            string inquiry = "delete from pracownicy where pracID=" + id + "";              // usunięcie pracownika o podanym id
            string inquiry2 = "delete from wypozyczenia where pracID=" + id + "";           // usunięcie wypożyczeń pracownika o podanym id

            selectedIndex = pracownicyGrid.SelectedIndex;                                   // pokazuje indeks na pracownicyGrid
           
            if (row != null)
            {
                DataShow(inquiry2, pracownicyGrid);
                DataShow(inquiry, pracownicyGrid);
                usersList.RemoveAt(selectedIndex);                                          // usuwa z listy użytkowników użytkownika pod podanym indekse     
            }
           
            Serialization.SaveUserListToFile(usersList, @"user.xml");                       // zapisuje listę użytkowników do pliku

            inquiry = "Select * from pracownicy";                                           // odświeżenie widoku pracownicy po dodaniu rekordu
            DataShow(inquiry, pracownicyGrid);                                             
        }

        private void DaneAdmin_Click(object sender, RoutedEventArgs e)
        {
            errorMessageLabel.Content = "Wprowadź nowe dane administratora";
            loginAdminTextBox.Focus();                                                      // ustawia kursor na pole loginAdminTextBox
            repeatPasswordAdminBox.IsEnabled = true;                                        // uaktywnia przycisk repeatPasswordAdminBox   
            confirChanges.IsEnabled = true;                                                 // uaktywnia przycisk confirmChanges
        }
        private void ConfirmChanges_Click(object sender, RoutedEventArgs e)
        {
            adminList = new List<User>();

            string adminLogin = loginAdminTextBox.Text;
            string adminPassword = passwordAdminBox.Password.ToString();
            string repeatPasswprd = repeatPasswordAdminBox.Password.ToString();

            if(adminPassword.Equals(repeatPasswprd))
            {
                errorMessageLabel.Content = "Hasło poprawne";

                User user = new User(adminLogin, adminLogin, adminPassword);               // tworzy nowego użytkownika
                adminList.Add(user);                                                       // dodanie uzytkownika do listy

                Serialization.SaveUserListToFile(adminList, @"admin.xml");                 // zapisuje listę użytkowników do pliku

                MessageBox.Show("Zmieniono dane administratora");
            }
            else
            {
                errorMessageLabel.Content = "Hasło nie jest poprawne";
            }
        }

        private void CreateFirstAdmin()                                            // tworzy lub resetuje login i hasło administratora
        {
            adminList = new List<User>();

            User admin = new User();
            admin.Name = "Admin";
            admin.Surname = "Admin";
            admin.Password = "Admin";

            adminList.Add(admin);

            Serialization.SaveUserListToFile(adminList, @"admin.xml");
        }
        
        private void Initialize()
        {
            changePasswordButton.IsEnabled = false;                           // deaktywuje przycisk dodaj
            removeUserButton.IsEnabled = false;                               // deaktywuje przycisk usuń
            daneAdminButton.IsEnabled = false;                                // deaktywuje przycisk hasło admin
            repeatPasswordAdminBox.IsEnabled = false;                         // deaktywuje pole repeatpasswordAdminBox
            confirChanges.IsEnabled = false;                                  // deaktywuje przycisk confirChanges
            daneAdminButton.Focus();                                          // ustawia kursror w polu daneAdminButton

            usersList = Serialization.LoadUserFromFile(@"user.xml");          // funkcja wgrywająca użytkowników do listy z pliku
            adminList = Serialization.LoadUserFromFile(@"admin.xml");         // funkcja wgrywająca administratorów do listy z pliku

            string inquiry = "Select * from pracownicy";                      // odświeżenie widoku pracownicy po dodaniu rekordu
            DataShow(inquiry, pracownicyGrid);

            // CreateFirstAdmin();
        }

        public void DataShow(string inquiry, DataGrid dataGrid)      // metoda do dodawania pracowników do bazy
        {                                                            // przyjmuje 2 parametry (zapytanie, siatka do wyświetlania danych)
            try
            {
                SqlCommand command = new SqlCommand();               // tworzy nowy rozkaz SQL

                command.CommandText = inquiry;                       // wczytuje zapytanie SQL
                command.Connection = connection;                     // ścieżka do bazy 

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

        private void Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            row = pracownicyGrid.SelectedItem as DataRowView;
         
            if (row != null)
            {
                id = row.Row.ItemArray[0].ToString();
            }
        }
    }
   
}
