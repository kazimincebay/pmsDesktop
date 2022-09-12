using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pmsDesktop
{
    public partial class Form4 : Form
    {
        public int personId;
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            var personId = this.personId;
            var person = getperson();
            getperson();
            getCompanies();
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            comboBox2.Items.Add("Onaylandı");
            comboBox2.Items.Add("Reddedildi");


            for (int i = 0; i <= person.Rows.Count - 1; i++)
            {
                for (int j = 0; j <= person.Columns.Count - 1; j++)
                {
                    label8.Text = person.Rows[0][0].ToString();
                    textBox1.Text = person.Rows[0][1].ToString();
                    textBox2.Text = person.Rows[0][2].ToString();
                    textBox3.Text = person.Rows[0][3].ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(person.Rows[0][4]);
                    comboBox1.SelectedItem = person.Rows[0][5].ToString();

                    comboBox2.SelectedItem = person.Rows[0][6].ToString();
                }
            }


        }

        private DataTable getperson()
        {
            DataTable dTable = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;

            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("sel_listoneperson", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@personId",personId);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            dTable.Load(reader);
            return dTable;
        }

        private DataTable getCompanies()
        {
            DataTable dTable = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;

            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("sel_listAuthsHepsi", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            comboBox1.Items.Add("Şirket Seçiniz");
            while (reader.Read())
            {

                comboBox1.Items.Add(reader["company"]);
                comboBox1.SelectedIndex = 0;
            }
            return dTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updatePerson();
            Form1 f1 = new Form1();
            f1.fillDataGrid();
            f1.dataGridView1.Update();
            f1.dataGridView1.Refresh();
            f1.dataGridView1.Parent.Refresh();
            this.Close();

        }

        private DataTable getpersons()
        {
            DataTable dTable = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;

            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("sel_listperson", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            dTable.Load(reader);
            return dTable;
        }

        private DataTable updatePerson()
        {
            DataTable dTable = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;

            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("up_updatePerson", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@personId", label8.Text);
            cmd.Parameters.AddWithValue("@personTC", textBox1.Text);
            cmd.Parameters.AddWithValue("@personFullname", textBox2.Text);
            cmd.Parameters.AddWithValue("@personTitle", textBox3.Text);
            cmd.Parameters.AddWithValue("@personBirthTime", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@personCompany", comboBox1.SelectedItem);
            cmd.Parameters.AddWithValue("@personState", comboBox2.SelectedItem);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            dTable.Load(reader);
            return dTable;

        }


    }
}
