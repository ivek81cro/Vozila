using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace Automobili
{
    public partial class Kategorije : Form
    {
        public Kategorije()
        {
            InitializeComponent();
            loadDatagrid();
        }
        DataTable dbdataset;
        void loadDatagrid()
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            MySqlConnection connection = new MySqlConnection(constring);
            MySqlCommand cmdDataBase = new MySqlCommand("select kategorija as 'Kategorija'" +
                ", cijena as 'Cijena/dan' from madjo.kategorije;", connection);

            try
            {
                MySqlDataAdapter sda = new MySqlDataAdapter();
                sda.SelectCommand = cmdDataBase;
                dbdataset = new DataTable();
                sda.Fill(dbdataset);
                BindingSource bsource = new BindingSource();

                bsource.DataSource = dbdataset;
                dataGridView1.DataSource = bsource;
                sda.Update(dbdataset);

                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnUnesi_Click(object sender, EventArgs e)
        {
            NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberGroupSeparator = "";
            nfi.NumberDecimalSeparator = ",";
            txtKategorija.CharacterCasing = CharacterCasing.Upper;
            double cijena1 = Double.Parse(txtCijena.Text);
            string cijena = cijena1.ToString("N", nfi);
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "insert into madjo.kategorije (kategorija, cijena) values ('" + txtKategorija.Text + "','" + cijena + "');";
            MySqlConnection bazaspoj = new MySqlConnection(constring);
            MySqlCommand bazazapovjed = new MySqlCommand(upit, bazaspoj);
            MySqlDataReader citaj;
            try
            {
                bazaspoj.Open();
                citaj = bazazapovjed.ExecuteReader();
                MessageBox.Show("Novi zapis spremljen.");
                while (citaj.Read())
                {

                }
                bazaspoj.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            loadDatagrid();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if (ch == 44 && txtCijena.Text.IndexOf (',') != -1)
            {
                e.Handled = true;
                return;
            }

            if (!Char.IsDigit(ch) && ch != 8 && ch !=44)
            {
                e.Handled = true;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow red = this.dataGridView1.Rows[e.RowIndex];

                txtCijena.Text = red.Cells["Cijena/dan"].Value.ToString();
                txtKategorija.Text= red.Cells["Kategorija"].Value.ToString();

            }
        }

        private void btnPromijeni_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "update madjo.kategorije set cijena='" + txtCijena.Text + "' where kategorija ='" + txtKategorija.Text + "';";
            MySqlConnection bazaspoj = new MySqlConnection(constring);
            MySqlCommand bazazapovjed = new MySqlCommand(upit, bazaspoj);
            MySqlDataReader citaj;
            try
            {
                bazaspoj.Open();
                citaj = bazazapovjed.ExecuteReader();
                MessageBox.Show("Novi zapis spremljen.");
                while (citaj.Read())
                {

                }
                bazaspoj.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            loadDatagrid();
        }
    }
}
