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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            getCompanies();
        }
        private DataTable getCompanies()
        {
            DataTable dTable = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;

            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("sel_listAuths", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["company"]);
                comboBox1.SelectedIndex=0;
            }

            return dTable;
        }


    }
}
