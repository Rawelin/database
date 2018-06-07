using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Shapes;

namespace Baza
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public string login;
        public string password;
        public MainWindow mainWindow;                                     // obiekt klasy MainWindow
        public List<User> usersList;                                      // lista użytkowników
        public List<User> adminList;

        public Login(MainWindow window)                                   // konstruktor pobierający MainWindow
        {
           InitializeComponent();
           Initailize();                                                   // inicjalizuje pola 
           mainWindow = window;                                            // inicjalizuje obiekt mainWindow

        }


        private void Login_Click(object sender, RoutedEventArgs e)         // metoda przycisku login 
        {

            for (int i = 0; i < usersList.Count; i++)                      // sprawdza w pętli czy jakiś login i hasło pasują do zapisanych w pliku
            {
                if (this.loginTextBox.Text.Equals(usersList[i].Name) && passwordBox.Password.Equals(usersList[i].Password))   // walidacja loginu i hasła dla użytkownika
                {
                    this.Close();                                           // zamyka okno Login

                    mainWindow.StartConnection();                           // aktywuje funkcję z MainWindow
                    mainWindow.userButtonsEnable();
                    mainWindow.loginDisable();
                    mainWindow.Status.Content = "Połączony z bazą jako użytkownik " + usersList[i].Name + " " + usersList[i].Surname;   // aktualizuje etykietę napisem
                }
              
            }

            for (int i = 0; i < adminList.Count; i++)                      // sprawdza w pętli czy jakiś login i hasło pasują do zapisanych w pliku 
            {
                if (this.loginTextBox.Text.Equals(adminList[i].Name) && passwordBox.Password.ToString().Equals(adminList[i].Password)) // walidacja loginu i hasła dla administratora
                {
                    this.Close();                                           // zamyka okno Login

                    mainWindow.StartConnection();                           // aktywuje funkcję z MainWindow
                    mainWindow.adminButtonsEnable();
                    mainWindow.loginDisable();
                    mainWindow.Status.Content = "Połączony z bazą jako Administrator";  // aktualizuje etykietę napisem
                }
            }
      
            loginTextBox.Text = "";                                    // czyści pole tekstowe login
            passwordBox.Password = "";                                 // czyści pole pasword
            messageLabel.Content = "Dane są nieprawidłowe.";
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)     // metoda przycisku cancel
        {
            MessageBox.Show("Nie połączono z bazą danych");             // wyświetla messagebox na ekran
            this.Close();                                               // zamyka okno Login
        }
        private void Initailize()
        {
                                                      // inicjalizacja mainWindow obiektem pobranym przez konstruktor
            usersList = Serialization.LoadUserFromFile(@"user.xml");       // funkcja wgrywająca użytkowników do listy z pliku
            adminList = Serialization.LoadUserFromFile(@"admin.xml");      // funkcja wgrywająca administratorów do listy z pliku
            loginTextBox.Focus();
        }
    }
}
