using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Baza.Connection
{
    public class DataFill
    {

        public void DataShow(string inquiry, DataGrid dataGrid, SqlConnection connection)     // funkcja do wyświetlania zapytan 
        {                                                                                     // przyjmuje 2 parametry (zapytanie, siatka do wyświetlania danych)
            try
            {
                SqlCommand command = new SqlCommand();                                        // tworzy nowy rozkaz SQL

                command.CommandText = inquiry;                                                // wczytuje zapytanie SQL
                command.Connection = connection;                                              // ścieżka do bazy 

                // command.ExecuteScalar();

                SqlDataAdapter da = new SqlDataAdapter(command);                              // pobiera zapytanie do adaptera
                DataTable dt = new DataTable("Car Rent");                                     // tworzy nową tabelę
                da.Fill(dt);                                                                  // pobiera tabelę do adaptera

                dataGrid.ItemsSource = dt.DefaultView;                                        // wyświetla dane na gridzie z tabeli dt
            }
            catch (Exception ex)                                                              // wyłapuje wyjątki 
            {
                MessageBox.Show("Komunikat diagnostyczny do odczytywania błędów", ex.Message);    // wyświetla messagebox na ekran
            }
        }
    }
}
