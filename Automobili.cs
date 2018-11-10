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
using System.IO;

namespace Automobili
{
    public partial class automobili : Form
    {
        string pravaA;
        string korisnikIme;
        public automobili(string prava, string korisnik)
        {
            InitializeComponent();
            pravaA = prava;
            korisnikIme = korisnik;
            LoadDatagrid();
            Autocompletekorisnik();
            popuniComboboxKor();
            popuniComboboxSD();
            popuniComboboxSektor();
            if (prava != "0")
            {
                btnBrisi.Visible = false;
                btnUnesi.Visible = true;
                btnPromjeni.Visible = false;
            }
            int brojRedova = dataGridView1.RowCount;
            label8.Text = brojRedova.ToString();
            txtKorisnik.Text = "-";
            txtUprava.Text = "-";
            txtSektor.Text = "-";
        }
        DataTable dbdataset;
        void LoadDatagrid()
        {
                string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
                MySqlConnection connection = new MySqlConnection(constring);
                MySqlCommand cmdDataBase = new MySqlCommand("select regoznaka as 'Reg.oznaka', model as 'Model'" +
                    ", kategorija as 'Kategorija', sifra as 'Sifra', status as 'Status', korisnik as 'Korisnik'" +
                    ",uprava_sd_pf as 'Uprava_sd_pf', sektor as 'Sektor', stanjekm as 'Stanje km'," +
                    " stanjegoriva as 'Gorivo' from madjo.automobili;", connection);
                        
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
        void Autocompletekorisnik()
        {
            txtKorisnik.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtKorisnik.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection kor = new AutoCompleteStringCollection();

            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select DISTINCT naziv from madjo.korisnici ORDER BY naziv;";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string nazivK = citac.GetString("naziv");
                    kor.Add(nazivK);
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtKorisnik.AutoCompleteCustomSource = kor;
        }
        void popuniComboboxKor()
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select DISTINCT naziv from madjo.korisnici ORDER BY naziv;";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;
            txtKorisnik.Items.Clear();
            txtKorisnik.Text = "";
            txtKorisnik.Items.Add('-');

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string korisnici = citac.GetString("naziv");
                    txtKorisnik.Items.Add(korisnici);
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void popuniComboboxSD()
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select DISTINCT firma_uprava from madjo.uprava_sd where firma='" + txtKorisnik.Text + "' ORDER BY firma_uprava;";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;
            txtUprava.Items.Clear();
            txtUprava.Text = "-";
            txtUprava.Items.Add("-");

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string uprava = citac.GetString("firma_uprava");
                    if (uprava == "")
                        continue;
                    txtUprava.Items.Add(uprava);
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void popuniComboboxSektor()
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.uprava_sd where firma='" + txtKorisnik.Text + "' and firma_uprava='" + txtUprava.Text + "';";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;
            txtSektor.Items.Clear();
            txtSektor.Text = "-";
            txtSektor.Items.Add('-');

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string sektor = citac.GetString("sektor");
                    txtSektor.Items.Add(sektor);
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void ClearAllText(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c is TextBox)
                    ((TextBox)c).Clear();
                else
                    ClearAllText(c);
            }
        }

        private void btnUnesi_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "insert into madjo.automobili (regoznaka, sifra, kategorija, model, status, stanjekm)" +
                          "values ('" + txtReg.Text + "','" + txtSifra.Text + "','" + txtKat.Text + "','" +
                          txtModel.Text + "','" + txtStatus.Text + "','" + txtStanjeKm.Text + "');";

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            LoadDatagrid();

        }

        private void btnBrisi_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "delete from madjo.automobili where regoznaka='" + txtReg.Text +"';";

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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            LoadDatagrid();
        }

        private void btnPromjeni_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "update madjo.automobili set sifra='" + txtSifra.Text + "',kategorija='" + txtKat.Text +
                          "',model='" + txtModel.Text + "',status='" + txtStatus.Text + "',korisnik='" + txtKorisnik.Text + "',stanjekm='" + txtStanjeKm.Text +
                          "', uprava_sd_pf='" + txtUprava.Text + "', sektor='" + txtSektor.Text + "' where regoznaka='" + txtReg.Text + "';";

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
                ClearAllText(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            LoadDatagrid();
        }

        private void txtSifraFilter_TextChanged(object sender, EventArgs e)
        {
            DataView dw = new DataView(dbdataset);
            dw.RowFilter = string.Format("Status LIKE '%{0}%'", txtSifraFilter.Text);
            dataGridView1.DataSource = dw;
            int brojRedova = dataGridView1.RowCount;
            label8.Text = brojRedova.ToString();
        }

        private void txtRegFilter_TextChanged(object sender, EventArgs e)
        {
            DataView dw = new DataView(dbdataset);
            dw.RowFilter = string.Format("Reg.oznaka LIKE '%{0}%'", txtRegFilter.Text);
            dataGridView1.DataSource = dw;
            int brojRedova = dataGridView1.RowCount;
            label8.Text = brojRedova.ToString();
        }

        private void txtKategFilter_TextChanged(object sender, EventArgs e)
        {
            DataView dw = new DataView(dbdataset);
            dw.RowFilter = string.Format("Kategorija LIKE '%{0}%'", txtKategFilter.Text);
            dataGridView1.DataSource = dw;
            int brojRedova = dataGridView1.RowCount;
            label8.Text = brojRedova.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Automobili_ispis autIspis = new Automobili_ispis();
            autIspis.Show();
        }

        private void txtKorisnik_TextChanged(object sender, EventArgs e)
        {
            popuniComboboxSD();
            popuniComboboxSektor();
        }

        private void txtFkor_TextChanged(object sender, EventArgs e)
        {
            DataView dw = new DataView(dbdataset);
            dw.RowFilter = string.Format("Korisnik LIKE '%{0}%'", txtFkor.Text);
            dataGridView1.DataSource = dw;
        }

        private void txtUprava_TextChanged(object sender, EventArgs e)
        {
            popuniComboboxSektor();
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow red = this.dataGridView1.Rows[e.RowIndex];

                txtReg.Text = red.Cells["Reg.oznaka"].Value.ToString();
                txtSifra.Text = red.Cells["Sifra"].Value.ToString();
                txtKat.Text = red.Cells["Kategorija"].Value.ToString();
                txtModel.Text = red.Cells["Model"].Value.ToString();
                txtStatus.Text = red.Cells["Status"].Value.ToString();
                txtStanjeKm.Text = red.Cells["Stanje km"].Value.ToString();

            }
        }

        private void automobili_Activated(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
