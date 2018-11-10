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
using CrystalDecisions.CrystalReports;

namespace Automobili
{
    public partial class Narudj_pregled : Form
    {
        string korImeA;
        public Narudj_pregled(string korIme)
        {
            InitializeComponent();
            LoadDatagrid();
            korImeA = korIme;
        }
        DataTable dbdataset;
        void LoadDatagrid()
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            MySqlConnection connection = new MySqlConnection(constring);
            MySqlCommand cmdDataBase = new MySqlCommand("select brnarudb as 'Br.narudzbenice', registracija as 'Reg.oznaka'" +
                ", model as 'Model', stanjekm as 'Stanje(km)', stanjegoriva as 'Stanje goriva', partner as 'Pos.partner'," +
                " napomena as 'Napomena', datum as 'Datum', status as 'Status', zakljucio as 'Zakljucio' from madjo.narudbenice;", connection);

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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataView dw = new DataView(dbdataset);
            dw.RowFilter = string.Format("Reg.oznaka LIKE '%{0}%'", txtRegFilter.Text);
            dataGridView1.DataSource = dw;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow red = this.dataGridView1.Rows[e.RowIndex];

                txtBrNar.Text = red.Cells["Br.narudzbenice"].Value.ToString();
                txtReg.Text = red.Cells["Reg.oznaka"].Value.ToString();
            }
        }

        private void btnZakljuci_Click(object sender, EventArgs e)
        {
            DateTime danas = DateTime.Today.Date;
            string danasDan = danas.ToString("dd-MM-yyyy");
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "update madjo.narudbenice set zakljucio='" + korImeA + "', status='Zakljuceno', datumzatvaranja='" + danasDan + "' where brnarudb='" + txtBrNar.Text + "';" +
                          "update madjo.automobili set status='" + txtStatus.Text + "' where regoznaka='" + txtReg.Text + "';";

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
                LoadDatagrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //greška jer je u bazi int
        private void txtFbrNar_TextChanged(object sender, EventArgs e)
        {
            DataView dw = new DataView(dbdataset);
            dw.RowFilter = string.Format("Br.narudzbenice LIKE '%{0}%'", txtFbrNar.Text);
            dataGridView1.DataSource = dw;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Nar_ispis_sve narudIspis = new Nar_ispis_sve();
            narudIspis.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "delete from madjo.narudbenice where brnarudb='" + txtBrNar.Text + "';";

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
                LoadDatagrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonIspisNar_Click(object sender, EventArgs e)
        {
            NarIspis ni = new NarIspis();
            ni.Show();
            ni.Refresh();
        }
    }
}
