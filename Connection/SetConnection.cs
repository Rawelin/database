using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Baza
{
    public class SetConnection
    {
        SqlConnection connection;

        public SetConnection() { }

        public SqlConnection GetConnection ()
        {
            try
            {
                connection = new SqlConnection();                                                                           // tworzy nowe polączenie SQL
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["wypozyczalnia"].ConnectionString;     // ścieżka do bazy zmajdująca się w App.config
                connection.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Failure", ex.Message);                                                          // wyświetla messagebox na ekranie
            }
            return connection;
        }
    }
}
