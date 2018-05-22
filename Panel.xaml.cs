using System;
using System.Collections.Generic;
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
   
    public partial class Panel : Window
    {
        List<User> usersList;

        public Panel()
        {
            InitializeComponent();
            changePasswordButton.IsEnabled = false;
            usersList = new List<User>();
        }

        private void adminLaogin_Click(object sender, RoutedEventArgs e)
        {
            if (this.loginAdminTextBox.Text.Equals("") && passwordAdminBox.Password.ToString().Equals(""))
            {
                changePasswordButton.IsEnabled = true;
                errorMessageLabel.Content = "";
            }
            else
            {
                errorMessageLabel.Content = "Podano zły login lub hasło";
            }
        }

        private void addUser_Click(object sender, RoutedEventArgs e)
        {
            string login = loginTextBox.Text;
            string password = passwordBox.Password.ToString();
            string repeatPassword = repeatPasswordBox.Password.ToString();
            
            //  errorMessageLabel.Content = login + " " + password + " " + repeatPassword;

            if (password.Equals(repeatPassword))
            {
               errorMessageLabel.Content = "Hasło poprawne";
                User user = new User(login, password);
                usersList.Add(user);
                errorMessageLabel.Content = usersList[0].Login;
            }
            else
            {
                errorMessageLabel.Content = "Hasło nie poprawne";
            }

           
        }
    }
}
