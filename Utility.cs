using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;


namespace Baza
{
    public class Utility
    {
        private SqlConnection connection;

        public Utility(SqlConnection connection)
        {
            this.connection = connection;
        }

        public void addItemsToComboBox(string inq, ComboBox comboBox, int atr = 0)                        // metoda dodaje elementy do combobox;
        {
            SqlCommand command = new SqlCommand(inq, connection);
            SqlDataReader dataReader = command.ExecuteReader();

            comboBox.Items.Clear();

            if(atr == 0)
            {
                while (dataReader.Read())
                {
                    comboBox.Items.Add(dataReader[0] + ". " + dataReader[1] + " " + dataReader[2]);
                }
                dataReader.Close();
            }
            if (atr == 1)
            {
                while (dataReader.Read())
                {
                    if (dataReader[4].ToString().Equals("False"))                                        // dodje tylko dostępne samochody
                    {
                        comboBox.Items.Add(dataReader[0] + ". " + dataReader[1] + " " + dataReader[2]);
                    }
                }
                dataReader.Close();
            }

        }

        public static string TrimPath(string path)                                                      // metoda usuwająca wszystkie znaki po kropce łacznie z nią
        {
            int index = path.IndexOf(".");

            if(index > 0)
                path = path.Substring(0, index);

            return path;
        }
    }
}
