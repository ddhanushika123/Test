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
    public partial class studetDetails : Form
    {
        // Define all variables
        public SqlConnection conn;
        public SqlCommand cmd;
        public SqlDataAdapter adt;
       

        int code,index,line = 0;        
        String fName, lName;
        DateTime dob;


        // Create class constructer to load component with form when start
        public studetDetails()
        {
            InitializeComponent();
            lbl_sCode.Visible = false;      
            btn_delete.Visible = false;     // When open form that Delete button will not visible
            loadDataGrid();                 // Call loadDataGrid method 
        }

        private void studetDetails_Load(object sender, EventArgs e)
        {
            panel1.BackColor = Color.FromArgb(100,0,0,0);
            panel2.BackColor = Color.FromArgb(100,0,0,0);
        }


        // Create method for student_details table data load to DataGrideView
        public void loadDataGrid()
        {
            // try block
            try
            {
                // Get databse connection
                Database db = new Database();
                db.connection();

                // to open the connection
                db.con.Open();

                // Create the SQL command
                cmd = new SqlCommand("SELECT * FROM student_details",db.con);
                adt = new SqlDataAdapter();                     // data adapter object is use to insert, update or delete commands  
                adt.SelectCommand = cmd;
                DataTable dt = new DataTable();
                adt.Fill(dt);
                dataGridView1.DataSource = dt;

                // to close the connection 
                db.con.Close();

            }
            //catch block
            catch (Exception)
            {
                MessageBox.Show(" Can not load data to datadridview. Something went wrong. \n Please check and try again later", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

        // For clear text boxes data and refresh DataGridView
        public void clearData()
        {
            txt_code.Clear();
            txt_firstName.Clear();
            txt_lastName.Clear();
            txt_search.Clear();
            txt_search.Text = "     Enter student code or name for search";
            txt_search.ForeColor = Color.Silver;
            dateTimePicker1.Value = DateTime.Today;
            lbl_sCode.Visible = false;

            loadDataGrid();
            txt_code.Visible = true;
            btn_save.Text = "SAVE";
            btn_delete.Visible = false;
        }


        // Click clear button
        private void btn_clear_Click(object sender, EventArgs e)
        {
            //try block
            try
            {
                // Call clearData method
                clearData();
                
            }
            catch (Exception)
            {
                MessageBox.Show(" Can not clear data. Something went wrong. \n Please check and try again later", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }

        }

        
        // Click save button
        private void btn_save_Click(object sender, EventArgs e)
        {    
            //try block
            try
            {
                // Get database connection
                Database db = new Database();
                db.connection();

                // If buttone text is "SAVE" then buttone work as submit buttone 
                if (btn_save.Text == "SAVE")
                {    
                    // Check did user entered student code and did user enter only numbers
                    if(txt_code.Text.Length > 0 && txt_code.Text.All(char.IsDigit))
                    {
                        errorProvider_code.Clear();     // Not display error

                        // Check user enter student code is new one or already has in database
                        // Create the SQL command 
                        cmd = new SqlCommand("SELECT Code FROM student_details WHERE Code = @c",db.con);

                        // Add the parameters
                        cmd.Parameters.AddWithValue("@c",txt_code.Text);


                        // If has maching student code with user enterd values then it assign to line variable
                        adt = new SqlDataAdapter(cmd);          // data adapter object is use to insert, update or delete commands    
                        DataSet ds = new DataSet();
                        adt.Fill(ds);

                        line = ds.Tables[0].Rows.Count;

                        // If user enterd student code already has in databae
                        if (line == 1)
                        {
                            MessageBox.Show(" Student code  " + txt_code.Text + "  already have in database  \n Please enter new student code ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txt_code.Clear();

                        }
                        // If user entered student code already has not in database
                        else
                        {
                            // Check student first name
                            if (txt_firstName.Text.Length > 0 && !txt_firstName.Text.Any(char.IsDigit))
                            {
                                errorProvider_fName.Clear();

                                // Check student last name
                                if (txt_lastName.Text.Length > 0 && !txt_lastName.Text.Any(char.IsDigit))
                                {
                                    errorProvider_lName.Clear();

                                    // For check that Is user birthday select or not
                                    if (dateTimePicker1.Value != DateTime.Today)
                                    {
                                        errorProvider_dob.Clear();

                                        // User enter values assign to variables
                                        code = Convert.ToInt32(txt_code.Text);
                                        fName = txt_firstName.Text;
                                        lName = txt_lastName.Text;
                                        dob = dateTimePicker1.Value;


                                        // to open the connection 
                                        db.con.Open();

                                        // Create the SQL command 
                                        cmd = new SqlCommand("INSERT INTO student_details VALUES (@c,@f,@l,@d)", db.con);

                                        // Add the parameters
                                        cmd.Parameters.AddWithValue("@c", code);
                                        cmd.Parameters.AddWithValue("@f", fName);
                                        cmd.Parameters.AddWithValue("@l", lName);
                                        cmd.Parameters.AddWithValue("@d", dob);

                                        // Execute sql query
                                        line = cmd.ExecuteNonQuery();

                                        // closing all the objects 
                                        cmd.Dispose();

                                        // to close the connection
                                        db.con.Close();

                                        // If inserted data to databse
                                        if (line == 1)
                                        {
                                            MessageBox.Show("Student code : " + txt_code.Text + "\n Deatails successfuly Saved ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            clearData();
                                            loadDataGrid();
                                        }
                                        // If can not inserted data to database
                                        else
                                        {
                                            MessageBox.Show("Data can not Insert", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }

                                    // If have errors in user entered data then display error message from error providers
                                    }
                                    else
                                    {
                                        errorProvider_dob.SetError(dateTimePicker1, "Please select student birthday");
                                    }
                                }
                                else
                                {
                                    errorProvider_lName.SetError(txt_lastName, "You should enter student last name using only characters");
                                }
                            }
                            else
                            {
                                errorProvider_fName.SetError(txt_firstName, "You should enter student first name using only characters");
                            }
                        }
                    }
                    else
                    {
                        errorProvider_code.SetError(txt_code,"You should enter student code using only numbers ");
                    }
                }


                // If buttone text is "UPDATE" then buttone work as update buttone 
                else
                {
                    // For check that do user select correct data record in DataGridView
                    if (txt_firstName.Text.Length > 0)
                    {
                        // Check user enter value for student first name
                        if (txt_firstName.Text.Length > 0 && !txt_firstName.Text.Any(char.IsDigit))
                        {
                            errorProvider_fName.Clear();

                            // Check user enterd value for student last name
                            if (txt_lastName.Text.Length > 0 && !txt_lastName.Text.Any(char.IsDigit))
                            {
                                errorProvider_lName.Clear();

                                // Check Is user selecte student birthday or not
                                if (dateTimePicker1.Value != DateTime.Today)
                                {
                                    errorProvider_dob.Clear();


                                    // Get user enter data to variables
                                    code = Convert.ToInt32(lbl_sCode.Text);
                                    fName = txt_firstName.Text;
                                    lName = txt_lastName.Text;
                                    dob = dateTimePicker1.Value;

                                    // Get confermation to update data
                                    DialogResult dr = MessageBox.Show("Do you want to update the selected row student details? ", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                    // If user click yes buttone in dialog box
                                    if (dr.ToString() == "Yes")
                                    {
                                        // to open the connection
                                        db.con.Open();

                                        // Create the SQL command 
                                        cmd = new SqlCommand("UPDATE student_details SET FirstName =@f, LastName = @l, DOB = @d WHERE Code = @no", db.con);

                                        // Add the parameters
                                        cmd.Parameters.AddWithValue("@no", code);
                                        cmd.Parameters.AddWithValue("@f", fName);
                                        cmd.Parameters.AddWithValue("@l", lName);
                                        cmd.Parameters.AddWithValue("@d", dob);

                                        line = cmd.ExecuteNonQuery();

                                        // closing all the objects 
                                        cmd.Dispose();

                                        // to close the connection
                                        db.con.Close();

                                        MessageBox.Show(" Student code : " + txt_code.Text + "\n Details updated ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        clearData();        // Call cleardata method
                                        loadDataGrid();     // Call loadDataGrid method
                                    }
                                    // If user click no buttone about dialog box
                                    else
                                    {
                                        MessageBox.Show("Data will not update", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }

                                // If have errors in user entered data then display error message from error providers
                                }
                                else
                                {
                                    errorProvider_dob.SetError(dateTimePicker1, "Please select student birthday");
                                }
                            }
                            else
                            {
                                errorProvider_lName.SetError(txt_lastName, "You should enter student last name using only characters");
                            }
                        }
                        else
                        {
                            errorProvider_fName.SetError(txt_firstName, "You should enter student first name using only characters");
                        }
                    }

                    // If user select error data record in DataGridView
                    else
                    {
                        MessageBox.Show(" Can not update. \n Please select correct record of data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }                                    
            }
            // catch block
            catch(Exception)
            {
                MessageBox.Show("Can not save data. Something went wrong. \n Please check and try again later", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
            }
        }

        

        

        
        // When select data recors in DataGridView then pass that values to text boxes,labels,dateTimePicker
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            index = e.RowIndex;
            DataGridViewRow selectedRow = dataGridView1.Rows[index];

            txt_code.Visible = false;
            lbl_sCode.Visible = true;
            btn_delete.Visible = true;
            
            btn_save.Text = "UPDATE";           
            btn_save.BackColor = Color.Yellow;

            lbl_sCode.Text = selectedRow.Cells[0].Value.ToString();
            txt_code.Text = selectedRow.Cells[0].Value.ToString();
            txt_firstName.Text = selectedRow.Cells[1].Value.ToString();
            txt_lastName.Text = selectedRow.Cells[2].Value.ToString();
            dateTimePicker1.Text = selectedRow.Cells[3].Value.ToString();
        }


        // When mouse point click on txt_search box then clear text box
        private void txt_search_Enter(object sender, EventArgs e)
        {
            if (txt_search.Text == "     Enter student code or name for search")
            {
                txt_search.Text = "";
                txt_search.ForeColor = Color.Black;
            }
        }

        // When mouse point getout from txt_search box then display some text on the text box
        private void txt_search_Leave(object sender, EventArgs e)
        {
            if (txt_search.Text == "")
            {
                txt_search.Text = "     Enter student code or name for search";
                txt_search.ForeColor = Color.Silver;
            }
        }

        // Click search button
        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                // Get databse connectin
                Database db = new Database();
                db.connection();

                // To open the connection
                db.con.Open();

                // Create the SQL command 
                cmd = new SqlCommand("SELECT * FROM student_details WHERE Code LIKE '%" + txt_search.Text + "%' OR FirstName LIKE '%" + txt_search.Text + "%' OR LastName LIKE '%" + txt_search.Text + "%'", db.con);

                adt = new SqlDataAdapter();
                adt.SelectCommand = cmd;
                DataTable dt = new DataTable();
                adt.Fill(dt);
                dataGridView1.DataSource = dt;

                // To close the connection
                db.con.Close();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        // Click delete button
        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                Database db = new Database();
                db.connection();

                code = Convert.ToInt32(lbl_sCode.Text);

                // Get confermation to delete selected record
                DialogResult dr = MessageBox.Show("Do you want to delete the selected row ?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // If user select yes button from in dialog box
                if (dr.ToString() == "Yes")
                {
                    db.con.Open();

                    cmd = new SqlCommand("DELETE FROM student_details WHERE code = @no", db.con);
                    cmd.Parameters.AddWithValue("@no", code);

                    line = cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    db.con.Close();

                    if (line == 1)
                    {
                        MessageBox.Show(" Student code : " + lbl_sCode.Text + "\n Details deleted ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }
                    else
                    {
                        MessageBox.Show("Data can not Delete", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    loadDataGrid();         // For refresh DataGridView
                    clearData();            // For text boxes
                }
            }
            catch (Exception)
            {                
                MessageBox.Show(" Can not delete. Something went wrong. \n Please check and try again later", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        } 


        // Click Sign Out button
        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Hide();
            login fm = new login();
            fm.Show();
        }         
    }
}
