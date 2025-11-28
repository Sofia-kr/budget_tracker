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
           string connectionString = "server=sql7.freesqldatabase.com;port=3306;user=sql7803706;password=DrUIbcmB1f;database=sql7803706;Charset=utf8mb4;";
            return new MySqlConnection(connectionString);
        }
    }
}
