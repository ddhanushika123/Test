using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace StudentsDetails
{
    public partial class login : Form
    {
        // Define all variables
        public SqlConnection con;
        public SqlCommand cmd;
        public SqlDataAdapter adt;
        int count;

        // Create class constructer to load component with form when start
        public login()
        {
            InitializeComponent();
        }

        // Login button click
        private void btn_login_Click(object sender, EventArgs e)
        {
            // try block
            try
            {
                // Check username or password is empty
                if (txt_userName.Text == "" || txt_password.Text == "")
                {
                    MessageBox.Show("Please enter UserName and Password", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;     // If username or password empty then do not execute code
                }

                // Access connection method in Databse class for get databse connection
                Database db = new Database();   // Create class instanse for Database class
                db.connection();                // Call connection methode in Database class

                // Create the SQL command
                cmd = new SqlCommand("Select * from Mas_user where Username = @username and Password = @password", db.con);
                
                // Add the parameters
                cmd.Parameters.AddWithValue("@username", txt_userName.Text);
                cmd.Parameters.AddWithValue("@password", txt_password.Text);


                // If has maching record with user enterd values then it assign to count variable
                adt = new SqlDataAdapter(cmd);          // data adapter object is use to insert, update or delete commands                 
                DataSet ds = new DataSet();
                adt.Fill(ds);

                count = ds.Tables[0].Rows.Count;

                // If count variable value equal to 1 then user can be loging
                if (count == 1)
                {
                    this.Hide();
                    studetDetails fm = new studetDetails();
                    fm.Show();
                }
                // If count variable has not equal to a value then user can not loging. Because databse has not save that user entered values
                else
                {
                    MessageBox.Show("Login Failed!  \nPlease check username and passsword", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txt_userName.Clear();
                    txt_password.Clear();
                }
            }
            // catch block
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_userName.Clear();
                txt_password.Clear();
            }
        }
    }
}
