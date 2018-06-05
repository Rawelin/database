using System.Data.SqlClient;
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

        public void addItemsToComboBox(string inq, ComboBox comboBox)               // metoda dodaje elementy do combobox;
        {
            SqlCommand cmd = new SqlCommand(inq, connection);
            SqlDataReader DR = cmd.ExecuteReader();

            while (DR.Read())
            {
                comboBox.Items.Add(DR[0] + ". " + DR[1] + " " + DR[2]);

            }
            DR.Close();
        }
    }
}
