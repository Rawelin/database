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
            try
            {
                connection = new SqlConnection();
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["wypozyczalnia"].ConnectionString;
                connection.Open();

                Start.IsEnabled = true;
                MessageBox.Show("Connected to db");


                inquiry =  "Select * from klienci";    
                DataShow(inquiry, klienciGrid);

                inquiry = "Select * from pracownicy";
                DataShow(inquiry, pracownicyGrid);

                inquiry = "Select * from samochody";
                DataShow(inquiry, samochodyGrid);

                inquiry = "select klienci.imie, klienci.nazwisko, samochody.marka, wypozyczenia.wypID from klienci, samochody, wypozyczenia where wypozyczenia.klientID = 2 and wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID";
                DataShow(inquiry, zamowieniaGrid);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Failure", ex.Message);
            }
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
                DataTable dt = new DataTable("Car Rent");
                da.Fill(dt);

                g1.ItemsSource = dt.DefaultView;
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

        private void g1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // funkcja pomocnicza

        private void DataShow(string inquiry, DataGrid dataGrid)
        {
            try
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = inquiry;
                command.Connection = connection;

                command.ExecuteScalar();

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable("Car Rent");
                da.Fill(dt);
                
                dataGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Inquiry Failure", ex.Message);
            }
        }
    }
}
