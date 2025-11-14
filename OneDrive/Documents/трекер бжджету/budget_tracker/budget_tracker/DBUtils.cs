using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budget_tracker
{
    internal class DBUtils
    {
        public static MySqlConnection GetDBConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDatabaseConnection"].ConnectionString;
            return new MySqlConnection(connectionString);
        }
    }
}
