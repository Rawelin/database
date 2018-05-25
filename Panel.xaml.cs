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
        public List<User> usersList;

        public Panel()
        {
            InitializeComponent();
            changePasswordButton.IsEnabled = false;
           
            loadUserFromFile();
        }

        private void adminLaogin_Click(object sender, RoutedEventArgs e)
        {
            if (this.loginAdminTextBox.Text.Equals("admin") && passwordAdminBox.Password.ToString().Equals(""))
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

                using (Stream fs = new FileStream(@"user.xml", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<User>));
                    serializer.Serialize(fs, usersList);

                }

                errorMessageLabel.Content = usersList[0].Login;

            }
            else
            {
                errorMessageLabel.Content = "Hasło nie poprawne";
            }
        }
    
        private void loadUserFromFile()
        {
            usersList = new List<User>();
          
            XmlSerializer serializer = new XmlSerializer(typeof(List<User>));

            using (FileStream fs2 = File.OpenRead(@"user.xml"))
            {
                usersList = (List<User>)serializer.Deserialize(fs2);
            }
        }

        private void closePanel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
