using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Baza.Connection
{
    public class SelectCommand
    {
        private DataFill dataFill;
        private SqlCommand command;

        public SelectCommand()
        {
            dataFill = new DataFill();
            command = new SqlCommand();
        }

        public void SelectTop(DataGrid grid, int number, SqlConnection connection)
        {
            if(number == 1)
            {
                command = new SqlCommand("Select imie, nazwisko, count(wypozyczenia.klientID) as 'Wypożyczenia' from wypozyczenia, klienci " +
               "where wypozyczenia.klientID = klienci.klientID group by imie, nazwisko order by count(wypozyczenia.KlientID) DESC;", connection);
            }
            else if(number == 2)
            {
                command = new SqlCommand("Select marka, model, count(samochody.samID) as 'Wypożyczenia', sum(wypozyczenia.koszt) as koszt from wypozyczenia, samochody" +
                    " where wypozyczenia.samID = samochody.samID group by marka, model order by koszt DESC;", connection);
            }
            else if(number == 3)
            {
                command = new SqlCommand("Select imie, nazwisko, count(pracownicy.pracID) as 'Wypożyczenia' from wypozyczenia, pracownicy" +
                    " where wypozyczenia.pracID = pracownicy.pracID group by imie, nazwisko order by count(pracownicy.pracID) DESC;", connection);
            }

            dataFill.DataShow2(grid, command);
        }

        public void SelectAll(DataGrid grid, int number, SqlConnection connection)
        {
            if(number == 1)
            {
                command = new SqlCommand("Select * from klienci", connection);
            }
            else if(number == 2)
            {
                command = new SqlCommand("Select * from pracownicy", connection);
            }
            else if(number == 3)
            {
                command = new SqlCommand("Select * from samochody", connection);
            }
            else if(number == 4)
            {
                command = new SqlCommand("select wypozyczenia.wypID, klienci.imie, klienci.nazwisko, samochody.samID, samochody.marka, samochody.model, samochody.zajety, samochody.cenaDoby," +
               " pracownicy.pracID, pracownicy.imie as imiePrac, pracownicy.nazwisko as nazwiskoPrac, wypozyczenia.datawyp, wypozyczenia.datazwr, wypozyczenia.koszt as RabatKoszt, " +
               "(datediff(day,datawyp, datazwr) * samochody.cenaDoby) as Koszt, datediff(day,datawyp, datazwr) as IloscDni, datediff(day,GETDATE(), datazwr) as DniDoZwrotu " +
               "from klienci, samochody, wypozyczenia, pracownicy where wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID and " +
               "wypozyczenia.pracID = pracownicy.pracID", connection);      
            }
            else if(number == 5)
            {
                command = new SqlCommand("select wypozyczenia.wypID, klienci.imie, klienci.nazwisko, samochody.marka, samochody.model, wypozyczenia.datawyp, wypozyczenia.datazwr " +
                "from klienci, samochody, wypozyczenia where wypozyczenia.klientID = klienci.klientID and wypozyczenia.samID = samochody.samID", connection);
            }

            dataFill.DataShow2(grid, command);
        }
    }
}
