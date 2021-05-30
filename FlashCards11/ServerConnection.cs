using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FlashCards11
{
    class ServerConnection
    {
        public static SqlConnection GetDBConnection()
        {
            //Server/Instance; TableName; SQL-Login
            //string connectionString = @"Data Source=DESKTOP-KM\SQLEXPRESS; Initial Catalog=StudyApp; Persist Security Info=True;User ID = sa; Password = sa";
            string connectionString = @"Data Source=DESKTOP-KM\SQLEXPRESS; Initial Catalog=StudyApp; Persist Security Info=True; Integrated Security=SSPI";
            SqlConnection con = new SqlConnection(connectionString);
            return con;
        }
    }
}

