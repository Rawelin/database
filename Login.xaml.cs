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
        MainWindow mainWindow;

        public Login(MainWindow window)
        {
           InitializeComponent();
           mainWindow = window;
        }

     
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (this.loginTextBox.Text.Equals("") && passwordBox.Password.ToString().Equals(""))
            {
                this.Close();

                mainWindow.StartConnection();
                mainWindow.userButtonsEnable();
                mainWindow.loginDisable();
            }
            else if (this.loginTextBox.Text.Equals("admin") && passwordBox.Password.ToString().Equals(""))
            {
                this.Close();

                mainWindow.StartConnection();
                mainWindow.adminButtonsEnable();
                mainWindow.loginDisable();
            }
            else
            {
                loginTextBox.Text = "";
                passwordBox.Password = "";
                messageLabel.Content = "Podano zły login lub hasło";
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
