using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Baza.Connection
{
    public class InsertCommand
    {
        private DataFill dataFill;

        public InsertCommand()
        {
            dataFill = new DataFill();    
        }

        public void InsertCar(DataGrid Grid, List<string> lista, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("insert into samochody(marka, model, kolor, zajety, cenaDoby) values(@marka, @model, @kolor, @zajety, @cenaDoby);", connection);
            command.Parameters.AddWithValue("@marka", lista[0]);
            command.Parameters.AddWithValue("@model", lista[1]);
            command.Parameters.AddWithValue("@kolor", lista[2]);
            command.Parameters.AddWithValue("@cenaDoby", lista[3]);
            command.Parameters.AddWithValue("@zajety", lista[4]);

            dataFill.DataShow2(Grid, command);                     
        }

        public void InsertEmployee(DataGrid Grid, List<string> lista, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("insert into pracownicy(imie, nazwisko) values(@imie, @nazwisko);", connection);
            command.Parameters.AddWithValue("@imie", lista[0]);
            command.Parameters.AddWithValue("@nazwisko", lista[1]);
          
            dataFill.DataShow2(Grid, command);
        }

        public void InsertKlient(DataGrid Grid, List<string> lista, SqlConnection connection)
        {     
            SqlCommand command = new SqlCommand("insert into klienci(imie, nazwisko, pesel) values(@imie, @nazwisko, @pesel);", connection);
            command.Parameters.AddWithValue("@imie", lista[0]);
            command.Parameters.AddWithValue("@nazwisko", lista[1]);
            command.Parameters.AddWithValue("@pesel", lista[2]);

            dataFill.DataShow2(Grid, command);
        }

        public void InsertWypozyczenia(DataGrid Grid, List<string> lista, SqlConnection connection)
        {       
            SqlCommand command = new SqlCommand("insert into wypozyczenia(samID, pracID, klientID, datawyp, datazwr, koszt) values(@samID, @pracID, @klientID, @datawyp, @datazwr, @koszt);", connection);
            command.Parameters.AddWithValue("@samID", lista[0]);
            command.Parameters.AddWithValue("@pracID", lista[1]);
            command.Parameters.AddWithValue("@klientID", lista[2]);
            command.Parameters.AddWithValue("@datawyp", lista[3]);
            command.Parameters.AddWithValue("@datazwr", lista[4]);
            command.Parameters.AddWithValue("@koszt", lista[5]);

            dataFill.DataShow2(Grid, command);
        }
    }
}
