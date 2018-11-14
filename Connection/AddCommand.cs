using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Baza.Connection
{
    public class AddCommand
    {
        DataFill dataFill;

        public AddCommand()
        {
            dataFill = new DataFill();    
        }

        public void AddCar(DataGrid samochodyGrid, List<string> lista, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("insert into samochody(marka, model, kolor, zajety, cenaDoby) values(@marka, @model, @kolor, @zajety, @cenaDoby);", connection);
            command.Parameters.AddWithValue("@marka", lista[0]);
            command.Parameters.AddWithValue("@model", lista[1]);
            command.Parameters.AddWithValue("@kolor", lista[2]);
            command.Parameters.AddWithValue("@cenaDoby", lista[3]);
            command.Parameters.AddWithValue("@zajety", lista[4]);

            dataFill.DataShow2(samochodyGrid, command);                     
        }
    }
}
