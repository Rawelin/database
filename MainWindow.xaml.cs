using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;                       // for ConfigurationManger - add references in solution explorer
using System.Data;                                // for DataTable
                                                  

namespace Baza
{
   
    public partial class MainWindow : Window
    {
        private String inquiry;
        private SqlConnection connection;

        public MainWindow()
        {
            InitializeComponent();
            
            Start.IsEnabled = false;
        }

        private void StartConnection()
        {
            // String connectionString = "Data Source=(localdb)\v11.0;Initial Catalog=Student; Integrated Security=true";   
            // connection.Open();     // because default state is Close();
  
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["connstudent"].ConnectionString;
                connection.Open();

                Start.IsEnabled = true;
                MessageBox.Show("Connected to db");

            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Failure", ex.Message);
            }

            /*   var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
               var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
               connectionStringsSection.ConnectionStrings["Blah"].ConnectionString = "Data Source=(localdb)\v11.0;Initial Catalog=Student; Integrated Security=true";
               config.Save();
               ConfigurationManager.RefreshSection("connectionStrings");

           */
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            inquiry = inquiryTextBox.Text;
          
            try
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = inquiry;
                command.Connection = connection;

                command.ExecuteScalar();

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable("Students");
                da.Fill(dt);

                g1.ItemsSource = dt.DefaultView;

               // MessageBox.Show("Inquiry ok.");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Inquiry Failure", ex.Message);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            StartConnection();
        }
    }
}
