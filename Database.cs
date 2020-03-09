using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace StudentsDetails
{
    class Database
    {
        // For the connection to sql server database
        public SqlConnection con;

        public void connection()
        {
            // Data Source is the name of the  server on which the database is stored.            
            // The Initial Catalog is used to specify the name of the database             
            con = new SqlConnection("Data Source=(local);Initial Catalog=Student;Integrated Security=True;");
        }

        internal void Open()
        {
            throw new NotImplementedException();
        }
    }
}
