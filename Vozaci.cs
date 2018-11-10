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

namespace Automobili
{
    public partial class Vozaci : Form
    {
        public Vozaci(string nazivFirme)
        {
            InitializeComponent();
            LoadDatagrid();
            txtNazivF.Text = nazivFirme;
            dateRodenja.CustomFormat = "dd.MM.yyyy";
            dateRodenja.Format = DateTimePickerFormat.Custom;
        }
        DataTable dbdataset;
        void LoadDatagrid()
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            MySqlConnection connection = new MySqlConnection(constring);
            MySqlCommand cmdDataBase = new MySqlCommand("select nazivfirme as 'Naziv firme'" +
                ", ime_prezime as 'Ime i prezime', datrodenja as 'Datum rođenja', mjestorodjenja as 'Mjesto rođenja', brojosobne as" + 
                " 'Broj osobne' , brojvozacke as 'Broj vozačke', adresa as 'Adresa', brojputovnice as 'Broj putovnice' from madjo.vozac;", connection);

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

        private void Vozaci_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "insert into madjo.vozac (nazivfirme, ime_prezime, datrodenja, mjestorodjenja, brojosobne, brojvozacke," +
                          "adresa, brojputovnice) values ('" + txtNazivF.Text + "','" + txtIme.Text + "','" + dateRodenja.Text + "','" +
                          txtMjRod.Text + "','" + txtBrOsIs.Text + "','" + txtBrVozDoz.Text + "','" + txtAdresa.Text + "','" + txtBrPut.Text + "');";

            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {

                }
                veza.Close();
                MessageBox.Show("Vozač unešen.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + Environment.NewLine + "Provjerite dali je broj ugovora jedinsven.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "delete from madjo.vozac where ime_prezime='" + txtIme.Text + "';";

            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {

                }
                veza.Close();
                MessageBox.Show("Vozač izbrisan.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
