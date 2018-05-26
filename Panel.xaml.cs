using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Baza
{
   
    public partial class Panel : Window
    {
        public List<User> usersList;                                 // lista użytkowników

        public Panel()
        {
            InitializeComponent();
            changePasswordButton.IsEnabled = false;                  // deaktywuje przycisk dodaj
           
            loadUserFromFile();                                      // funkcja wgrywająca użytkowników do listy z pliku
            addUserToUsersView();
        }

        private void adminLaogin_Click(object sender, RoutedEventArgs e)
        {
            if (this.loginAdminTextBox.Text.Equals("admin") && passwordAdminBox.Password.ToString().Equals(""))  // walidacja loginu i hasła
            {
                changePasswordButton.IsEnabled = true;               // aktywuje przycisk dodaj
                errorMessageLabel.Content = "";                      // czyści etykiete errorMessageLabel
            }
            else
            {
                errorMessageLabel.Content = "Podano zły login lub hasło";      // wysyła wiadomość do errorMessageLabel
            }
        }

        private void addUser_Click(object sender, RoutedEventArgs e)            // metoda dla przycisku addUser
        {
            string login = loginTextBox.Text;                                   // zapis do zmiennych z pól tekstowych i password
            string password = passwordBox.Password.ToString();
            string repeatPassword = repeatPasswordBox.Password.ToString();
            
            //  errorMessageLabel.Content = login + " " + password + " " + repeatPassword;

            if (password.Equals(repeatPassword))                                 // walidacja hasła
            {
                errorMessageLabel.Content = "Hasło poprawne";

                User user = new User(login, password);                           // tworzy nowego użytkownika
                usersList.Add(user);                                             // dodanie uzytkownika do listy

                using (Stream fs = new FileStream(@"user.xml", FileMode.Create, FileAccess.Write, FileShare.None))   // zapis do pliku
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                    serializer.Serialize(fs, usersList);
                }

                MessageBox.Show("Dodano Użytkownika");

                loginTextBox.Text = "";                                         // Czyszczenie pól 
                passwordBox.Password = "";
                repeatPasswordBox.Password = "";

                UsersListView.Items.Clear();                                    // Czyszczenie listy widoku

                addUserToUsersView();                                           // odświeżenie widoku

            }
            else
            {
                errorMessageLabel.Content = "Hasło nie jest poprawne";
            }
        }
     
        private void loadUserFromFile()                                          // odczyt z pliku
        {
            usersList = new List<User>();                                        // tworzy nową listę dla użytkowników
          
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));    // deserializacja

            using (FileStream fs2 = File.OpenRead(@"user.xml"))                  // odczyt z pliku
            {
                usersList = (List<User>)serializer.Deserialize(fs2);             // wczytanie użytkowników z pliku do listy
            }

           
        }
        private void addUserToUsersView()
        {
            for (int i = 0; i < usersList.Count(); i++)
            {
                UsersListView.Items.Add(usersList[i].Login);
            } 
        }

        private void closePanel_Click(object sender, RoutedEventArgs e)        // funkcja zamykająca okno Panel
        {
            this.Close();
        }
    }
}
