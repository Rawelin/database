﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Baza.Connection
{
    class EditCommand
    {
        private DataFill dataFill;

        public EditCommand()
        {
            dataFill = new DataFill();
        }

        public void EditClient(DataGrid Grid, List<string> lista, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("update klienci set imie=@imie, nazwisko=@nazwisko, pesel=@pesel where klientID=@klientID", connection);
            command.Parameters.AddWithValue("@imie", lista[0]);
            command.Parameters.AddWithValue("@nazwisko", lista[1]);
            command.Parameters.AddWithValue("@pesel", lista[2]);
            command.Parameters.AddWithValue("@klientID", lista[3]);
        
            dataFill.DataShow2(Grid, command);
        }

        public void EditEmployee(DataGrid Grid, List<string> lista, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("update pracownicy set imie=@imie, nazwisko=@nazwisko where pracID=@pracID", connection);
            command.Parameters.AddWithValue("@imie", lista[0]);
            command.Parameters.AddWithValue("@nazwisko", lista[1]);
            command.Parameters.AddWithValue("@pracID", lista[2]);
         
            dataFill.DataShow2(Grid, command);
        }

        public void EditCar(DataGrid Grid, List<string> lista, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand("update samochody set marka=@marka, model=@model, kolor=@kolor, cenaDoby=@cenaDoby where samID=@samID", connection);
            command.Parameters.AddWithValue("@marka", lista[0]);
            command.Parameters.AddWithValue("@model", lista[1]);
            command.Parameters.AddWithValue("@kolor", lista[2]);
            command.Parameters.AddWithValue("@cenaDoby", lista[3]);
            command.Parameters.AddWithValue("@samID", lista[4]);

            dataFill.DataShow2(Grid, command);
        }

    }
}