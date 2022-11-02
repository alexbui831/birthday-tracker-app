using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace birthdayApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBoxFName.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Data Source: name of server on which database resides
            // Initial Catalog: specifies the name of database


            // sqlcommand is a class used to perofrm read/write to database
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string connectString = @"data source=SUPERBYTE\SQLEXPRESS;initial catalog=dbBirthday;trusted_connection=true";
            SqlConnection cnn = new SqlConnection(connectString);
            cnn.Open();

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
                "SELECT firstName as 'First Name', lastName as 'Last Name', birthday as Birthday " +
                "FROM users " +
                "ORDER BY (MONTH(birthday) - MONTH(GETDATE()) + 12) % 12, DATEADD(year, YEAR(GETDATE()) - YEAR(birthday), birthday), YEAR(birthday);", cnn);

            DataTable dtbl = new DataTable();
            sqlDataAdapter.Fill(dtbl);

            BirthdayDataGrid.DataSource = dtbl;
            cnn.Close();
        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
            string connectString = @"data source=SUPERBYTE\SQLEXPRESS;initial catalog=dbBirthday;trusted_connection=true";
            SqlConnection cnn = new SqlConnection(connectString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) " +
                "FROM users " +
                "WHERE firstName LIKE '%" + textBoxFName.Text + "%' AND lastName LIKE '%" + textBoxLName.Text + "%'", cnn);
            SqlDataReader myreader;

            myreader = cmd.ExecuteReader();
            myreader.Read();

            string value = myreader[0].ToString();

            myreader.Close();
            if ((Int32.Parse(value) == 0) && (textBoxFName.Text != "" && textBoxLName.Text != ""))
            {
                SqlCommand sqlINSERT = new SqlCommand(
                    "INSERT INTO users (firstName, lastName, birthday) VALUES (@firstName, @lastName, @birthday)", cnn);

                sqlINSERT.Parameters.AddWithValue("@firstName", textBoxFName.Text);
                sqlINSERT.Parameters.AddWithValue("@lastName", textBoxLName.Text);
                sqlINSERT.Parameters.AddWithValue("@birthday", dateTimePicker.Value);

                sqlINSERT.ExecuteNonQuery();

                MessageBox.Show("Successful");

                cnn.Close();
                }
            else
            {
                MessageBox.Show("Issue");
            }
            
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            string connectString = @"data source=SUPERBYTE\SQLEXPRESS;initial catalog=dbBirthday;trusted_connection=true";
            SqlConnection cnn = new SqlConnection(connectString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) " +
                "FROM users " +
                "WHERE firstName LIKE '%" + textBoxFName.Text + "%' AND lastName LIKE '%" + textBoxLName.Text + "%'", cnn);
            SqlDataReader myreader;

            myreader = cmd.ExecuteReader();
            myreader.Read();

            string value = myreader[0].ToString();

            myreader.Close();

            // Int32 converts string into integer
            // insures updating of only 1 record at a time
            if (Int32.Parse(value) == 1)
            {

                SqlCommand sqlUPDATE = new SqlCommand(
                    "UPDATE users SET birthday = '" + dateTimePicker.Value + "' " +
                    "WHERE firstName  LIKE '%" + textBoxFName.Text + "%' AND lastName LIKE '%" + textBoxLName.Text + "%'", cnn);
                sqlUPDATE.ExecuteNonQuery();

                MessageBox.Show("Update successful.");
            } 
            else
            {
                MessageBox.Show("Name not distinct.");
            }
    
            cnn.Close();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string connectString = @"data source=SUPERBYTE\SQLEXPRESS;initial catalog=dbBirthday;trusted_connection=true";
            SqlConnection cnn = new SqlConnection(connectString);
            cnn.Open();

            SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(*) " +
                "FROM users " +
                "WHERE firstName LIKE '%" + textBoxFName.Text + "%' AND lastName LIKE '%" + textBoxLName.Text + "%'", cnn);
            SqlDataReader myreader;

            myreader = cmd.ExecuteReader();
            myreader.Read();

            string value = myreader[0].ToString();

            myreader.Close();

            // insures deletion of only 1 record at a time
            if ((Int32.Parse(value) == 1) && (textBoxFName.Text != "" && textBoxLName.Text != ""))
            {

                SqlCommand sqlDELETE = new SqlCommand(
                    "DELETE FROM users " +
                    "WHERE firstName = '" + textBoxFName.Text + "' AND lastName ='" + textBoxLName.Text + "'", cnn);

                sqlDELETE.ExecuteNonQuery();

                MessageBox.Show("Deleted user success.");
            }
            else
            {
                MessageBox.Show("Name not distinct.");
            }

            cnn.Close();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            this.InitializeComponent();

            string connectString = @"data source=SUPERBYTE\SQLEXPRESS;initial catalog=dbBirthday;trusted_connection=true";
            SqlConnection cnn = new SqlConnection(connectString);
            cnn.Open();

            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(
                "SELECT firstName as 'First Name', lastName as 'Last Name', birthday as Birthday " +
                "FROM users " +
                "ORDER BY (MONTH(birthday) - MONTH(GETDATE()) + 12) % 12, DATEADD(year, YEAR(GETDATE()) - YEAR(birthday), birthday), YEAR(birthday)", cnn);

            DataTable dtbl = new DataTable();
            sqlDataAdapter.Fill(dtbl);

            BirthdayDataGrid.DataSource = dtbl;
            cnn.Close();
        }
    }
}
