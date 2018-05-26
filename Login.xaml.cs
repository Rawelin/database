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
        MainWindow mainWindow;                                  // obiekt klasy MainWindow

        public Login(MainWindow window)                         // konstruktor pobierający MainWindow
        {
           InitializeComponent();
           mainWindow = window;                                 // inicjalizacja mainWindow obiektem pobranym przez konstruktor
        }


        private void Login_Click(object sender, RoutedEventArgs e)      // metoda przycisku login 
        {
            if (this.loginTextBox.Text.Equals("user") && passwordBox.Password.ToString().Equals(""))   // walidacja loginu i hasła dla użytkownika
            {
                this.Close();                                           // zamyka okno Login

                mainWindow.StartConnection();                           // aktywuje funkcję z MainWindow
                mainWindow.userButtonsEnable();
                mainWindow.loginDisable();
                mainWindow.Status.Content = "Połączony z bazą jako użytkownik";   // aktualizuje etykietę napisem
            }
            else if (this.loginTextBox.Text.Equals("admin") && passwordBox.Password.ToString().Equals("")) // walidacja loginu i hasła dla administratora
            {
                this.Close();                                           // zamyka okno Login

                mainWindow.StartConnection();                           // aktywuje funkcję z MainWindow
                mainWindow.adminButtonsEnable();
                mainWindow.loginDisable();
                mainWindow.Status.Content = "Połączony z bazą jako Administrator";  // aktualizuje etykietę napisem
            }
            else
            {
                loginTextBox.Text = "";                                 // czyści pole tekstowe login
                passwordBox.Password = "";                              // czyści pole pasword
                messageLabel.Content = "Dane są nieprawidłowe.";
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)     // metoda przycisku cancel
        {
            MessageBox.Show("Nie połączono z bazą danych");             // wyświetla messagebox na ekran
            this.Close();                                               // zamyka okno Login
        }
    }
}
