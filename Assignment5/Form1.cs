using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment5
{
    public partial class Form1 : Form
    {
       
        private SQLiteConnection sqlite_conn;
        public Form1()
        {
            InitializeComponent();
            SQLiteConnection();
        }
        private void SQLiteConnection()
        {
            sqlite_conn = new SQLiteConnection("Data Source=Employee.db;Version=3;New=True;Compress=True;");
            try
            {
                sqlite_conn.Open();
                Database();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Database()
        {
            using(SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand())
        {
                string createTable = "CREATE TABLE IF NOT EXISTS Employees (EmpId INTEGER PRIMARY KEY, EmpName TEXT(50), EmpGender CHAR(1), EmpHiringDate DATE)";
                sqlite_cmd.CommandText = createTable;
                sqlite_cmd.ExecuteNonQuery();
            }
        }
        private void ReadData()
        {
            try
            {
                string query = "SELECT * FROM Employees";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, sqlite_conn);
                DataTable table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "INSERT INTO Employees (EmpName, EmpGender, EmpHiringDate) VALUES (@EmpName, @EmpGender, @EmpHiringDate)";
                using (SQLiteCommand add = new SQLiteCommand(query, sqlite_conn))
                {
                    add.Parameters.AddWithValue("@EmpName", txtEmpName.Text);
                    add.Parameters.AddWithValue("@EmpGender", txtEmpGender.Text);
                    DateTime hiringDate;
                    if (DateTime.TryParseExact(txtEmpHiringDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out hiringDate))
                    {
                        add.Parameters.AddWithValue("@EmpHiringDate", hiringDate);
                    }
                    else
                    {
                        MessageBox.Show("Invalid date format. Please enter the date in the format yyyy-MM-dd.");
                        return;
                    }

                    add.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ReadData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "UPDATE Employees SET EmpName=@EmpName, EmpGender=@EmpGender, EmpHiringDate=@EmpHiringDate WHERE EmpId=@EmpId";
                using (SQLiteCommand update = new SQLiteCommand(query, sqlite_conn))
                {
                    update.Parameters.AddWithValue("@EmpName", txtEmpName.Text);
                    update.Parameters.AddWithValue("@EmpGender", txtEmpGender.Text);
                    update.Parameters.AddWithValue("@EmpId", txtEmpId.Text);
                    DateTime hiringDate;
                    if (DateTime.TryParseExact(txtEmpHiringDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out hiringDate))
                    {
                        update.Parameters.AddWithValue("@EmpHiringDate", hiringDate.ToString("yyyy-MM-dd"));
                    }
                    else
                    {
                        MessageBox.Show("Invalid date format. Please enter the date in the format yyyy-MM-dd.");
                        return;
                    }
                    update.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ReadData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "DELETE FROM Employees WHERE EmpId=@EmpId";
                using (SQLiteCommand delete = new SQLiteCommand(query, sqlite_conn))
                {
                    delete.Parameters.AddWithValue("@EmpId", txtEmpId.Text);
                    delete.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            ReadData();
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            ReadData();
        }
    }
}
