using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;

namespace Baza
{
   
    public partial class Panel : Window
    {
        public List<User> usersList;                                 // lista użytkowników
        public List<User> adminList;                                 // lista administratorów

        public Panel()
        {
            InitializeComponent();
            Initialize();
        }

        private void AdminLaogin_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < adminList.Count(); i++)
            {
                if (this.loginAdminTextBox.Text.Equals(adminList[i].Login) && passwordAdminBox.Password.ToString().Equals(adminList[i].Password))  // walidacja loginu i hasła
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

        private void addUser_Click(object sender, RoutedEventArgs e)            // metoda dla przycisku addUser
        {
            string login = loginTextBox.Text;                                   // zapis do zmiennych z pól tekstowych i password
            string password = passwordBox.Password.ToString();
            string repeatPassword = repeatPasswordBox.Password.ToString();
            
            if (password.Equals(repeatPassword))                                 // walidacja hasła
            {
                errorMessageLabel.Content = "Hasło poprawne";

                User user = new User(login, password);                           // tworzy nowego użytkownika
                usersList.Add(user);                                             // dodanie uzytkownika do listy

                Serialization.SaveUserListToFile(usersList, @"user.xml");                     // zapisuje listę użytkowników do pliku


                MessageBox.Show("Dodano Użytkownika");

                loginTextBox.Text = "";                                           // Czyszczenie pól 
                passwordBox.Password = "";
                repeatPasswordBox.Password = "";

                UsersListView.Items.Clear();                                      // Czyszczenie listy widoku

                AddUserToUsersView();                                             // odświeżenie widoku

            }
            else
            {
                errorMessageLabel.Content = "Hasło nie jest poprawne";
            }
        }
     
        private void AddUserToUsersView()                                        // metoda odświeżająca listę
        { 
            UsersListView.Items.Clear();                                         // czyści listę

            for (int i = 0; i < usersList.Count(); i++)
            {
                UsersListView.Items.Add(usersList[i].Login);                     // dodaje użytkowników z listy użytkowników do listy w widoku
            } 
        }

        private void closePanel_Click(object sender, RoutedEventArgs e)          // funkcja zamykająca okno Panel
        {
            this.Close();
        }

        private void RemoveUser_Click(object sender, RoutedEventArgs e)          // metoda do usuwania użytkownika z listy 
        {
            int selectedIndex = UsersListView.SelectedIndex;                     // pobiera index wciśnięty na liście w widoku

            if (selectedIndex >= 0)
                usersList.RemoveAt(selectedIndex);                                   // usuwa z listy użytkowników użytkownika pod podanym indeksem

            AddUserToUsersView();                                                // odświeża listę w widoku

            Serialization.SaveUserListToFile(usersList, @"user.xml");                         // zapisuje listę użytkowników do pliku
        }

        private void DaneAdmin_Click(object sender, RoutedEventArgs e)
        {
            errorMessageLabel.Content = "Wprowadź nnowe dane administratora";
            loginAdminTextBox.Focus();                                            // ustawia kursor na pole loginAdminTextBox
            repeatPasswordAdminBox.IsEnabled = true;                              // uaktywnia przycisk repeatPasswordAdminBox   
            confirChanges.IsEnabled = true;                                       // uaktywnia przycisk confirmChanges
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

                User user = new User(adminLogin, adminPassword);                           // tworzy nowego użytkownika
                adminList.Add(user);                                             // dodanie uzytkownika do listy

                Serialization.SaveUserListToFile(adminList, @"admin.xml");                     // zapisuje listę użytkowników do pliku

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
            admin.Login = "Admin";
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

            usersList = Serialization.LoadUserFromFile(@"user.xml");           // funkcja wgrywająca użytkowników do listy z pliku
            adminList = Serialization.LoadUserFromFile(@"admin.xml");          // funkcja wgrywająca administratorów do listy z pliku
            AddUserToUsersView();

            CreateFirstAdmin();
        }
    }
}
