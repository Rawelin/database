using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Baza.Connection
{
    public class DeleteCommand
    {
        private DataFill dataFill;

        public DeleteCommand()
        {
            dataFill = new DataFill();
        }

        public void DeleteClient(DataGrid Grid, string klientID, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("delete from klienci where klientID=@klientID", connection);
            command.Parameters.AddWithValue("@klientID", klientID);
           
            dataFill.DataShow2(Grid, command);
        }

        public void DeleteWypozyczenia(DataGrid Grid, string wypID, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("delete from wypozyczenia where wypID=@wypID", connection);
            command.Parameters.AddWithValue("@wypID", wypID);

            dataFill.DataShow2(Grid, command);
        }

        public void DeletePracownik(DataGrid Grid, string klientID, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("delete from pracownicy where klientID=@klientID", connection);
            command.Parameters.AddWithValue("@klientID", klientID);

            dataFill.DataShow2(Grid, command);
        }

        public void DeleteCar(DataGrid Grid, string samID, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("delete from samochody where samID=@samID", connection);
            command.Parameters.AddWithValue("@samID", samID);

            dataFill.DataShow2(Grid, command);
        }
    }
}
