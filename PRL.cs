using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Automobili
{
    public partial class PRL : Form
    {
        string prava;
        string korime;
        public PRL(string brugovora, string registracija, string model, string korisnik, string vozac, string dateOd, string dateDo, string pravaA, string korisnikIme)
        {
            InitializeComponent();
            loadDataGrid();
            AutocompleteReg();
            Autocompletekorisnik();
            Autocompletevozac();
            AutocompleteReg2();
            txtBrUgovora.Text = brugovora;
            brugovora = Regex.Match(txtBrUgovora.Text, @"\d+").Value;
            txtBrPRL.Text = brugovora;
            txtKorisnikPRL.Text = korisnik;
            txtVozacPRL.Text = vozac;
            txtZamjenaZaModel.Text = model;
            txtZamjenaZaReg.Text = registracija;
            DateTime dtOd = DateTime.Parse(dateOd);
            dateOdDana.Value = dtOd;
            DateTime dtDo = DateTime.Parse(dateDo);
            dateDoDana.Value = dtDo;
            prava = pravaA;
            korime = korisnikIme;
            dateOdDana.CustomFormat = "dd.MM.yyyy";
            dateOdDana.Format = DateTimePickerFormat.Custom;
            dateDoDana.CustomFormat = "dd.MM.yyyy";
            dateDoDana.Format = DateTimePickerFormat.Custom;
            if (txtBrUgovora.Text != "")
            {
                groupKojeSeMijenja.Visible = true;
            }
            
        }
        DataTable dbdataset;
        void loadDataGrid()
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            MySqlConnection connection = new MySqlConnection(constring);
            MySqlCommand cmdDataBase = new MySqlCommand("select brprl as 'Br.PRL', brugovora as 'Br.ugovora', regoznaka as 'Reg.oznaka'" +
                ",zamjenaZa as 'Model', regzamjenski as 'Reg.zamjenski', zamjenski as 'Model-zamjenski', od as 'Od datuma', do as 'Do datuma', dana as 'Uk.dana',korisnik as 'Korisnik', vozac as 'Vozač'," + 
                "napomena as 'Napomena', status as 'Status' from madjo.prl;", connection);

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
            connection.Close();
            if (txtBrUgovora.Text == "")
            {
                groupKojeSeMijenja.Visible = false;
            }
        }
        private void btnBrojDana_Click_1(object sender, EventArgs e)
        {
            string datumOd = dateOdDana.Text;
            string datumDo = dateDoDana.Text;
            DateTime mydatetime1 = new DateTime();
            DateTime mydatetime2 = new DateTime();
            mydatetime1 = DateTime.Parse(datumOd);
            mydatetime2 = DateTime.Parse(datumDo);
            TimeSpan ukDana1 = mydatetime2.Date - mydatetime1.Date;
            int ukDana = Convert.ToInt32(ukDana1.Days);
            ukDana++;
            txtUkDana.Text = ukDana.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtZamjVoz.Text != "" && txtBrPRL.Text != "")
            {
                var brugovora = Regex.Match(txtBrUgovora.Text, @"\d+").Value;
                string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
                string upit;
                if (txtZamjenaZaReg.Text == "")
                {
                    upit = "insert into madjo.prl (regoznaka, brugovora, brprl, od, regzamjenski, zamjenski, zamjenaZa, korisnik, vozac, napomena, status, otvorio, gorivo, kmzamjenski)" +
                                  "values('" + txtZamjenaZaReg.Text + "',+'" + txtBrUgovora.Text + "','" + txtBrPRL.Text +
                                  "','" + dateOdDana.Text + "','" + txtZamjVoz.Text + "','" + txtZamjVozModel.Text + "','" + txtZamjenaZaModel.Text +
                                  "','" + txtKorisnikPRL.Text + "','" + txtVozacPRL.Text + "','" + txtNapomena.Text + "', 'Otvoren', '" + korime + "','" + txtStGorivoZamjenski.Text + "','" + txtKmZamjenski.Text + "');" +
                                  "update madjo.automobili set status='Otvoreno na PRL' where regoznaka='" + txtZamjVoz.Text + "'; ";
                }
                else
                {
                    upit = "insert into madjo.prl (regoznaka, brugovora, brprl, od, regzamjenski, zamjenski, zamjenaZa, korisnik, vozac, napomena, status, otvorio, gorivo, kmzamjenski)" +
                                  "values('" + txtZamjenaZaReg.Text + "',+'" + txtBrUgovora.Text + "','" + txtBrPRL.Text +
                                  "','" + dateOdDana.Text + "','" + txtZamjVoz.Text + "','" + txtZamjVozModel.Text + "','" + txtZamjenaZaModel.Text +
                                  "','" + txtKorisnikPRL.Text + "','" + txtVozacPRL.Text + "','" + txtNapomena.Text + "', 'Otvoren', '" + korime + "','" + txtStGorivoZamjenski.Text + "','" + txtKmZamjenski.Text + "');" +
                                  "update madjo.automobili set status='Zamjena na PRL, reg." + txtZamjenaZaReg.Text + "' where regoznaka='" + txtZamjVoz.Text + "'; ";
                }

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
                    MessageBox.Show("PRL unešen");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + Environment.NewLine + "Provjerite dali je broj ugovora jedinsven.");
                }
                loadDataGrid();
            }
            else
            {
                MessageBox.Show("Provjerite dali ste unjeli sve podatke.");
            }
        }
        void AutocompleteReg()
        {
            txtZamjenaZaReg.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtZamjenaZaReg.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection kor = new AutoCompleteStringCollection();

            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.automobili where status !='Slobodno';";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string nazivV = citac.GetString("regoznaka");
                    kor.Add(nazivV);
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtZamjenaZaReg.AutoCompleteCustomSource = kor;
        }
        void AutocompleteReg2()
        {
            txtZamjVoz.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtZamjVoz.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection kor = new AutoCompleteStringCollection();

            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.automobili where status='Slobodno';";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string nazivV = citac.GetString("regoznaka");
                    kor.Add(nazivV);
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtZamjVoz.AutoCompleteCustomSource = kor;
        }
        void Autocompletekorisnik()
        {
            txtKorisnikPRL.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtKorisnikPRL.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection kor = new AutoCompleteStringCollection();

            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.korisnici;";
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
            txtKorisnikPRL.AutoCompleteCustomSource = kor;
        }
        void Autocompletevozac()
        {
            txtVozacPRL.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtVozacPRL.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoCompleteStringCollection kor = new AutoCompleteStringCollection();

            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "select * from madjo.vozac where nazivfirme='" + txtKorisnikPRL.Text + "';";
            MySqlConnection veza = new MySqlConnection(constring);
            MySqlCommand naredba = new MySqlCommand(upit, veza);
            MySqlDataReader citac;

            try
            {
                veza.Open();
                citac = naredba.ExecuteReader();
                while (citac.Read())
                {
                    string nazivV = citac.GetString("ime_prezime");
                    kor.Add(nazivV);
                }
                veza.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtVozacPRL.AutoCompleteCustomSource = kor;
        }

        private void txtZamjenaZaReg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                String selItem = this.txtZamjenaZaReg.Text;
                AutocompleteReg();
                string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
                string upit = "select * from madjo.automobili where regoznaka='" + txtZamjenaZaReg.Text + "' ;";
                MySqlConnection veza = new MySqlConnection(constring);
                MySqlCommand naredba = new MySqlCommand(upit, veza);
                MySqlDataReader citac;

                try
                {
                    veza.Open();
                    citac = naredba.ExecuteReader();
                    while (citac.Read())
                    {
                        txtZamjenaZaModel.Text = citac.GetString("model");
                    }
                    veza.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void txtKorisnikPRL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                String selItem = this.txtKorisnikPRL.Text;
                Autocompletevozac();
            }
        }

        private void txtVozacPRL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                String selItem = this.txtVozacPRL.Text;

                string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
                string upit = "select * from madjo.vozac where ime_prezime='" + selItem + "';";
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
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataView dw = new DataView(dbdataset);
            dw.RowFilter = string.Format("Br.PRL LIKE '%{0}%'", txtFbrPRL.Text);
            dataGridView1.DataSource = dw;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow red = this.dataGridView1.Rows[e.RowIndex];

                txtBrUgovora.Text = red.Cells["Br.ugovora"].Value.ToString();
                txtBrPRL.Text = red.Cells["Br.PRL"].Value.ToString();
                txtZamjenaZaReg.Text = red.Cells["Reg.oznaka"].Value.ToString();
                txtZamjenaZaModel.Text = red.Cells["Model"].Value.ToString();
                txtUkDana.Text = red.Cells["Uk.dana"].Value.ToString();
                txtKorisnikPRL.Text = red.Cells["Korisnik"].Value.ToString();
                txtZamjVoz.Text = red.Cells["Reg.zamjenski"].Value.ToString();
                txtZamjVozModel.Text = red.Cells["Model-zamjenski"].Value.ToString();

            }
        }

        private void txtZamjVoz_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                String selItem = this.txtZamjVoz.Text;
                AutocompleteReg2();
                string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
                string upit = "select * from madjo.automobili where regoznaka='" + txtZamjVoz.Text + "';";
                MySqlConnection veza = new MySqlConnection(constring);
                MySqlCommand naredba = new MySqlCommand(upit, veza);
                MySqlDataReader citac;

                try
                {
                    veza.Open();
                    citac = naredba.ExecuteReader();
                    while (citac.Read())
                    {
                        txtZamjVozModel.Text = citac.GetString("model");
                        txtKmZamjenski.Text = citac.GetString("stanjekm");
                    }
                    veza.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void btnZatvori_Click(object sender, EventArgs e)
        {
            if (txtZamjVoz.Text != "" && txtBrPRL.Text != "" && txtStatusZaklucivanje.Text != "")
            {
                string datumOd = dateOdDana.Text;
                string datumDo = dateDoDana.Text;
                DateTime mydatetime1 = new DateTime();
                DateTime mydatetime2 = new DateTime();
                mydatetime1 = DateTime.Parse(datumOd);
                mydatetime2 = DateTime.Parse(datumDo);
                TimeSpan ukDana1 = mydatetime2.Date - mydatetime1.Date;
                int ukDana = Convert.ToInt32(ukDana1.Days);
                ukDana++;
                txtUkDana.Text = ukDana.ToString();
                string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
                string upit = "update madjo.automobili set status='" + txtStatusZaklucivanje.Text + "' where regoznaka='" + txtZamjVoz.Text + "'; " +
                              "update madjo.prl set status='Zaključen', zatvorio='" + korime + "', do='" + dateDoDana.Text + "', dana='"
                              + txtUkDana.Text + "' where brprl='" + txtBrPRL.Text + "' ;";

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
                    MessageBox.Show("PRL Zaključen.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + Environment.NewLine + "Greška.");
                }
                loadDataGrid();
            }

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            DataView dw = new DataView(dbdataset);
            dw.RowFilter = string.Format("Br.ugovora LIKE '%{0}%'", txtFiltBrUg.Text);
            dataGridView1.DataSource = dw;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string constring = "datasource=localhost;port=3306;username=root2;password=madtom54";
            string upit = "delete from madjo.prl where brprl='" + txtBrPRL.Text + "';";
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
                loadDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
            
        
}
