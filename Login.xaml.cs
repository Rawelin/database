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
                    this.Close();                                          // zamyka okno Login

                    LoginProcess(mainWindow, usersList, i, true);

                    mainWindow.pracownicyComboBox.Items.Clear();
                    mainWindow.pracownicyComboBox.Items.Add(usersList[i].ID);
                }
              
            }

            for (int i = 0; i < adminList.Count; i++)                      // sprawdza w pętli czy jakiś login i hasło pasują do zapisanych w pliku 
            {
                if (this.loginTextBox.Text.Equals(adminList[i].Name) && passwordBox.Password.ToString().Equals(adminList[i].Password)) // walidacja loginu i hasła dla administratora
                {
                    this.Close();                                          // zamyka okno Login

                    LoginProcess(mainWindow, adminList, i, false);
                }
            }
          
            loginTextBox.Text = "";                                        // czyści pole tekstowe login
            passwordBox.Password = "";                                     // czyści pole pasword
            messageLabel.Content = "Dane są nieprawidłowe.";
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)        // metoda przycisku cancel
        {
            MessageBox.Show("Nie połączono z bazą danych");                // wyświetla messagebox na ekran
            this.Close();                                                  // zamyka okno Login
        }
        private void Initailize()
        {                                                                  // inicjalizacja mainWindow obiektem pobranym przez konstruktor
            usersList = Serialization.LoadUserFromFile(@"user.xml");       // funkcja wgrywająca użytkowników do listy z pliku
            adminList = Serialization.LoadUserFromFile(@"admin.xml");      // funkcja wgrywająca administratorów do listy z pliku
            loginTextBox.Focus();
        }
        private void LoginProcess(MainWindow mainWindow, List<User> lista, int index, bool userType)
        {
             mainWindow.StartConnection();                                  // inicjalizuje polączenie z bazą

            if (userType == true)
            {
                mainWindow.userButtonsEnable();
            }
            else
            {
                mainWindow.adminButtonsEnable();     
            }
         
            string text = "Połączony z bazą jako użytkownik";

            string userFormat = string.Format("{0}: {1} {2} Id: {3}", text, lista[index].Name, lista[index].Surname, lista[index].ID);
            string adminFormat = string.Format("{0}: {1}", text, lista[index].Name);

            mainWindow.Status.Content = userType == true ? userFormat : adminFormat;
            mainWindow.loginDisable();
        }
    }
}
