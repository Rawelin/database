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
        private String id;
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
                // MessageBox.Show("Connected to db", "Successful");
                Login login = new Login();
                login.Show();
               
                inquiry =  "Select * from klienci";    
                DataShow(inquiry, klienciGrid);

                inquiry = "Select * from pracownicy";
                DataShow(inquiry, pracownicyGrid);

                inquiry = "Select * from samochody";
                DataShow(inquiry, samochodyGrid);

                inquiry = "select klienci.imie, klienci.nazwisko, samochody.marka, wypozyczenia.wypID from klienci, samochody, wypozyczenia where wypozyczenia.klientID = 2 and wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID";
                DataShow(inquiry, wypozyczeniGrid);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Failure", ex.Message);
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            inquiry = inquiryTextBox.Text;

            DataShow(inquiry, wypozyczeniGrid);

            inquiry = "Select  klientID, count(klientID) as 'Wypożyczenia' from wypozyczenia group by KlientID";
            DataShow(inquiry, raportyGrid);
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
        private void DataShow(string inquiry, DataGrid dataGrid)     // funkcja pomocnicza - przyjmuje 2 parametry (zapytanie, siatka do wyświetlania danych)
        {
            try
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = inquiry;
                command.Connection = connection;

               // command.ExecuteScalar();

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

        private void addClient_click(object sender, RoutedEventArgs e)
        {
            string name = nameTextBox.Text;
            string surname =surnameTextBox.Text;
            string pesel = peselTextBox.Text;

            inquiry = "insert into klienci values('"+name+"', '"+surname+"', '"+pesel+"')";  

            DataShow(inquiry, klienciGrid);                                // dodanie rekordu

            inquiry = "Select * from klienci";                             // Odświeżenie widoku po dodaniu rekordu
            DataShow(inquiry, klienciGrid);
        }

        private void klienci_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = klienciGrid.SelectedItem as DataRowView;
            
            id = row.Row.ItemArray[0].ToString();
            nameTextBox.Text = row.Row.ItemArray[1].ToString(); 
            surnameTextBox.Text = row.Row.ItemArray[2].ToString(); 
            peselTextBox.Text = row.Row.ItemArray[3].ToString();
        }

        private void deleteClient_Click(object sender, RoutedEventArgs e)
        {
            inquiry = "delete from klienci where klientID="+id+"";

            DataShow(inquiry, klienciGrid);                                // skasownie rekordu

            inquiry = "Select * from klienci";                             // Odświeżenie widoku po dodaniu rekordu
            DataShow(inquiry, klienciGrid);
        }

        private void topClient_Click(object sender, RoutedEventArgs e)
        {
            // inquiry = "Select klienci.imie, klienci.nazwisko, samochody.marka, wypozyczenia.wypID from klienci, samochody, wypozyczenia where wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID";
            // inquiry = "Select klienci.imie, klienci.nazwisko, samochody.marka, wypozyczenia.wypID from klienci, samochody, wypozyczenia where wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID";
            // inquiry = "Select  klientID, klienci.imie, count(klientID) as 'Wypożyczenia' from wypozyczenia, klienci, samochody where wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID group by KlientID";
            // inquiry = "Select  klientID, klienci.imie, count(klientID) as 'Wypożyczenia' from wypozyczenia, klienci, where wypozyczenia.klientID = klienci.klientID group by KlientID";
            inquiry = "Select  klientID, count(klientID) as 'Wypożyczenia' from wypozyczenia group by KlientID";
            DataShow(inquiry, raportyGrid);
        }

        private void addWypozyczenie(object sender, RoutedEventArgs e)
        {
            string samID = samIDTextBox.Text;
            string pracID = pracIDTextBox.Text;
            string klientID = klientIDTextBox.Text;
            string datawyp = dataWypozyczeniaDatePicker.Text;
            string datazwr = dataZwrotuDatePicker.Text;
            string koszt = koszTextBox.Text;

            // insert into  wypozyczenia values(1, 1, 1, '2018-3-25', '2018-3-28', 1200)

            inquiry = "insert into wypozyczenia values('"+samID+"', '"+pracID+"', '"+klientID+"', '"+datawyp+"', '"+datazwr+"', '"+koszt+"')";

            DataShow(inquiry, wypozyczeniGrid);
        }

        private void deleteWypozyczenie(object sender, RoutedEventArgs e)
        {

        }
    }
}
