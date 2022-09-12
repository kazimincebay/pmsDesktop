using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace pmsDesktop
{
    public partial class Form1 : Form
    {
        public string update;
        public int personId;
        public Form1()
        {
            InitializeComponent();
            fillDataGrid();
            getCompanies();
            textBox1.MaxLength = 11;
            dateTimePicker1.CustomFormat = "dd-MM-yyyy";
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
        }


        public void fillDataGrid()
        {
            dataGridView1.DataSource = getpersons();
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
            var button = new DataGridViewButtonColumn();
            dataGridView1.Columns.Add(button);
            button.Text = "Güncelle";
            button.HeaderText="Güncelle";
            button.UseColumnTextForButtonValue = true;
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
            comboBox2.Items.Add("Şirket Seçiniz");
            while (reader.Read())
            {

                comboBox2.Items.Add(reader["company"]);
                comboBox2.SelectedIndex = 0;
            }
            return dTable;
        }




        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }




        private void button3_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var valid = true;
            var TCKontrol = TCkontrol(textBox1.Text);




            if (textBox1.Text.Length != 11)
            {
                label8.Text = "T.C. Kimlik Numarası 11 Karakter Olmalıdır.";
                label8.ForeColor = Color.Red;
                valid = false;
            }
            if (SayiMi(textBox1.Text.ToString()) == false)
            {
                label8.Text = "T.C. Kimlik Numarası Harf içermemelidir.";
                label8.ForeColor = Color.Red;
                valid = false;
            }


            if (TCKontrol == false)
            {
                label8.Text = "Geçersiz T.C Kimlik Numarası";
                label8.ForeColor = Color.Red;
                valid = false;
            }
            else
            {
                label8.Visible = false;
            }

            if (this.textBox2.Text == "")
            {
                label9.Text = "Ad Soyad Boş Olamaz";
                label9.ForeColor = Color.Red;
                valid = false;
            }
            if (comboBox2.Text == "Şirket Seçiniz" || comboBox2.SelectedIndex == 0)
            {
                label11.Text = "Şirket Seçimi Yapınız";
                label11.ForeColor = Color.Red;
                valid = false;
            }
            else
            {
                label11.Visible = false;
            }



            if (this.textBox3.Text == "")
            {
                label10.Text = "Ünvan Boş Olamaz";
                label10.ForeColor = Color.Red;
                valid = false;
            }
            if (valid == true)
            {
                addPerson();
            }




        }
        static bool SayiMi(string text)
        {
            foreach (char chr in text)
            {
                if (!Char.IsNumber(chr)) { return false; }
                else
                {
                    return true;
                };


            }
            return true;
        }

        static bool TCkontrol(string value)
        {

            var TCtek = Convert.ToInt32(value[0].ToString()) + Convert.ToInt32(value[2].ToString()) + Convert.ToInt32(value[4].ToString()) + Convert.ToInt32(value[6].ToString()) + Convert.ToInt32(value[8].ToString());
            var TCcift = Convert.ToInt32(value[1].ToString()) + Convert.ToInt32(value[3].ToString()) + Convert.ToInt32(value[5].ToString()) + Convert.ToInt32(value[7].ToString());
            var TCtek7x = TCtek * 7;
            var rakam10 = (TCtek7x - TCcift) % 10;
            var rakam11toplam = Convert.ToInt32(value[0].ToString()) + Convert.ToInt32(value[1].ToString()) + Convert.ToInt32(value[2].ToString()) + Convert.ToInt32(value[3].ToString()) + Convert.ToInt32(value[4].ToString()) + Convert.ToInt32(value[5].ToString()) + Convert.ToInt32(value[6].ToString()) + Convert.ToInt32(value[7].ToString()) + Convert.ToInt32(value[8].ToString()) + Convert.ToInt32(value[9].ToString());
            var rakam11 = rakam11toplam % 10;


            if (value[9] == rakam10 && value[10] == rakam11)
            {
                return true;
            }
            else { return false; }
        }
        private DataTable addPerson()
        {
            DataTable dTable = new DataTable();
            string connString = ConfigurationManager.ConnectionStrings["dbx"].ConnectionString;

            SqlConnection con = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand("up_addOnayperson", con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@personTC", textBox1.Text);
            cmd.Parameters.AddWithValue("@personFullname", textBox2.Text);
            cmd.Parameters.AddWithValue("@personTitle", textBox3.Text);
            cmd.Parameters.AddWithValue("@personBirthTime", dateTimePicker1.Value);
            cmd.Parameters.AddWithValue("@personCompany", comboBox2.SelectedItem);
            cmd.Parameters.AddWithValue("@personState", "--");
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            dTable.Load(reader);
            return dTable;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SayiMi(textBox1.Text);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.Text != "Şirket Seçiniz" || comboBox2.SelectedIndex != 0)
            {
                label11.Visible = false;
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 0)
            {
                var personId = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                Form4 f4 = new Form4()
                {
                    personId = Convert.ToInt32(personId)

                };
                f4.Show();
            }





        }

        private void Form1_Load(object sender, EventArgs e)
        {
            fillDataGrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fillDataGrid();
        }
    }
}
