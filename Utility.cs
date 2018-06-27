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

        public void addItemsToComboBox(string inq, ComboBox comboBox, int atr = 0)               // metoda dodaje elementy do combobox;
        {
            SqlCommand cmd = new SqlCommand(inq, connection);
            SqlDataReader DR = cmd.ExecuteReader();

            comboBox.Items.Clear();

            if(atr == 0)
            {
                while (DR.Read())
                {
                    comboBox.Items.Add(DR[0] + ". " + DR[1] + " " + DR[2]);
                }
                DR.Close();
            }
            if (atr == 1)
            {
                while (DR.Read())
                {
                    if (DR[4].ToString().Equals("False"))                                        // dodje tylko dostępne samochody
                    {
                        comboBox.Items.Add(DR[0] + ". " + DR[1] + " " + DR[2]);
                    }
                }
                DR.Close();
            }

        }

        public static string TrimPath(string path)                                    // metoda usuwająca wszystkie znaki po kropce łacznie z nią
        {
            int index = path.IndexOf(".");

            if(index > 0)
                path = path.Substring(0, index);

            return path;
        }
    }
}
