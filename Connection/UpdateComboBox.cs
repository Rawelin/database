using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Baza.Connection
{
    class UpdateComboBox
    {
        private SqlConnection sqlConnection;
        private Utility utility;

        public UpdateComboBox(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection;
            utility = new Utility(sqlConnection); 
        }

        public void UpdateClienci(ComboBox comboBox)
        {
            string querry = "select * from klienci";
            utility.addItemsToComboBox(querry, comboBox);
        }

        public void UpdateSamochody(ComboBox comboBox)
        {
            string querry = "select * from samochody";
            utility.addItemsToComboBox(querry, comboBox, 1);
        }
    }
}
