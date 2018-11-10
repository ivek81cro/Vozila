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
    public partial class Vozaci_preglaed : Form
    {
        public Vozaci_preglaed()
        {
            InitializeComponent();
            LoadDatagrid();
        }
        DataTable dbdataset;
        void LoadDatagrid()
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            MySqlConnection connection = new MySqlConnection(constring);
            MySqlCommand cmdDataBase = new MySqlCommand("select nazivfirme as 'Naziv_tvrtke'" +
                ", ime_prezime as 'Ime_prezime', datrodenja as 'Datum rođenja', mjestorodjenja as 'Mjesto rođenja', brojosobne as" +
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataView dw = new DataView(dbdataset);
            dw.RowFilter = string.Format("Naziv_tvrtke LIKE '%{0}%'", txtFilFirma.Text);
            dataGridView1.DataSource = dw;
        }

        private void txtFiltIme_TextChanged(object sender, EventArgs e)
        {
            DataView dw = new DataView(dbdataset);
            dw.RowFilter = string.Format("Ime_prezime LIKE '%{0}%'", txtFiltIme.Text);
            dataGridView1.DataSource = dw;
        }
    }
}
